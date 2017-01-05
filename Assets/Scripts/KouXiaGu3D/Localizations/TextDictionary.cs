using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KouXiaGu.Localizations
{

    public class TextDictionary : Dictionary<string, string>
    {
        public TextDictionary() : base() { }
        public TextDictionary(IEqualityComparer<string> comparer) : base(comparer) { }
        public TextDictionary(IDictionary<string, string> dictionary) : base(dictionary) { }
        public TextDictionary(int capacity) : base(capacity) { }
        public TextDictionary(IDictionary<string, string> dictionary, IEqualityComparer<string> comparer) : base(dictionary, comparer) { }
        public TextDictionary(int capacity, IEqualityComparer<string> comparer) : base(capacity, comparer) { }

        /// <summary>
        /// 加入
        /// </summary>
        public bool Add(TextPack pack)
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
