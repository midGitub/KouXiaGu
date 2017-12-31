using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace JiongXiaGu.Unity.Resources.BindingRead
{

    public abstract class AssetAttribute : Attribute
    {
        /// <summary>
        /// 相对路径;
        /// </summary>
        public string RelativePath { get; private set; }

        public AssetAttribute(string relativePath)
        {
            RelativePath = relativePath;
        }

        /// <summary>
        /// 获取到对应序列化器;
        /// </summary>
        public abstract ISerializer GetSerializer(Type type);
    }

    /// <summary>
    /// 使用Xml方式读取数据;
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false)]
    public class XmlAssetAttribute : AssetAttribute
    {
        public XmlAssetAttribute(string relativePath) : base(relativePath)
        {
        }

        public override ISerializer GetSerializer(Type type)
        {
            return new XmlSerializer(type);
        }
    }

    /// <summary>
    /// 使用Json方式读取数据;
    /// </summary>
    [Obsolete("还未实现")]
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false)]
    public class JsonAssetAttribute : AssetAttribute
    {
        public JsonAssetAttribute(string relativePath) : base(relativePath)
        {
        }

        public override ISerializer GetSerializer(Type type)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// 使用特性,将类型从资源合集中读取读取;
    /// </summary>
    public class BindingReader
    {
        private Type type;
        private List<IMember> children;

        public BindingReader(Type type)
        {
            if (type == null)
                throw new ArgumentNullException(nameof(type));

            this.type = type;
        }

        /// <summary>
        /// 序列化;
        /// </summary>
        public void Write(Content content, object item)
        {
            if (content == null)
                throw new ArgumentNullException(nameof(content));
            if (item == null)
                throw new ArgumentNullException(nameof(item));
            if (!type.Equals(item))
                throw new ArgumentException(nameof(item));

            throw new NotImplementedException();
        }

        /// <summary>
        /// 反序列化;
        /// </summary>
        public object Read(Content content)
        {
            if (content == null)
                throw new ArgumentNullException(nameof(content));

            var obj = Activator.CreateInstance(type);


            throw new NotImplementedException();
        }

        private List<IMember> GetChildren()
        {
            throw new NotImplementedException();
        }

        public interface IMember
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
