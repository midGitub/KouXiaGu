using System.Collections.Generic;

namespace JiongXiaGu.Unity.Localizations
{

    public interface IReadOnlyLanguageDictionary : IEnumerable<KeyValuePair<string, string>>
    {
        int Count { get; }
        bool TryGetValue(string key, out string value);
    }

    public interface ILanguageDictionary : IReadOnlyLanguageDictionary, IEnumerable<KeyValuePair<string, string>>
    {
        void Add(string key, string value);
    }
}
