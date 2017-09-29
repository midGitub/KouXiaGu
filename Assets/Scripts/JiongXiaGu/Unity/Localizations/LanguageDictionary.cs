using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using JiongXiaGu.Collections;

namespace JiongXiaGu.Unity.Localizations
{

    /// <summary>
    /// 文本字典;
    /// </summary>
    [XmlRoot("LanguageDictionary")]
    public class LanguageDictionary : Dictionary<string, string>, IDictionary<string, string>, IXmlSerializable
    {
        public LanguageDictionary()
        {
        }

        public LanguageDictionary(IDictionary<string, string> dictionary) : base(dictionary)
        {
        }

        /// <summary>
        /// 添加字典内容;
        /// </summary>
        public void AddOrUpdate(LanguageDictionary languageDictionary)
        {
            if (languageDictionary == this)
                return;

            foreach (var pair in languageDictionary)
            {
                this.AddOrUpdate(pair);
            }
        }

        XmlSchema IXmlSerializable.GetSchema()
        {
            return null;
        }

        internal const string TextItemElementName = "Text";
        internal const string TextItemKeyAttributeName = "key";
        internal const string TextItemValueAttributeName = "value";

        void IXmlSerializable.ReadXml(XmlReader reader)
        {
            if (reader.ReadToDescendant(TextItemElementName))
            {
                while (reader.ReadToNextSibling(TextItemElementName))
                {
                    string key = reader.GetAttribute(TextItemKeyAttributeName);
                    string value = reader.GetAttribute(TextItemValueAttributeName);
                    this.AddOrUpdate(key, value);
                }
                reader.ReadEndElement();
            }
            reader.ReadEndElement();
        }

        void IXmlSerializable.WriteXml(XmlWriter writer)
        {
            foreach (var pair in this)
            {
                writer.WriteStartElement(TextItemElementName);
                writer.WriteAttributeString(TextItemKeyAttributeName, pair.Key);
                writer.WriteAttributeString(TextItemValueAttributeName, pair.Value);
                writer.WriteEndElement();
            }
        }
    }
}
