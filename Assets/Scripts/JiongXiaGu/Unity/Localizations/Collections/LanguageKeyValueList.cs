using System.Collections.Generic;
using System.Xml.Serialization;

namespace JiongXiaGu.Unity.Localizations
{
    [XmlRoot(XmlRootName)]
    public class LanguageKeyValueList : LanguageKeyValueCollection
    {
        private readonly List<KeyValuePair<string, string>> list;

        public LanguageKeyValueList()
        {
            list = new List<KeyValuePair<string, string>>();
        }

        public override void Add(string key, string value)
        {
            var pair = new KeyValuePair<string, string>(key, value);
            list.Add(pair);
        }

        public override IEnumerator<KeyValuePair<string, string>> GetEnumerator()
        {
            return list.GetEnumerator();
        }
    }
}
