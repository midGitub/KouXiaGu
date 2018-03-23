using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace JiongXiaGu.Unity.Resources.Binding
{

    /// <summary>
    /// 通过特性定义序列化方式,统一进行序列化操作;
    /// </summary>
    public sealed class BindingSerializer<T>
    {
        private readonly Lazy<List<SerializableMember>> lazySerializableMember;
        internal bool IsMemberComplete => lazySerializableMember.IsValueCreated;
        private IEnumerable<SerializableMember> serializableMembers => lazySerializableMember.Value;

        public BindingSerializer()
        {
            var type = typeof(T);
            lazySerializableMember = new Lazy<List<SerializableMember>>(delegate ()
            {
                var members = ReflectionImporter.BuildMembers(type).Select(member => new SerializableMember(member));
                return new List<SerializableMember>(members);
            });
        }

        public BindingSerializer(IEnumerable<IMember> members)
        {            
            var type = typeof(T);
            this.lazySerializableMember = new Lazy<List<SerializableMember>>(delegate ()
            {
                var serializableMember = members.Select(member => new SerializableMember(member));
                return new List<SerializableMember>(serializableMember);
            });
        }

        /// <summary>
        /// 序列化;
        /// </summary>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ObjectDisposedException"></exception>
        /// <exception cref="InvalidOperationException"></exception>
        public void Serialize(Content writableContent, ref T instance)
        {
            Serialize(writableContent, ref instance, serializableMembers);
        }

        /// <summary>
        /// 仅序列化指定成员;
        /// </summary>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ObjectDisposedException"></exception>
        /// <exception cref="InvalidOperationException"></exception>
        public void Serialize(Content writableContent, ref T instance, Func<IMember, bool> filter)
        {
            Serialize(writableContent, ref instance, serializableMembers.Where(member => filter.Invoke(member.MemberInfo)));
        }
        
        /// <summary>
        /// 序列化;
        /// </summary>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ObjectDisposedException"></exception>
        /// <exception cref="InvalidOperationException"></exception>
        private void Serialize(Content writableContent, ref T instance, IEnumerable<SerializableMember> members)
        {
            if (writableContent == null)
                throw new ArgumentNullException(nameof(writableContent));
            if (instance == null)
                throw new ArgumentNullException(nameof(instance));
            if (members == null)
                throw new ArgumentNullException(nameof(members));

            foreach (var member in members)
            {
                Serialize(writableContent, ref instance, member);
            }
        }

        /// <summary>
        /// 序列化指定成员;
        /// </summary>
        /// <exception cref="ObjectDisposedException"></exception>
        /// <exception cref="InvalidOperationException"></exception>
        private void Serialize(Content writableContent, ref T instance, SerializableMember member)
        {
            using (var stream = writableContent.GetOutputStream(member.MemberInfo.RelativePath))
            {
                var value = member.MemberInfo.GetValue(instance);
                member.Serializer.Serialize(stream, value);
            }
        }

        /// <summary>
        /// 反序列化;(注意,该方法使用无参数构造函数创建 T,所以T必须要具有无参数构造函数)
        /// </summary>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="FileNotFoundException"></exception>
        /// <exception cref="ObjectDisposedException"></exception>
        /// <exception cref="MissingMethodException">为指定的类型 T 不具有无参数构造函数</exception>
        public T Deserialize(Content content)
        {
            var instance = Activator.CreateInstance<T>();
            return Deserialize(content, instance);
        }

        /// <summary>
        /// 反序列化所有成员;
        /// </summary>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="FileNotFoundException"></exception>
        /// <exception cref="ObjectDisposedException"></exception>
        public T Deserialize(Content content, T instance)
        {
            return Deserialize(content, instance, serializableMembers);
        }

        /// <summary>
        /// 反序列化指定成员;
        /// </summary>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="FileNotFoundException"></exception>
        /// <exception cref="ObjectDisposedException"></exception>
        public T Deserialize(Content content, T instance, Func<IMember, bool> filter)
        {
            return Deserialize(content, instance, serializableMembers.Where(member => filter.Invoke(member.MemberInfo)));
        }


        /// <summary>
        /// 反序列化指定成员;
        /// </summary>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="FileNotFoundException"></exception>
        /// <exception cref="ObjectDisposedException"></exception>
        private T Deserialize(Content content, T instance, IEnumerable<SerializableMember> members)
        {
            if (content == null)
                throw new ArgumentNullException(nameof(content));
            if (instance == null)
                throw new ArgumentNullException(nameof(members));
            if (members == null)
                throw new ArgumentNullException(nameof(members));

            foreach (var member in members)
            {
                Deserialize(content, instance, member);
            }

            return instance;
        }

        /// <summary>
        /// 反序列化成员;
        /// </summary>
        /// <exception cref="FileNotFoundException"></exception>
        /// <exception cref="ObjectDisposedException"></exception>
        private void Deserialize(Content content, T instance, SerializableMember member)
        {
            try
            {
                using (var stream = content.GetInputStream(member.MemberInfo.RelativePath))
                {
                    var value = member.Serializer.Deserialize(stream);
                    member.MemberInfo.SetValue(instance, value);
                }
            }
            catch (FileNotFoundException ex)
            {
                if (member.MemberInfo.IsNecessaryAsset)
                {
                    throw ex;
                }
            }
        }

        private struct SerializableMember
        {
            private ISerializer serializer;
            public IMember MemberInfo { get; private set; }
            public ISerializer Serializer => serializer ?? (serializer = MemberInfo.CreateSerializer());

            public SerializableMember(IMember member)
            {
                if (member == null)
                    throw new ArgumentNullException(nameof(member));

                MemberInfo = member;
                serializer = null;
            }
        }
    }
}
