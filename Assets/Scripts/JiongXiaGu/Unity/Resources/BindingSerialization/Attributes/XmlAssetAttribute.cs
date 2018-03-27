using System;

namespace JiongXiaGu.Unity.Resources.BindingSerialization
{

    /// <summary>
    /// 使用Xml方式读取数据;需要挂载到公共的变量或者属性上;
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false)]
    public class XmlAssetAttribute : AssetAttribute
    {

        public const string defaultFileExtension = ".xml";
        public override string DefaultFileExtension => defaultFileExtension;

        public XmlAssetAttribute(string relativePath) : base(relativePath)
        {
        }

        public XmlAssetAttribute(string relativePath, bool useDefaultExtension) : base(relativePath, useDefaultExtension)
        {
        }

        public XmlAssetAttribute(string relativePath, string name, string message) : base(relativePath, name, message)
        {
        }

        public XmlAssetAttribute(string relativePath, string name, string message, bool useDefaultExtension) : base(relativePath, name, message, useDefaultExtension)
        {
        }

        public override ISerializer CreateSerializer(Type type)
        {
            return new XmlSerializer(type);
        }
    }
}
