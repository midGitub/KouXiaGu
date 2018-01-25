using System.Collections.Generic;

namespace JiongXiaGu.Unity.Localizations
{
    public interface ILanguageDictionary : IEnumerable<KeyValuePair<string, string>>
    {
        int Count { get; }
        void Add(string key, string value);
        bool TryGetValue(string key, out string value);
    }
}
