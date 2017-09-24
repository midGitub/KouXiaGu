using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace JiongXiaGu.Inputs
{

    /// <summary>
    /// KeyCode 类型的组合键;
    /// </summary>
    public class CombinationKey : IEquatable<CombinationKey>
    {
        const string Key_AttributeName = "key";
        public List<KeyCode> Keys { get; private set; }

        public CombinationKey()
        {
            Keys = new List<KeyCode>();
        }

        public CombinationKey(IEnumerable<KeyCode> keys)
        {
            Keys = new List<KeyCode>(keys);
        }

        public CombinationKey(params KeyCode[] keys) : this(keys as IEnumerable<KeyCode>)
        {
        }

        /// <summary>
        /// 删除组合键中的重复按键;
        /// </summary>
        public void Normalize()
        {
            Keys.RemoveSame();
        }

        public override int GetHashCode()
        {
            int hashCode = Keys.Count ^ Keys.Sum(key => (int)key);
            return hashCode;
        }

        public override bool Equals(object obj)
        {
            if (obj is CombinationKey)
            {
                return Equals((CombinationKey)obj);
            }
            return false;
        }

        public bool Equals(CombinationKey other)
        {
            return Keys.IsSameContent(other.Keys);
        }
    }
}
