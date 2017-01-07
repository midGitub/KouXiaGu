using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KouXiaGu.Collections;

namespace KouXiaGu.Localizations
{


    public class TextDictionary : Dictionary<string, string>, IReadOnlyDictionary<string, string>
    {
        public TextDictionary() : base() { }
        public TextDictionary(IEqualityComparer<string> comparer) : base(comparer) { }
        public TextDictionary(IDictionary<string, string> dictionary) : base(dictionary) { }
        public TextDictionary(int capacity) : base(capacity) { }
        public TextDictionary(IDictionary<string, string> dictionary, IEqualityComparer<string> comparer) : base(dictionary, comparer) { }
        public TextDictionary(int capacity, IEqualityComparer<string> comparer) : base(capacity, comparer) { }

        IEnumerable<string> IReadOnlyDictionary<string, string>.Keys
        {
            get { return base.Keys; }
        }

        IEnumerable<string> IReadOnlyDictionary<string, string>.Values
        {
            get { return base.Values; }
        }

        public bool Add(TextItem pack)
        {
            if (ContainsKey(pack.Key) && !pack.IsUpdate)
            {
                return false;
            }

            this.AddOrUpdate(pack.Key, pack.Value);
            return true;
        }

    }

}
