using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KouXiaGu.Collections;

namespace KouXiaGu.Globalization
{


    public class TextDictionary : Dictionary<string, string>, IReadOnlyDictionary<string, string>, ITextDictionary,
        IEnumerable<TextItem>
    {
        public TextDictionary() : base() { }
        public TextDictionary(IEqualityComparer<string> comparer) : base(comparer) { }
        public TextDictionary(IDictionary<string, string> dictionary) : base(dictionary) { }
        public TextDictionary(int capacity) : base(capacity) { }
        public TextDictionary(IDictionary<string, string> dictionary, IEqualityComparer<string> comparer) : base(dictionary, comparer) { }
        public TextDictionary(int capacity, IEqualityComparer<string> comparer) : base(capacity, comparer) { }

        public TextDictionary(IEnumerable<TextItem> textItems)
        {
            foreach (var item in textItems)
            {
                Add(item);
            }
        }


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

        IEnumerator<TextItem> IEnumerable<TextItem>.GetEnumerator()
        {
            foreach (var item in this as IEnumerable<KeyValuePair<string, string>>)
            {
                yield return new TextItem(item.Key, item.Value, false);
            }
        }

    }

}
