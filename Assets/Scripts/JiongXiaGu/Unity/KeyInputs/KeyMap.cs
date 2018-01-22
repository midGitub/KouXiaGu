using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using System.Text;
using System.Threading.Tasks;
using JiongXiaGu.Collections;

namespace JiongXiaGu.Unity.KeyInputs
{

    /// <summary>
    /// 按键映射;
    /// </summary>
    [XmlRoot("KeyMap")]
    public class KeyMap : IEnumerable<KeyInfo>
    {
        public IDictionary<string, CombinationKey> Dictionary { get; private set; }

        public CombinationKey this[string keyName]
        {
            get { return Dictionary[keyName]; }
            set { Dictionary[keyName] = value; }
        }

        public int Count
        {
            get { return Dictionary.Count; }
        }

        public KeyMap()
        {
            Dictionary = new Dictionary<string, CombinationKey>();
        }

        public KeyMap(IDictionary<string, CombinationKey> dictionary)
        {
            Dictionary = new Dictionary<string, CombinationKey>(dictionary);
        }

        public void Add(string keyName, CombinationKey key)
        {
            Dictionary.Add(keyName, key);
        }

        public void Add(KeyInfo info)
        {
            Dictionary.Add(info.Name, info.Key);
        }

        public AddOrUpdateStatus AddOrUpdate(KeyInfo info)
        {
            if (Dictionary.ContainsKey(info.Name))
            {
                Dictionary[info.Name] = info.Key;
                return AddOrUpdateStatus.Updated;
            }
            else
            {
                Add(info);
                return AddOrUpdateStatus.Added;
            }
        }

        public void AddOrUpdate(IEnumerable<KeyInfo> infos)
        {
            foreach (var info in infos)
            {
                AddOrUpdate(info);
            }
        }

        IEnumerator<KeyInfo> IEnumerable<KeyInfo>.GetEnumerator()
        {
            return Dictionary.Select(item => new KeyInfo(item.Key, item.Value)).GetEnumerator();
        }

        public IEnumerator GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
