using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using UnityEngine;

namespace JiongXiaGu.Inputs
{

    /// <summary>
    /// KeyCode 类型的组合键;
    /// </summary>
    [XmlRoot("CombinationKey")]
    public class CombinationKey : IEquatable<CombinationKey>, IXmlSerializable
    {
        [XmlElement]
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

        /// <summary>
        /// 用于XML序列化的信息;
        /// </summary>
        const string Key_ElementName = "Key";

        XmlSchema IXmlSerializable.GetSchema()
        {
            return null;
        }

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
