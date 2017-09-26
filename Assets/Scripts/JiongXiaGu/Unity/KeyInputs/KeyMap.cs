using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using JiongXiaGu.Collections;

namespace JiongXiaGu.Unity.KeyInputs
{

    /// <summary>
    /// 按键映射;
    /// </summary>
    public class KeyMap : Dictionary<string, CombinationKey>, IDictionary<string, CombinationKey>
    {
        public KeyMap()
        {
        }

        public KeyMap(IDictionary<string, CombinationKey> dictionary) : base(dictionary)
        {
        }

        public void Add(KeyInfo info)
        {
            Add(info.Name, info.Key);
        }

        public AddOrUpdateStatus AddOrUpdate(KeyInfo info)
        {
            if (ContainsKey(info.Name))
            {
                this[info.Name] = info.Key;
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

        public KeyInfo[] ToArray()
        {
            return this.ToArray(pair => new KeyInfo(pair.Key, pair.Value));
        }
    }
}
