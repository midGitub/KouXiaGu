using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace JiongXiaGu.Unity.Localizations
{
    /// <summary>
    /// 提供字典文本结构序列化方法;
    /// </summary>
    [XmlRoot(XmlRootName)]
    public abstract class LanguageKeyValueCollection : IEnumerable<KeyValuePair<string, string>>, IXmlSerializable
    {
        public abstract void Add(string key, string value);

        XmlSchema IXmlSerializable.GetSchema()
        {
            return null;
        }

        internal const string XmlRootName = "LanguageDictionary";
        internal const string TextItemElementName = "Text";
        internal const string TextItemKeyAttributeName = "key";

        void IXmlSerializable.ReadXml(XmlReader reader)
        {
            if (reader.ReadToDescendant(TextItemElementName))
            {
                do
                {
                    string key = reader.GetAttribute(TextItemKeyAttributeName);
                    string value = reader.ReadElementContentAsString();
                    Add(key, value);
                }
                while (reader.IsStartElement());
            }
        }

        void IXmlSerializable.WriteXml(XmlWriter writer)
        {
            foreach (var pair in this)
            {
                writer.WriteStartElement(TextItemElementName);
                writer.WriteAttributeString(TextItemKeyAttributeName, pair.Key);
                writer.WriteValue(pair.Value);
                writer.WriteEndElement();
            }
        }

        public abstract IEnumerator<KeyValuePair<string, string>> GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
