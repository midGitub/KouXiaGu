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

        public string Name { get; set; }
        public string Path { get; set; }

        public AssetBundleDescription(string name, string path)
        {
            Name = name;
            Path = path;
        }

        XmlSchema IXmlSerializable.GetSchema()
        {
            return null;
        }

        void IXmlSerializable.ReadXml(XmlReader reader)
        {
            Name = reader.GetAttribute(NameAttribute);
            Path = reader.ReadElementContentAsString();
            reader.ReadEndElement();
        }

        void IXmlSerializable.WriteXml(XmlWriter writer)
        {
            writer.WriteAttributeString(NameAttribute, Name);
            writer.WriteValue(Path);
        }
    }
}
