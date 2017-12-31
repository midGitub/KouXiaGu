using System;

namespace JiongXiaGu.Unity.Resources.Binding
{

    /// <summary>
    /// 使用Xml方式读取数据;需要挂载到公共的变量或者属性上;
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false)]
    public class XmlAssetAttribute : AssetAttribute
    {
        public override string FileExtension => ".xml";

        public XmlAssetAttribute(string relativePath) : base(relativePath)
        {
        }

        public XmlAssetAttribute(string relativePath, bool modifyExtension) : base(relativePath, modifyExtension)
        {
        }

        public XmlAssetAttribute(string relativePath, string message, bool modifyExtension) : base(relativePath, message, modifyExtension)
        {
        }

        public XmlAssetAttribute(string relativePath, string message) : base(relativePath, message)
        {
        }

        public override ISerializer GetSerializer(Type type)
        {
            return new XmlSerializer(type);
        }
    }
}
