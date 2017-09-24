using System;
using UnityEngine;
using System.Xml.Serialization;
using System.Xml;
using System.Xml.Schema;

namespace JiongXiaGu.Inputs
{

    /// <summary>
    /// 提供两个按键组合
    /// </summary>
    public struct CustomKey2 : IEquatable<CustomKey2>, IXmlSerializable
    {
        const string Key0_AttributeName = "key0";
        const string Key1_AttributeName = "key1";

        public KeyCode Key0 { get; set; }
        public KeyCode Key1 { get; set; }

        public CustomKey2(KeyCode key0, KeyCode key1)
        {
            Key0 = key0;
            Key1 = key1;
        }

        public override string ToString()
        {
            return "[Key0:" + Key0 + ",Key1:" + Key1 + "]";
        }

        public override int GetHashCode()
        {
            return Key0.GetHashCode() ^ Key1.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (obj is CustomKey2)
            {
                return Equals((CustomKey2)obj);
            }
            return false;
        }

        bool IEquatable<CustomKey2>.Equals(CustomKey2 other)
        {
            if (Key0 == other.Key0 && Key1 == other.Key1)
            {
                return true;
            }
            else if (Key0 == other.Key1 && Key1 == other.Key0)
            {
                return true;
            }
            return false;
        }

        XmlSchema IXmlSerializable.GetSchema()
        {
            return null;
        }

        void IXmlSerializable.ReadXml(XmlReader reader)
        {
            try
            {
                string key0 = reader.GetAttribute(Key0_AttributeName);
                string key1 = reader.GetAttribute(Key1_AttributeName);

                if (key0 == key1)
                {
                    Key0 = ConvertToKeyCode(key0);
                    Key1 = KeyCode.None;
                }
                else
                {
                    Key0 = ConvertToKeyCode(key0);
                    Key1 = ConvertToKeyCode(key1);
                }
            }
            catch(Exception ex)
            {
                Debug.LogWarning("未知功能键:" + ToString() + ex);
            }
            finally
            {
                reader.Read();
            }
        }

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
            if (Key0 == Key1)
            {
                WriteKeyAttributeString(writer, Key0_AttributeName, Key0);
            }
            else
            {
                WriteKeyAttributeString(writer, Key0_AttributeName, Key0);
                WriteKeyAttributeString(writer, Key0_AttributeName, Key1);
            }
        }

        static void WriteKeyAttributeString(XmlWriter writer, string name, KeyCode value)
        {
            if (value != KeyCode.None)
            {
                writer.WriteAttributeString(name, value.ToString());
            }
        }
    }
}
