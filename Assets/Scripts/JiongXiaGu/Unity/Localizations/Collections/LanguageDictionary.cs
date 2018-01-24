using System.Linq;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace JiongXiaGu.Unity.Localizations
{

    /// <summary>
    /// 文本字典;
    /// </summary>
    [XmlRoot(LanguageKeyValueCollection.XmlRootName)]
    public class LanguageDictionary : LanguageKeyValueCollection, ILanguageDictionary
    {
        private Dictionary<string, string> dictionary;
        public int Count => dictionary.Count;

        public LanguageDictionary()
        {
            dictionary = new Dictionary<string, string>();
        }

        public override void Add(string key, string value)
        {
            if (dictionary.ContainsKey(key))
            {
                dictionary[key] = value;
            }
            else
            {
                dictionary.Add(key, value);
            }
        }

        public void Add(IEnumerable<KeyValuePair<string, string>> value)
        {
            foreach (var item in value)
            {
                Add(item.Key, item.Value);
            }
        }

        public bool TryGetValue(string key, out string value)
        {
            return dictionary.TryGetValue(key, out value);
        }

        public override IEnumerator<KeyValuePair<string, string>> GetEnumerator()
        {
            return dictionary.GetEnumerator();
        }
    }
}
