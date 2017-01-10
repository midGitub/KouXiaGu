using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KouXiaGu.Collections;

namespace KouXiaGu.xgLocalization
{


    public class TextDictionary : Dictionary<string, string>, IReadOnlyDictionary<string, string>, IReadOnlyDictionary
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

        ///// <summary>
        ///// 获取到文本,若获取失败则返回本身;
        ///// </summary>
        //public string GetText(string key)
        //{
        //    string text;
        //    if (TryGetValue(key, out text))
        //        return text;
        //    else
        //        return key;
        //}

        ///// <summary>
        ///// 获取到文本,若获取失败则返回本身,并且调用 onFail;
        ///// </summary>
        //public string GetText(string key, Action onFail)
        //{
        //    throw new NotImplementedException();
        //}

    }

}
