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
    public class LanguagePackHead
    {
        public LanguagePackHead()
        {
        }

        public LanguagePackHead(string name, string locName)
        {
            Name = name;
            LocName = locName;
        }

        [XmlAttribute(LanguagePackXmlSerializer.LanguageNameAttributeName)]
        public string Name { get; internal set; }

        [XmlAttribute(LanguagePackXmlSerializer.LanguageLocNameAttributeName)]
        public string LocName { get; internal set; }
    }

    /// <summary>
    /// 语言包信息;
    /// </summary>
    [XmlType(LanguagePackXmlSerializer.RootName)]
    public class LanguagePack : LanguagePackHead
    {
        public LanguagePack() : this(string.Empty, string.Empty)
        {
        }

        public LanguagePack(LanguagePackHead head) : this(head.Name, head.LocName)
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
            if (keyValuePairs != null)
            {
                Dictionary<string, string> dictionary = new Dictionary<string, string>();
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
            return null;
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
    }
}
