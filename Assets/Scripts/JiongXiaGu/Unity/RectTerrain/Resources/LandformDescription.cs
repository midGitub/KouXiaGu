using JiongXiaGu.Unity.Resources;
using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace JiongXiaGu.Unity.RectTerrain
{

    /// <summary>
    /// 地形资源描述;
    /// </summary>
    [XmlRoot("LandformDescription")]
    public struct LandformDescription : IEquatable<LandformDescription>
    {
        [XmlAttribute("id")]
        public string ID { get; set; }

        [XmlAttribute("name")]
        public string Name { get; set; }

        /// <summary>
        /// 高度调整贴图;
        /// </summary>
        [XmlElement("HeightTex")]
        public AssetInfo HeightTex { get; set; }

        /// <summary>
        /// 高度调整的权重贴图;
        /// </summary>
        [XmlElement("HeightBlendTex")]
        public AssetInfo HeightBlendTex { get; set; }

        /// <summary>
        /// 漫反射贴图名;
        /// </summary>
        [XmlElement("DiffuseTex")]
        public AssetInfo DiffuseTex { get; set; }

        /// <summary>
        /// 漫反射混合贴图名;
        /// </summary>
        [XmlElement("DiffuseBlendTex")]
        public AssetInfo DiffuseBlendTex { get; set; }

        public override bool Equals(object obj)
        {
            return obj is LandformDescription && Equals((LandformDescription)obj);
        }

        public bool Equals(LandformDescription other)
        {
            return ID == other.ID &&
                   Name == other.Name &&
                   EqualityComparer<AssetInfo>.Default.Equals(HeightTex, other.HeightTex) &&
                   EqualityComparer<AssetInfo>.Default.Equals(HeightBlendTex, other.HeightBlendTex) &&
                   EqualityComparer<AssetInfo>.Default.Equals(DiffuseTex, other.DiffuseTex) &&
                   EqualityComparer<AssetInfo>.Default.Equals(DiffuseBlendTex, other.DiffuseBlendTex);
        }

        public override int GetHashCode()
        {
            var hashCode = 765293214;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(ID);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Name);
            hashCode = hashCode * -1521134295 + EqualityComparer<AssetInfo>.Default.GetHashCode(HeightTex);
            hashCode = hashCode * -1521134295 + EqualityComparer<AssetInfo>.Default.GetHashCode(HeightBlendTex);
            hashCode = hashCode * -1521134295 + EqualityComparer<AssetInfo>.Default.GetHashCode(DiffuseTex);
            hashCode = hashCode * -1521134295 + EqualityComparer<AssetInfo>.Default.GetHashCode(DiffuseBlendTex);
            return hashCode;
        }

        public static bool operator ==(LandformDescription description1, LandformDescription description2)
        {
            return description1.Equals(description2);
        }

        public static bool operator !=(LandformDescription description1, LandformDescription description2)
        {
            return !(description1 == description2);
        }
    }
}
