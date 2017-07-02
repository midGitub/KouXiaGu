using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using UnityEngine;

namespace KouXiaGu.KeyInput
{

    /// <summary>
    /// 自定义按键 和 UnityEngine.KeyCode 的映射;
    /// </summary>
    [XmlType("Key")]
    public struct CustomKey : IEquatable<CustomKey>, IXmlSerializable
    {
        public CustomKey(KeyFunction function, KeyCode keyCode)
        {
            this.function = function;
            this.keyCode = keyCode;
        }

        [XmlAttribute("function")]
        public KeyFunction function { get; private set; }

        [XmlAttribute("key")]
        public KeyCode keyCode { get; private set; }

        public override bool Equals(object obj)
        {
            if (!(obj is CustomKey))
                return false;
            return Equals((CustomKey)obj);
        }

        public bool Equals(CustomKey other)
        {
            return function == other.function &&
                keyCode == other.keyCode;
        }

        public override int GetHashCode()
        {
            return ((int)function) ^ ((int)keyCode);
        }

        public override string ToString()
        {
            return "Function:" + function.ToString() + ";Key:" + keyCode.ToString();
        }


        XmlSchema IXmlSerializable.GetSchema()
        {
            return null;
        }

        void IXmlSerializable.ReadXml(XmlReader reader)
        {
            string function = reader.GetAttribute("function");
            string keyCode = reader.GetAttribute("key");
            reader.Read();

            try
            {
                this.function = (KeyFunction)Enum.Parse(typeof(KeyFunction), function, true);
                this.keyCode = (KeyCode)Enum.Parse(typeof(KeyCode), keyCode, true);
            }
            catch
            {
                Debug.LogWarning("未知功能键:" + function);
            }
        }

        void IXmlSerializable.WriteXml(XmlWriter writer)
        {
            writer.WriteAttributeString("function", function.ToString());
            writer.WriteAttributeString("key", keyCode.ToString());
        }

        public static implicit operator KeyValuePair<KeyFunction, KeyCode>(CustomKey custom)
        {
            return new KeyValuePair<KeyFunction, KeyCode>(custom.function, custom.keyCode);
        }

        public static implicit operator KeyValuePair<int, KeyCode>(CustomKey custom)
        {
            return new KeyValuePair<int, KeyCode>((int)custom.function, custom.keyCode);
        }
    }
}
