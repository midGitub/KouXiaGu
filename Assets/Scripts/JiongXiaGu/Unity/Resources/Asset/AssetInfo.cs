using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace JiongXiaGu.Unity.Resources
{

    /// <summary>
    /// 表示Unity资源;
    /// </summary>
    public struct AssetInfo : IXmlSerializable, IEquatable<AssetInfo>
    {
        internal const string AssetFromAttribute = "from";

        /// <summary>
        /// 来自的文件;
        /// </summary>
        public AssetPath From { get; private set; }

        /// <summary>
        /// 若从 AssetBundle 读取,则为文件名,忽略拓展名;
        /// 若从 File 读取,则为相对路径;
        /// </summary>
        public string Name { get; private set; }

        public AssetInfo(AssetPath assteBundleName, string name) : this()
        {
            From = assteBundleName;
            Name = name;
        }

        XmlSchema IXmlSerializable.GetSchema()
        {
            return null;
        }

        void IXmlSerializable.ReadXml(XmlReader reader)
        {
            string from = reader.GetAttribute(AssetFromAttribute);
            if (string.IsNullOrWhiteSpace(from))
            {
                throw new ArgumentNullException(nameof(from));
            }
            else
            {
                From = from;
            }

            string name = reader.ReadElementContentAsString();
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentNullException(nameof(name));
            }
            else
            {
                Name = name;
            }
        }

        void IXmlSerializable.WriteXml(XmlWriter writer)
        {
            writer.WriteAttributeString(AssetFromAttribute, From.Name);
            writer.WriteValue(Name.ToString());
        }

        public override bool Equals(object obj)
        {
            return obj is AssetInfo && Equals((AssetInfo)obj);
        }

        public bool Equals(AssetInfo other)
        {
            return From.Equals(other.From) &&
                   Name.Equals(other.Name);
        }

        public override int GetHashCode()
        {
            var hashCode = -167635157;
            hashCode = hashCode * -1521134295 + EqualityComparer<AssetPath>.Default.GetHashCode(From);
            hashCode = hashCode * -1521134295 + EqualityComparer<AssetPath>.Default.GetHashCode(Name);
            return hashCode;
        }

        public static bool operator ==(AssetInfo info1, AssetInfo info2)
        {
            return info1.Equals(info2);
        }

        public static bool operator !=(AssetInfo info1, AssetInfo info2)
        {
            return !(info1 == info2);
        }
    }
}
