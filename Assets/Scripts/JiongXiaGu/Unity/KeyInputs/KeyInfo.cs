using System;
using System.Xml.Serialization;

namespace JiongXiaGu.Unity.KeyInputs
{

    /// <summary>
    /// 功能按键信息;
    /// </summary>
    [XmlRoot("KeyInfo")]
    public struct KeyInfo : IEquatable<KeyInfo>
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

        public KeyInfo(string name, CombinationKey key)
        {
            Name = name;
            Key = key;
        }

        public override int GetHashCode()
        {
            return Name.GetHashCode() ^ Key.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public bool Equals(KeyInfo other)
        {
            return Name == other.Name
                && Key == other.Key;
        }

        public static bool operator ==(KeyInfo v1, KeyInfo v2)
        {
            return v1.Equals(v2);
        }

        public static bool operator !=(KeyInfo v1, KeyInfo v2)
        {
            return !v1.Equals(v2);
        }
    }
}
