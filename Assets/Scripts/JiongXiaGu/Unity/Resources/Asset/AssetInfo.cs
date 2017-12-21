using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace JiongXiaGu.Unity.Resources
{

    /// <summary>
    /// 表示资源;
    /// </summary>
    public struct AssetInfo : IXmlSerializable, IEquatable<AssetInfo>
    {
        internal const AssetLoadModes DefaultLoadMode = AssetLoadModes.File;
        internal const string LoadModeAttribute = "from";
        internal const string AssetBundleNameAttribute = "assetBundle";

        /// <summary>
        /// 读取方式,默认从文件读取;
        /// </summary>
        public AssetLoadModes From { get; private set; }

        /// <summary>
        /// 若为 AssetBundle 的资源,则为 AssetBundleName,否则为null;
        /// </summary>
        public AssetPath AssetBundleName { get; private set; }

        /// <summary>
        /// 若从 AssetBundle 读取,则为文件名,忽略拓展名;
        /// 若从 File 读取,则为相对路径;
        /// </summary>
        public AssetPath Name { get; private set; }

        public AssetInfo(AssetPath name) : this()
        {
            From = AssetLoadModes.File;
            Name = name;
        }

        public AssetInfo(AssetPath assteBundleName, string name) : this()
        {
            From = AssetLoadModes.AssetBundle;
            AssetBundleName = assteBundleName;
            Name = name;

            if (Name.IsReferencePath())
            {
                throw new ArgumentException();
            }
        }

        XmlSchema IXmlSerializable.GetSchema()
        {
            return null;
        }

        void IXmlSerializable.ReadXml(XmlReader reader)
        {
            Name = reader.ReadElementContentAsString();
            string fromStr = reader.GetAttribute(LoadModeAttribute);
            try
            {
                From = (AssetLoadModes)Enum.Parse(typeof(AssetLoadModes), fromStr, true);

                switch (From)
                {
                    case AssetLoadModes.AssetBundle:
                        AssetBundleName = reader.GetAttribute(AssetBundleNameAttribute);
                        break;

                    default:
                        break;
                }
            }
            catch (ArgumentException)
            {
                From = AssetLoadModes.Unknown;
            }
        }

        void IXmlSerializable.WriteXml(XmlWriter writer)
        {
            switch (From)
            {
                case AssetLoadModes.File:
                    writer.WriteAttributeString(LoadModeAttribute, From.ToString());
                    writer.WriteValue(Name.ToString());
                    break;

                case AssetLoadModes.AssetBundle:
                    writer.WriteAttributeString(LoadModeAttribute, From.ToString());
                    writer.WriteAttributeString(AssetBundleNameAttribute, AssetBundleName.Name);
                    writer.WriteValue(Name.ToString());
                    break;

                default:
                    throw new NotSupportedException(From.ToString());
            }
        }

        public override bool Equals(object obj)
        {
            return obj is AssetInfo && Equals((AssetInfo)obj);
        }

        public bool Equals(AssetInfo other)
        {
            return From == other.From &&
                   AssetBundleName.Equals(other.AssetBundleName) &&
                   Name.Equals(other.Name);
        }

        public override int GetHashCode()
        {
            var hashCode = -167635157;
            hashCode = hashCode * -1521134295 + From.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<AssetPath>.Default.GetHashCode(AssetBundleName);
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
