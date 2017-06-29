using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace KouXiaGu.Globalization
{

    [XmlType(LanguagePackXmlSerializer.TextElementName)]
    public struct KeyValuePair
    {
        public KeyValuePair(string key, string value)
        {
            Key = key;
            Value = value;
        }

        [XmlAttribute(LanguagePackXmlSerializer.TextKeyAttributeName)]
        public string Key { get; set; }

        [XmlAttribute(LanguagePackXmlSerializer.TextValueAttributeName)]
        public string Value { get; set; }

        public static implicit operator KeyValuePair<string, string>(KeyValuePair pair)
        {
            return new KeyValuePair<string, string>(pair.Key, pair.Value);
        }

        public static implicit operator KeyValuePair(KeyValuePair<string, string> pair)
        {
            return new KeyValuePair(pair.Key, pair.Value);
        }
    }
}
