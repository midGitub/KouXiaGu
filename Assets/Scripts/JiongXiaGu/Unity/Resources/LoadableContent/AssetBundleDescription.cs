using System;
using System.IO;
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

        private string name;

        /// <summary>
        /// 指定的唯一名;
        /// </summary>
        public string Name
        {
            get { return name; }
            private set { name = value.ToLower(); }
        }

        /// <summary>
        /// 相对路径;
        /// </summary>
        public string RelativePath { get; private set; }

        public AssetBundleDescription(string name) : this()
        {
            Name = name;
            RelativePath = GetDefalutRelativePath(name);
        }

        public AssetBundleDescription(string name, string path) : this()
        {
            Name = name;
            RelativePath = path;
        }

        private static string GetDefalutRelativePath(string name)
        {
            return Path.Combine("AssetBundle", name.ToLower());
        }

        XmlSchema IXmlSerializable.GetSchema()
        {
            return null;
        }

        void IXmlSerializable.ReadXml(XmlReader reader)
        {
            name = reader.GetAttribute(NameAttribute);
            if (string.IsNullOrWhiteSpace(Name))
            {
                throw new XmlException(reader.ToString());
            }
            else
            {
                Name = name;
            }

            RelativePath = reader.ReadElementContentAsString();
            if (string.IsNullOrWhiteSpace(RelativePath))
            {
                RelativePath = GetDefalutRelativePath(Name);
            }

            reader.ReadEndElement();
        }

        void IXmlSerializable.WriteXml(XmlWriter writer)
        {
            if (string.IsNullOrWhiteSpace(Name))
            {
                throw new ArgumentNullException(nameof(Name));
            }

            writer.WriteAttributeString(NameAttribute, Name);
            writer.WriteValue(RelativePath);
        }
    }
}
