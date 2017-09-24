using System;
using UnityEngine;
using System.Xml.Serialization;
using System.Xml;
using System.Xml.Schema;

namespace JiongXiaGu.Inputs
{

    /// <summary>
    /// 自定义按键;
    /// </summary>
    public struct CustomKey : IEquatable<CustomKey>, IXmlSerializable
    {
        const string Name_AttributeName = "name";
        const string Key0_AttributeName = "key0";
        const string Key1_AttributeName = "key1";

        public string Name { get; set; }
        public KeyCode Key0 { get; set; }
        public KeyCode Key1 { get; set; }

        public CustomKey(string name, KeyCode key0, KeyCode key1)
        {
            Name = name;
            Key0 = key0;
            Key1 = key1;
        }

        public override string ToString()
        {
            return "[Name:" + Name + ",Key0:" + Key0 + ",Key1:" + Key1 + "]";
        }

        public override int GetHashCode()
        {
            return Key0.GetHashCode() ^ Key1.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (obj is CustomKey)
            {
                return Equals((CustomKey)obj);
            }
            return false;
        }

        bool IEquatable<CustomKey>.Equals(CustomKey other)
        {
            if (Name == other.Name)
            {
                if (Key0 == other.Key0 && Key1 == other.Key1)
                {
                    return true;
                }
                else if (Key0 == other.Key1 && Key1 == other.Key0)
                {
                    return true;
                }
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
                Name = reader.GetAttribute(Name_AttributeName);
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
            writer.WriteAttributeString(Name_AttributeName, Name.ToString());
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
