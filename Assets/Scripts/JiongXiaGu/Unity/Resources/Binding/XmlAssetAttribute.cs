using System;

namespace JiongXiaGu.Unity.Resources.Binding
{

    /// <summary>
    /// 使用Xml方式读取数据;需要挂载到公共的变量或者属性上;
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false)]
    public class XmlAssetAttribute : AssetAttribute
    {
        public XmlAssetAttribute(string relativePath) : base(relativePath)
        {
        }

        public XmlAssetAttribute(string relativePath, bool useDefaultExtension) : base(relativePath, useDefaultExtension)
        {
        }

        public XmlAssetAttribute(string relativePath, string message) : base(relativePath, message)
        {
        }

        public XmlAssetAttribute(string relativePath, string message, bool useDefaultExtension) : base(relativePath, message, useDefaultExtension)
        {
        }

        public const string DefaultFileExtension = ".xml";
        protected override string defaultFileExtension => DefaultFileExtension;

        public override ISerializer CreateSerializer(Type type)
        {
            return new XmlSerializer(type);
        }
    }
}
