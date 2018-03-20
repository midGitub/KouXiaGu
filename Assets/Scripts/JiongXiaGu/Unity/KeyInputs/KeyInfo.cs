using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using UnityEngine;

namespace JiongXiaGu.Unity.KeyInputs
{


    /// <summary>
    /// 按键信息;
    /// </summary>
    public struct KeyInfo : IEquatable<KeyInfo>, IXmlSerializable
    {
        private const string NameAttributeName = "name";

        /// <summary>
        /// 功能名称;
        /// </summary>
        public string Name { get; set; }

        private KeyCode key;

        /// <summary>
        /// 按键信息;
        /// </summary>
        public KeyCode Key
        {
            get { return key; }
            set { key = value; }
        }

        public KeyInfo(string name, KeyCode key)
        {
            Name = name;
            this.key = key;
        }

        public override bool Equals(object obj)
        {
            return obj is KeyInfo && Equals((KeyInfo)obj);
        }

        public bool Equals(KeyInfo other)
        {
            return Name == other.Name &&
                   Key == other.Key;
        }

        public override int GetHashCode()
        {
            var hashCode = -314821886;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Name);
            hashCode = hashCode * -1521134295 + Key.GetHashCode();
            return hashCode;
        }

        XmlSchema IXmlSerializable.GetSchema()
        {
            return null;
        }

        void IXmlSerializable.ReadXml(XmlReader reader)
        {
            Name = reader.GetAttribute(NameAttributeName);
            var keyStr = reader.ReadElementContentAsString();
            Enum.TryParse(keyStr, true, out key);
        }

        void IXmlSerializable.WriteXml(XmlWriter writer)
        {
            writer.WriteAttributeString(NameAttributeName, Name);
            writer.WriteValue(key.ToString());
            writer.WriteEndElement();
        }

        public static bool operator ==(KeyInfo info1, KeyInfo info2)
        {
            return info1.Equals(info2);
        }

        public static bool operator !=(KeyInfo info1, KeyInfo info2)
        {
            return !(info1 == info2);
        }
    }
}
