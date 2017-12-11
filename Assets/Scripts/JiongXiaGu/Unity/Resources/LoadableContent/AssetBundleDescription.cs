using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace JiongXiaGu.Unity.Resources
{

    /// <summary>
    /// AssetBundle 描述;
    /// </summary>
    public struct AssetBundleDescription : IXmlSerializable
    {
        internal const string NameAttribute = "name";

        /// <summary>
        /// 指定的唯一名;
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 相对路径;
        /// </summary>
        public string RelativePath { get; set; }

        public AssetBundleDescription(string name, string path)
        {
            Name = name;
            RelativePath = path;
        }

        XmlSchema IXmlSerializable.GetSchema()
        {
            return null;
        }

        void IXmlSerializable.ReadXml(XmlReader reader)
        {
            Name = reader.GetAttribute(NameAttribute);
            RelativePath = reader.ReadElementContentAsString();
            reader.ReadEndElement();
        }

        void IXmlSerializable.WriteXml(XmlWriter writer)
        {
            writer.WriteAttributeString(NameAttribute, Name);
            writer.WriteValue(RelativePath);
        }
    }
}
