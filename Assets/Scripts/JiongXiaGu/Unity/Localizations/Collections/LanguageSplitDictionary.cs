using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace JiongXiaGu.Unity.Localizations
{
    /// <summary>
    /// 记录Tag的文本字典结构;
    /// </summary>
    public class LanguageSplitDictionary : ILanguageDictionary
    {
        private Dictionary<string, LanguageValue> dictionary;
        public int Count => dictionary.Count;
        public IDictionary<string, LanguageValue> Dictionary => dictionary;

        public LanguageSplitDictionary()
        {
            dictionary = new Dictionary<string, LanguageValue>();
        }

        public void Add(string key, string value)
        {
            LanguageValue languageValue = new LanguageValue(value);
            if (dictionary.ContainsKey(key))
            {
                dictionary[key] = languageValue;
            }
            else
            {
                dictionary.Add(key, languageValue);
            }
        }

        public void Add(string tag, string key, string value)
        {
            LanguageValue languageValue = new LanguageValue(tag, value);
            if (dictionary.ContainsKey(key))
            {
                dictionary[key] = languageValue;
            }
            else
            {
                dictionary.Add(key, languageValue);
            }
        }

        public void Add(string tag, IEnumerable<KeyValuePair<string, string>> value)
        {
            foreach (var item in value)
            {
                Add(tag, item.Key, item.Value);
            }
        }

        public bool TryGetValue(string key, out string value)
        {
            LanguageValue languageValue;
            if (dictionary.TryGetValue(key, out languageValue))
            {
                value = languageValue.Value;
                return true;
            }
            else
            {
                value = default(string);
                return false;
            }
        }

        public IEnumerator<KeyValuePair<string, string>> GetEnumerator()
        {
            return dictionary.Select(item => new KeyValuePair<string, string>(item.Key, item.Value.Value)).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public struct LanguageValue
        {
            public string Tag { get; set; }
            public string Value { get; set; }

            public LanguageValue(string value) : this(null, value)
            {
            }

            public LanguageValue(string tag, string value) : this()
            {
                Tag = tag;
                Value = value;
            }

            public override string ToString()
            {
                string str = string.Format("[Tag:{0}, Value:{1}]", Tag, Value);
                return str;
            }
        }
    }
}
