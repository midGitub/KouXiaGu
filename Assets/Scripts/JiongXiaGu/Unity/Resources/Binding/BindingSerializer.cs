using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace JiongXiaGu.Unity.Resources.Binding
{

    /// <summary>
    /// 通过特性定义序列化方式,统一进行序列化操作;
    /// </summary>
    public sealed class BindingSerializer<T>
        where T : new()
    {
        private readonly Type type;
        private Lazy<List<IMember>> members;

        public BindingSerializer()
        {
            type = typeof(T);
            members = new Lazy<List<IMember>>(() => BuildMembers(type));
        }

        /// <summary>
        /// 序列化;
        /// </summary>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ObjectDisposedException"></exception>
        /// <exception cref="InvalidOperationException"></exception>
        public void Serialize(Content writableContent, T instance)
        {
            if (writableContent == null)
                throw new ArgumentNullException(nameof(writableContent));
            if (instance == null)
                throw new ArgumentNullException(nameof(instance));

            var members = GetMembers();
            foreach (var member in members)
            {
                Serialize(writableContent, instance, member);
            }
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
            if (writableContent == null)
                throw new ArgumentNullException(nameof(writableContent));
            if (instance == null)
                throw new ArgumentNullException(nameof(instance));
            if(string.IsNullOrWhiteSpace(name))
                throw new ArgumentNullException(nameof(name));

            var member = GetMember(name);
            if (member != null)
            {
                Serialize(writableContent, instance, member);
            }
            else
            {
                throw new KeyNotFoundException(name);
            }
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
            if (writableContent == null)
                throw new ArgumentNullException(nameof(writableContent));
            if (instance == null)
                throw new ArgumentNullException(nameof(instance));
            if (names == null)
                throw new ArgumentNullException(nameof(names));

            var members = GetMembers(names);
            foreach (var member in members)
            {
                Serialize(writableContent, instance, member);
            }
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
            Serialize(writableContent, instance, names);
        }

        /// <summary>
        /// 序列化指定成员;
        /// </summary>
        /// <exception cref="ObjectDisposedException"></exception>
        /// <exception cref="InvalidOperationException"></exception>
        private void Serialize(Content writableContent, T instance, IMember member)
        {
            using (var stream = writableContent.GetOutputStream(member.Info.RelativePath))
            {
                var value = member.GetValue(instance);
                member.Serializer.Serialize(stream, value);
            }
        }

        /// <summary>
        /// 反序列化;
        /// </summary>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="FileNotFoundException"></exception>
        /// <exception cref="ObjectDisposedException"></exception>
        public T Deserialize(Content content)
        {
            if (content == null)
                throw new ArgumentNullException(nameof(content));

            var instance = (T)Activator.CreateInstance(type);
            var members = GetMembers();

            foreach (var member in members)
            {
                Deserialize(content, instance, member);
            }

            return instance;
        }

        /// <summary>
        /// 反序列化;
        /// </summary>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="FileNotFoundException"></exception>
        /// <exception cref="ObjectDisposedException"></exception>
        /// <exception cref="KeyNotFoundException"></exception>
        public T Deserialize(Content content, string name)
        {
            if (content == null)
                throw new ArgumentNullException(nameof(content));
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentNullException(nameof(name));

            var instance = (T)Activator.CreateInstance(type);
            var member = GetMember(name);

            Deserialize(content, instance, member);

            return instance;
        }

        /// <summary>
        /// 反序列化;
        /// </summary>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="FileNotFoundException"></exception>
        /// <exception cref="ObjectDisposedException"></exception>
        /// <exception cref="KeyNotFoundException"></exception>
        public T Deserialize(Content content, IEnumerable<string> names)
        {
            if (content == null)
                throw new ArgumentNullException(nameof(content));
            if (names == null)
                throw new ArgumentNullException(nameof(names));

            var instance = (T)Activator.CreateInstance(type);
            var members = GetMembers(names);

            foreach (var member in members)
            {
                Deserialize(content, instance, member);
            }

            return instance;
        }

        /// <summary>
        /// 反序列化;
        /// </summary>
        /// <exception cref="FileNotFoundException"></exception>
        /// <exception cref="ObjectDisposedException"></exception>
        private void Deserialize(Content content, T instance, IMember member)
        {
            using (var stream = content.GetInputStream(member.Info.RelativePath))
            {
                var value = member.Serializer.Deserialize(stream);
                member.SetValue(instance, value);
            }
        }


        /// <summary>
        /// 获取到需要进行操作的成员;
        /// </summary>
        private IEnumerable<IMember> GetMembers()
        {
            return members.Value;
        }

        /// <summary>
        /// 获取到需要进行操作的成员;
        /// </summary>
        /// <exception cref="KeyNotFoundException"></exception>
        private IEnumerable<IMember> GetMembers(IEnumerable<string> names)
        {
            return names.Select(name => GetMember(members.Value, name));
        }

        /// <summary>
        /// 获取到对应成员,若不存在则返回异常;
        /// </summary>
        /// <exception cref="KeyNotFoundException"></exception>
        private IMember GetMember(string name)
        {
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
    }
}
