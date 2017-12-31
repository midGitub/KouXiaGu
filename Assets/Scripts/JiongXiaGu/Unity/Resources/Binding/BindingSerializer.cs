using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace JiongXiaGu.Unity.Resources.Binding
{

    /// <summary>
    /// 使用特性,将类型从资源合集中读取读取;
    /// </summary>
    public sealed class BindingSerializer
    {
        private readonly Type type;
        private List<IMember> members;

        public BindingSerializer(Type type)
        {
            if (type == null)
                throw new ArgumentNullException(nameof(type));

            this.type = type;
        }

        /// <summary>
        /// 序列化;
        /// </summary>
        public void Serialize(Content content, object instance)
        {
            if (content == null)
                throw new ArgumentNullException(nameof(content));
            if (instance == null)
                throw new ArgumentNullException(nameof(instance));
            if (!type.Equals(instance.GetType()))
                throw new ArgumentException(nameof(instance));

            var members = GetMembersInternal();
            foreach (var member in members)
            {
                using (var stream = content.CreateOutputStream(member.RelativePath))
                {
                    var value = member.GetValue(instance);
                    member.Serializer.Serialize(stream, value);
                }
            }
        }

        /// <summary>
        /// 反序列化;
        /// </summary>
        public object Deserialize(Content content)
        {
            if (content == null)
                throw new ArgumentNullException(nameof(content));

            var instance = Activator.CreateInstance(type);
            var members = GetMembersInternal();
            foreach (var member in members)
            {
                try
                {
                    using (var stream = content.GetInputStream(member.RelativePath))
                    {
                        var value = member.Serializer.Deserialize(stream);
                        member.SetValue(instance, value);
                    }
                }
                catch(FileNotFoundException)
                {
                    continue;
                }
            }
            return instance;
        }

        public const BindingFlags FieldBindingFlags = BindingFlags.Instance | BindingFlags.Public;
        public const BindingFlags PropertyBindingFlags = BindingFlags.Instance | BindingFlags.Public;

        /// <summary>
        /// 获取到需要进行操作的成员;
        /// </summary>
        private List<IMember> GetMembersInternal()
        {
            if (members == null)
            {
                members = new List<IMember>();

                var fields = type.GetFields(FieldBindingFlags);
                foreach (var field in fields)
                {
                    var attribute = field.GetCustomAttribute<AssetAttribute>();
                    if (attribute != null)
                    {
                        var serializer = attribute.GetSerializer(field.FieldType);
                        var member = new Field(attribute.RelativePath, serializer, field);
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
                            var serializer = attribute.GetSerializer(property.PropertyType);
                            var member = new Property(attribute.RelativePath, serializer, property);
                            members.Add(member);
                        }
                    }
                }
            }
            return members;
        }

        private interface IMember
        {
            /// <summary>
            /// 相对路径;
            /// </summary>
            string RelativePath { get; }

            /// <summary>
            /// 序列化接口;
            /// </summary>
            ISerializer Serializer { get; }

            /// <summary>
            /// 获取到值;
            /// </summary>
            object GetValue(object instance);

            /// <summary>
            /// 设置到值;
            /// </summary>
            void SetValue(object instance, object value);
        }

        private struct Field : IMember
        {
            public string RelativePath { get; private set; }
            public ISerializer Serializer { get; private set; }
            public FieldInfo FieldInfo { get; private set; }

            public Field(string relativePath, ISerializer serializer, FieldInfo fieldInfo)
            {
                RelativePath = relativePath;
                Serializer = serializer;
                FieldInfo = fieldInfo;
            }

            public object GetValue(object instance)
            {
                return FieldInfo.GetValue(instance);
            }

            public void SetValue(object instance, object value)
            {
                FieldInfo.SetValue(instance, value);
            }
        }

        private struct Property : IMember
        {
            public string RelativePath { get; private set; }
            public ISerializer Serializer { get; private set; }
            public PropertyInfo PropertyInfo { get; private set; }

            public Property(string relativePath, ISerializer serializer, PropertyInfo propertyInfo)
            {
                RelativePath = relativePath;
                Serializer = serializer;
                PropertyInfo = propertyInfo;
            }

            public object GetValue(object instance)
            {
                return PropertyInfo.GetValue(instance);
            }

            public void SetValue(object instance, object value)
            {
                PropertyInfo.SetValue(instance, value);
            }
        }
    }
}
