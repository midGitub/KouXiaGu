using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace JiongXiaGu.Unity.KeyInputs
{
    /// <summary>
    /// 组合按键信息;
    /// </summary>
    public struct CombinationKeyInfo : IEquatable<CombinationKeyInfo>
    {
        /// <summary>
        /// 功能名称;
        /// </summary>
        [XmlElement]
        public string Name { get; set; }

        /// <summary>
        /// 按键信息;
        /// </summary>
        [XmlElement]
        public CombinationKey Key { get; set; }

        public CombinationKeyInfo(string name, CombinationKey key)
        {
            Name = name;
            Key = key;
        }

        public override bool Equals(object obj)
        {
            return obj is CombinationKeyInfo && Equals((CombinationKeyInfo)obj);
        }

        public bool Equals(CombinationKeyInfo other)
        {
            return Name == other.Name &&
                   Key.Equals(other.Key);
        }

        public override int GetHashCode()
        {
            var hashCode = -314821886;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Name);
            hashCode = hashCode * -1521134295 + EqualityComparer<CombinationKey>.Default.GetHashCode(Key);
            return hashCode;
        }

        public static bool operator ==(CombinationKeyInfo info1, CombinationKeyInfo info2)
        {
            return info1.Equals(info2);
        }

        public static bool operator !=(CombinationKeyInfo info1, CombinationKeyInfo info2)
        {
            return !(info1 == info2);
        }
    }
}
