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
    public class LanguageDictionary : IEnumerable<KeyValuePair<string,string>>, IXmlSerializable
    {
        public Dictionary<string, string> Dictionary { get; private set; }

        public LanguageDictionary()
        {
            Dictionary = new Dictionary<string, string>();
        }

        public LanguageDictionary(IDictionary<string, string> dictionary)
        {
            Dictionary = new Dictionary<string, string>(dictionary);
        }

        public void Add(KeyValuePair<string, string> pair)
        {
            Dictionary.Add(pair.Key, pair.Value);
        }

        public void Add(string key, string value)
        {
            Dictionary.Add(key, value);
        }

        public void AddOrUpdate(string key, string value)
        {
            if (Dictionary.ContainsKey(key))
            {
                Dictionary[key] = value;
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

            foreach (var pair in Dictionary)
            {
                AddOrUpdate(pair.Key, pair.Value);
            }
        }

        public IEnumerator<KeyValuePair<string, string>> GetEnumerator()
        {
            return ((IEnumerable<KeyValuePair<string, string>>)Dictionary).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable<KeyValuePair<string, string>>)Dictionary).GetEnumerator();
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
            foreach (var pair in Dictionary)
            {
                writer.WriteStartElement(TextItemElementName);
                writer.WriteAttributeString(TextItemKeyAttributeName, pair.Key);
                writer.WriteValue(pair.Value);
                writer.WriteEndElement();
            }
        }
    }
}
