using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace JiongXiaGu.Unity.Resources
{

    /// <summary>
    /// AssetBundle 描述;
    /// </summary>
    public struct AssetBundleDescription : IXmlSerializable, IEquatable<AssetBundleDescription>
    {
        private const string NameAttribute = "name";

        /// <summary>
        /// 唯一名;
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 相对路径;
        /// </summary>
        public string RelativePath { get; set; }

        public AssetBundleDescription(string name) : this()
        {
            Name = name;
            RelativePath = GetDefalutRelativePath(name);
        }

        public AssetBundleDescription(string name, string path) : this()
        {
            Name = name;
            RelativePath = path;
        }

        private static string GetDefalutRelativePath(string name)
        {
            return Path.Combine("AssetBundles", name.ToLower());
        }

        XmlSchema IXmlSerializable.GetSchema()
        {
            return null;
        }

        void IXmlSerializable.ReadXml(XmlReader reader)
        {
            Name = reader.GetAttribute(NameAttribute);
            RelativePath = reader.ReadElementContentAsString();
            reader.ReadEndElement();
        }

        void IXmlSerializable.WriteXml(XmlWriter writer)
        {
            if (string.IsNullOrWhiteSpace(Name))
                throw new ArgumentNullException(nameof(Name));
            if (string.IsNullOrWhiteSpace(RelativePath))
                throw new ArgumentNullException(nameof(RelativePath));

            writer.WriteAttributeString(NameAttribute, Name);
            writer.WriteValue(RelativePath);
        }

        public override bool Equals(object obj)
        {
            return obj is AssetBundleDescription && Equals((AssetBundleDescription)obj);
        }

        public bool Equals(AssetBundleDescription other)
        {
            return Name == other.Name &&
              RelativePath == other.RelativePath;
        }

        public override int GetHashCode()
        {
            var hashCode = 2099663225;
            hashCode = hashCode * -1521134295 + base.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Name);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(RelativePath);
            return hashCode;
        }

        public static bool operator ==(AssetBundleDescription description1, AssetBundleDescription description2)
        {
            return description1.Equals(description2);
        }

        public static bool operator !=(AssetBundleDescription description1, AssetBundleDescription description2)
        {
            return !(description1 == description2);
        }
    }
}
