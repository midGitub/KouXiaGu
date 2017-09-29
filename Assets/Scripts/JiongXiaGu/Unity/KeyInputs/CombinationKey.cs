using JiongXiaGu.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using UnityEngine;

namespace JiongXiaGu.Unity.KeyInputs
{

    /// <summary>
    /// KeyCode 类型的组合键;
    /// </summary>
    [XmlRoot("CombinationKey")]
    public struct CombinationKey : IEquatable<CombinationKey>, IXmlSerializable
    {
        /// <summary>
        /// 组合按键合集;
        /// </summary>
        List<KeyCode> keys;

        public IEnumerable<KeyCode> Keys
        {
            get { return keys ?? (IEnumerable<KeyCode>)EmptyCollection<KeyCode>.Default; }
        }

        public int KeyCount
        {
            get { return keys != null ? keys.Count : 0; }
        }

        public CombinationKey(IEnumerable<KeyCode> keys)
        {
            this.keys = new List<KeyCode>(keys);
        }

        public CombinationKey(params KeyCode[] keys) : this(keys as IEnumerable<KeyCode>)
        {
        }

        public CombinationKey(CombinationKey combinationKey)
        {
            keys = new List<KeyCode>(combinationKey.keys);
        }

        /// <summary>
        /// 添加按键;
        /// </summary>
        void AddKey(KeyCode key)
        {
            if (key != KeyCode.None)
            {
                if (keys == null)
                {
                    keys = new List<KeyCode>();
                }
                keys.Add(key);
            }
        }

        /// <summary>
        /// 删除组合键中的重复按键;
        /// </summary>
        public void Normalize()
        {
            if (keys.Count <= 1)
                return;

            int i = 0;
            while (true)
            {
                var item = keys[i];
                if (item == KeyCode.None)
                {
                    keys.RemoveAt(i);
                }
                else if (++i < keys.Count)
                {
                    keys.RemoveAll(i, other => item.Equals(other));
                }
                else
                {
                    break;
                }
            }
            keys.Capacity = keys.Count;
        }

        public override string ToString()
        {
            string str = "[";
            for (int i = 0; i < keys.Count; i++)
            {
                str += string.Format("key{0}:{1}{2}", i, keys[i].ToString(), i == keys.Count - 1 ? string.Empty : ", ");
            }
            return str + "]";
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

        const string Key_Separator = ", ";
        static readonly string[] Key_SeparatorArray = new string[]
            {
                ","
            };

        void IXmlSerializable.ReadXml(XmlReader reader)
        {
            string keyNameString = reader.ReadElementContentAsString();
            if (!string.IsNullOrEmpty(keyNameString))
            {
                string[] keyNames = keyNameString.Split(Key_SeparatorArray, StringSplitOptions.RemoveEmptyEntries);
                foreach (var keyName in keyNames)
                {
                    KeyCode keyCode = (KeyCode)Enum.Parse(typeof(KeyCode), keyName, true);
                    AddKey(keyCode);
                }
                Normalize();
            }
        }

        void IXmlSerializable.WriteXml(XmlWriter writer)
        {
            string value = string.Join(Key_Separator, keys);
            writer.WriteValue(value);
        }

        public static bool operator ==(CombinationKey v1, CombinationKey v2)
        {
            return v1.Equals(v2);
        }

        public static bool operator !=(CombinationKey v1, CombinationKey v2)
        {
            return !v1.Equals(v2);
        }
    }
}
