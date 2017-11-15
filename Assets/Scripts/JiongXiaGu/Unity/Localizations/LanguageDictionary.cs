using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace JiongXiaGu.Unity.Localizations
{

    /// <summary>
    /// 文本字典;
    /// </summary>
    [XmlRoot("LanguageDictionary")]
    public class LanguageDictionary : Dictionary<string, string>, IXmlSerializable
    {
        public LanguageDictionary()
        {
        }

        public LanguageDictionary(IDictionary<string, string> dictionary) : base(dictionary)
        {
        }

        public void AddOrUpdate(string key, string value)
        {
            if (ContainsKey(key))
            {
                base[key] = value;
            }
            else
            {
                Add(key, value);
            }
        }

        /// <summary>
        /// 添加字典内容;
        /// </summary>
        public void AddOrUpdate(LanguageDictionary languageDictionary)
        {
            if (languageDictionary == this)
                return;

            foreach (var pair in this)
            {
                AddOrUpdate(pair.Key, pair.Value);
            }
        }

        XmlSchema IXmlSerializable.GetSchema()
        {
            return null;
        }

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
                    AddOrUpdate(key, value);
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
    }
}
