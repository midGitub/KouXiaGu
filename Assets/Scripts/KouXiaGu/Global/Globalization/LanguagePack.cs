using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using UnityEngine;

namespace KouXiaGu.Globalization
{

    /// <summary>
    /// 语言包信息;
    /// </summary>
    [XmlType(LanguagePackXmlSerializer.RootName)]
    public class LanguagePack : LanguagePackHead
    {
        public LanguagePack() : this(string.Empty, string.Empty)
        {
        }

        public LanguagePack(LanguagePackHead head) : this(head.Name, head.Language)
        {
        }

        public LanguagePack(string name, string locName) : base(name, locName)
        {
            TextDictionary = new Dictionary<string, string>();
        }

        [XmlArray(LanguagePackXmlSerializer.TextsRootName)]
        [XmlArrayItem(LanguagePackXmlSerializer.TextElementName)]
        public KeyValuePair[] Texts
        {
            get { return ToArray(TextDictionary); }
            private set { TextDictionary = ToDictionary(value); }
        }

        [XmlIgnore]
        public Dictionary<string, string> TextDictionary { get; internal set; }

        Dictionary<string, string> ToDictionary(IEnumerable<KeyValuePair> keyValuePairs)
        {
            Dictionary<string, string> dictionary = new Dictionary<string, string>();
            if (keyValuePairs != null)
            {
                foreach (var keyValuePair in keyValuePairs)
                {
                    if (dictionary.ContainsKey(keyValuePair.Key))
                    {
                        Debug.LogWarning("语言文件[" + Name + "]存在相同的Key:" + keyValuePair.Key);
                    }
                    else
                    {
                        dictionary.Add(keyValuePair.Key, keyValuePair.Value);
                    }
                }
                return dictionary;
            }
            return dictionary;
        }

        KeyValuePair[] ToArray(Dictionary<string, string> textDictionary)
        {
            if (textDictionary != null)
            {
                KeyValuePair[] array = new KeyValuePair[textDictionary.Count];
                int index = 0;
                foreach (var keyValuePair in textDictionary)
                {
                    array[index++] = keyValuePair;
                }
                return array;
            }
            return null;
        }

        public override string ToString()
        {
            return "[Name:" + Name + ", Language:" + Language + ", TextItemCount:" + TextDictionary.Count + "]";
        }
    }
}
