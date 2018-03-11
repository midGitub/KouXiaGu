using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace JiongXiaGu.Unity.Resources
{

    public enum AssetFrom
    {
        Stream,
        AssetBundle,
    }

    /// <summary>
    /// 表示Unity资源;
    /// </summary>
    public struct AssetInfo : IXmlSerializable, IEquatable<AssetInfo>
    {
        /// <summary>
        /// 来自的文件;
        /// </summary>
        public AssetFrom From { get; set; }

        /// <summary>
        /// 资源包名称;
        /// </summary>
        public string BundleName { get; set; }

        /// <summary>
        /// 若从 AssetBundle 读取,则为文件名,忽略拓展名;
        /// 若从 File 读取,则为相对路径;
        /// </summary>
        public string Name { get; set; }

        public AssetInfo(string name) : this()
        {
            From = AssetFrom.Stream;
            BundleName = null;
            Name = name;
        }

        public AssetInfo(string bundleName, string name) : this()
        {
            From = AssetFrom.AssetBundle;
            BundleName = bundleName;
            Name = name;
        }

        internal const string AssetFromAttribute = "from";
        internal const string BundleNameAttribute = "bundleName";

        XmlSchema IXmlSerializable.GetSchema()
        {
            return null;
        }

        void IXmlSerializable.ReadXml(XmlReader reader)
        {
            string fromStr = reader.GetAttribute(AssetFromAttribute);
            From = (AssetFrom)Enum.Parse(typeof(AssetFrom), fromStr, true);
            BundleName = reader.GetAttribute(BundleNameAttribute);
            Name = reader.ReadElementContentAsString();
        }

        void IXmlSerializable.WriteXml(XmlWriter writer)
        {
            writer.WriteAttributeString(AssetFromAttribute, From.ToString());

            if (!string.IsNullOrWhiteSpace(BundleName))
                writer.WriteAttributeString(AssetFromAttribute, BundleName);

            writer.WriteValue(Name);
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
            hashCode = hashCode * -1521134295 + EqualityComparer<AssetFrom>.Default.GetHashCode(From);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Name);
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
