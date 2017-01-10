using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KouXiaGu.XmlLocalization
{


    public struct TextItem
    {
        public TextItem(string Key, string Value, bool IsUpdate)
        {
            this.Key = Key;
            this.Value = Value;
            this.IsUpdate = IsUpdate;
        }

        public string Key { get; private set; }
        public string Value { get; private set; }
        public bool IsUpdate { get; private set; }

        public override bool Equals(object obj)
        {
            if (!(obj is TextItem))
                return false;
            return ((TextItem)obj).Key == Key;
        }

        public override int GetHashCode()
        {
            return Key.GetHashCode();
        }

        public override string ToString()
        {
            return "[Key:" + Key + ",Value:" + Value + ",IsUpdate:" + IsUpdate + "]";
        }

    }

}
