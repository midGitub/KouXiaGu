using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.IO;

namespace JiongXiaGu.Unity.Resources.Binding
{


    /// <summary>
    /// 通过特性定义序列化方式,统一进行序列化操作;
    /// </summary>
    public sealed class BindingSerializer<T>
    {
        private const BindingDeserializeOptions defaultBindingDeserializeOptions = BindingDeserializeOptions.None;
        private readonly Lazy<List<IMember>> members;
        public bool IsMemberComplete => members.IsValueCreated;
        public IEnumerable<IMember> Members => members.Value;

        public BindingSerializer()
        {
            var type = typeof(T);
            members = new Lazy<List<IMember>>(() => BuildMembers(type));
        }

        public BindingSerializer(IEnumerable<IMember> members)
        {
            this.members = new Lazy<List<IMember>>(() => new List<IMember>(members));
            this.members.Initialization();
        }

        #region Static

        /// <summary>
        /// 序列化;
        /// </summary>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ObjectDisposedException"></exception>
        /// <exception cref="InvalidOperationException"></exception>
        public static void Serialize(Content writableContent, T instance, IEnumerable<IMember> members)
        {
            if (writableContent == null)
                throw new ArgumentNullException(nameof(writableContent));
            if (instance == null)
                throw new ArgumentNullException(nameof(instance));
            if (members == null)
                throw new ArgumentNullException(nameof(members));
            if (writableContent.CanWrite)
                throw new InvalidOperationException(string.Format("[{0}] can not read;", nameof(Content)));
            if (writableContent.IsDisposed)
                throw new ObjectDisposedException(string.Format("[{0}] is Disposed;", nameof(Content)));

            foreach (var member in members)
            {
                Serialize(writableContent, instance, member);
            }
        }

        /// <summary>
        /// 序列化指定成员;
        /// </summary>
        /// <exception cref="ObjectDisposedException"></exception>
        /// <exception cref="InvalidOperationException"></exception>
        public static void Serialize(Content writableContent, T instance, IMember member)
        {
            using (var stream = writableContent.GetOutputStream(member.RelativePath))
            {
                var value = member.GetValue(instance);
                member.Serializer.Serialize(stream, value);
            }
        }


        /// <summary>
        /// 反序列化指定成员;
        /// </summary>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="FileNotFoundException"></exception>
        /// <exception cref="ObjectDisposedException"></exception>
        public static T Deserialize(Content content, T instance, IEnumerable<IMember> members, BindingDeserializeOptions options = defaultBindingDeserializeOptions)
        {
            if (content == null)
                throw new ArgumentNullException(nameof(content));
            if (instance == null)
                throw new ArgumentNullException(nameof(members));
            if (members == null)
                throw new ArgumentNullException(nameof(members));

            foreach (var member in members)
            {
                Deserialize(content, instance, member, options);
            }

            return instance;
        }

        /// <summary>
        /// 反序列化成员;
        /// </summary>
        /// <exception cref="FileNotFoundException"></exception>
        /// <exception cref="ObjectDisposedException"></exception>
        public static void Deserialize(Content content, T instance, IMember member, BindingDeserializeOptions options)
        {
            try
            {
                using (var stream = content.GetInputStream(member.RelativePath))
                {
                    var value = member.Serializer.Deserialize(stream);
                    member.SetValue(instance, value);
                }
            }
            catch (FileNotFoundException ex)
            {
                if ((options & BindingDeserializeOptions.IgnoreNonexistent) < 0)
                {
                    throw ex;
                }
            }
        }


        public const BindingFlags FieldBindingFlags = BindingFlags.Instance | BindingFlags.Public;
        public const BindingFlags PropertyBindingFlags = BindingFlags.Instance | BindingFlags.Public;

        /// <summary>
        /// 获取该类型成员信息;
        /// </summary>
        public static List<IMember> BuildMembers(Type type)
        {
            var members = new List<IMember>();

            var fields = type.GetFields(FieldBindingFlags);
            foreach (var field in fields)
            {
                var attribute = field.GetCustomAttribute<AssetAttribute>();
                if (attribute != null)
                {
                    var member = new Field(attribute, field);
                    members.Add(member);
                }
            }

            var properties = type.GetProperties(PropertyBindingFlags);
            foreach (var property in properties)
            {
                if (property.CanRead && property.CanWrite)
                {
                    var attribute = property.GetCustomAttribute<AssetAttribute>();
                    if (attribute != null)
                    {
                        var member = new Property(attribute, property);
                        members.Add(member);
                    }
                }
            }

            return members;
        }

        #endregion

        /// <summary>
        /// 序列化;
        /// </summary>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ObjectDisposedException"></exception>
        /// <exception cref="InvalidOperationException"></exception>
        public void Serialize(Content writableContent, T instance)
        {
            Serialize(writableContent, instance, members.Value);
        }

        /// <summary>
        /// 仅序列化指定成员;
        /// </summary>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ObjectDisposedException"></exception>
        /// <exception cref="InvalidOperationException"></exception>
        public void Serialize(Content writableContent, T instance, Func<IMember, bool> filter)
        {
            Serialize(writableContent, instance, members.Value.Where(filter));
        }

        /// <summary>
        /// 仅序列化指定变量;
        /// </summary>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ObjectDisposedException"></exception>
        /// <exception cref="InvalidOperationException"></exception>
        /// <exception cref="KeyNotFoundException"></exception>
        public void Serialize(Content writableContent, T instance, string name)
        {
            Serialize(writableContent, instance, GetMember(name));
        }

        /// <summary>
        /// 仅序列化指定变量;
        /// </summary>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ObjectDisposedException"></exception>
        /// <exception cref="InvalidOperationException"></exception>
        /// <exception cref="KeyNotFoundException"></exception>
        public void Serialize(Content writableContent, T instance, IEnumerable<string> names)
        {
            Serialize(writableContent, instance, GetMembers(names));
        }

        /// <summary>
        /// 仅序列化指定变量;
        /// </summary>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ObjectDisposedException"></exception>
        /// <exception cref="InvalidOperationException"></exception>
        /// <exception cref="KeyNotFoundException"></exception>
        public void Serialize(Content writableContent, T instance, params string[] names)
        {
            Serialize(writableContent, instance, names as IEnumerable<string>);
        }


        /// <summary>
        /// 反序列化所有成员;
        /// </summary>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="FileNotFoundException"></exception>
        /// <exception cref="ObjectDisposedException"></exception>
        public T Deserialize(Content content, T instance, BindingDeserializeOptions options = defaultBindingDeserializeOptions)
        {
            return Deserialize(content, instance, members.Value, options);
        }

        /// <summary>
        /// 反序列化指定成员;
        /// </summary>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="FileNotFoundException"></exception>
        /// <exception cref="ObjectDisposedException"></exception>
        public T Deserialize(Content content, T instance, Func<IMember, bool> filter, BindingDeserializeOptions options = defaultBindingDeserializeOptions)
        {
            return Deserialize(content, instance, members.Value.Where(filter), options);
        }

        /// <summary>
        /// 反序列化指定成员;
        /// </summary>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="FileNotFoundException"></exception>
        /// <exception cref="ObjectDisposedException"></exception>
        public T Deserialize(Content content, T instance, string name, BindingDeserializeOptions options = defaultBindingDeserializeOptions)
        {
            Deserialize(content, instance, GetMember(name), options);
            return instance;
        }

        /// <summary>
        /// 反序列化指定成员;
        /// </summary>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="FileNotFoundException"></exception>
        /// <exception cref="ObjectDisposedException"></exception>
        public T Deserialize(Content content, T instance, IEnumerable<string> names, BindingDeserializeOptions options = defaultBindingDeserializeOptions)
        {
            return Deserialize(content, instance, GetMembers(names), options);
        }

        /// <summary>
        /// 反序列化;(注意,该方法使用无参数构造函数创建 T,所以T必须要具有无参数构造函数)
        /// </summary>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="FileNotFoundException"></exception>
        /// <exception cref="ObjectDisposedException"></exception>
        /// <exception cref="MissingMethodException">为指定的类型 T 不具有无参数构造函数</exception>
        public static T Deserialize(Content content, IEnumerable<IMember> members, BindingDeserializeOptions options = defaultBindingDeserializeOptions)
        {
            var instance = Activator.CreateInstance<T>();
            return Deserialize(content, instance, members, options);
        }

        /// <summary>
        /// 反序列化;(注意,该方法使用无参数构造函数创建 T,所以T必须要具有无参数构造函数)
        /// </summary>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="FileNotFoundException"></exception>
        /// <exception cref="ObjectDisposedException"></exception>
        /// <exception cref="MissingMethodException">为指定的类型 T 不具有无参数构造函数</exception>
        public T Deserialize(Content content, BindingDeserializeOptions options = defaultBindingDeserializeOptions)
        {
            var instance = Activator.CreateInstance<T>();
            return Deserialize(content, instance, members.Value, options);
        }

        /// <summary>
        /// 获取到需要进行操作的成员;
        /// </summary>
        /// <exception cref="KeyNotFoundException"></exception>
        private IEnumerable<IMember> GetMembers(IEnumerable<string> names)
        {
            if (names == null)
                throw new ArgumentNullException(nameof(names));

            return names.Select(name => GetMember(members.Value, name));
        }

        /// <summary>
        /// 获取到对应成员,若不存在则返回异常;
        /// </summary>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="KeyNotFoundException"></exception>
        private IMember GetMember(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentNullException(nameof(name));

            return GetMember(members.Value, name);
        }

        /// <summary>
        /// 获取到对应成员,若不存在则返回异常;
        /// </summary>
        /// <exception cref="KeyNotFoundException"></exception>
        private IMember GetMember(List<IMember> members, string name)
        {
            var member = members.Find(item => item.Name.Equals(name, StringComparison.Ordinal));
            if (member != null)
            {
                return member;
            }
            else
            {
                throw new KeyNotFoundException(name);
            }
        }
    }
}
