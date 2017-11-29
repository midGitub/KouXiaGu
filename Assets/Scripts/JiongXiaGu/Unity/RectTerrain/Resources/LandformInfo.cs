using JiongXiaGu.Unity.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using UnityEngine;

namespace JiongXiaGu.Unity.RectTerrain.Resources
{

    public class LandformInfo
    {
        public LoadableContent Content { get; private set; }
        public LandformDescription Description { get; private set; }
        public Texture HeightTex { get; private set; }
        public Texture HeightBlendTex { get; private set; }
        public Texture DiffuseTex { get; set; }
        public Texture DiffuseBlendTex { get; set; }

        public LandformInfo(LandformDescription description)
        {
            Description = description;
        }
    }

    [XmlRoot("LandformDescription")]
    public struct LandformDescription
    {
        /// <summary>
        /// 唯一标识ID;
        /// </summary>
        [XmlAttribute("id")]
        public int ID { get; set; }

        /// <summary>
        /// 地形名;
        /// </summary>
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
    }
}
