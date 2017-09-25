using JiongXiaGu.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using UnityEngine;

namespace JiongXiaGu.Unity.Inputs
{

    /// <summary>
    /// KeyCode 类型的组合键;
    /// </summary>
    [XmlRoot("CombinationKey")]
    public class CombinationKey : IEquatable<CombinationKey>, IXmlSerializable
    {
        /// <summary>
        /// 组合按键合集;
        /// </summary>
        List<KeyCode> keys;

        public IEnumerable<KeyCode> Keys
        {
            get { return keys; }
        }

        public int KeyCount
        {
            get { return keys.Count; }
        }

        public CombinationKey()
        {
            keys = new List<KeyCode>();
        }

        public CombinationKey(IEnumerable<KeyCode> keys)
        {
            this.keys = new List<KeyCode>(keys);
        }

        public CombinationKey(params KeyCode[] keys) : this(keys as IEnumerable<KeyCode>)
        {
        }

        /// <summary>
        /// 删除组合键中的重复按键;
        /// </summary>
        public void Normalize()
        {
            keys.RemoveSame();
            keys.Capacity = keys.Count;
        }

        public override int GetHashCode()
        {
            int hashCode = keys.Count ^ keys.Sum(key => (int)key);
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
            return keys.IsSameContent(other.keys);
        }

        XmlSchema IXmlSerializable.GetSchema()
        {
            return null;
        }


        /// <summary>
        /// 用于XML序列化的信息;
        /// </summary>
        const string Key_ElementName = "Key";

        void IXmlSerializable.ReadXml(XmlReader reader)
        {
            while (reader.ReadToNextSibling(Key_ElementName))
            {
                string key = reader.ReadElementContentAsString();

            }
            throw new NotImplementedException();
        }

        /// <summary>
        /// 转换为KeyCode类型;
        /// </summary>
        static KeyCode ConvertToKeyCode(string name)
        {
            if (!string.IsNullOrEmpty(name))
            {
                return (KeyCode)Enum.Parse(typeof(KeyCode), name, true);
            }
            else
            {
                return KeyCode.None;
            }
        }

        void IXmlSerializable.WriteXml(XmlWriter writer)
        {
            throw new NotImplementedException();
        }
    }
}
