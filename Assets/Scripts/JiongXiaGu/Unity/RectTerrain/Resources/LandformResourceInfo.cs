using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace JiongXiaGu.Unity.RectTerrain.Resources
{

    [XmlRoot("LandformResourceInfo")]
    public struct LandformResourceInfo
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
        public TextureInfo HeightTex { get; set; }

        /// <summary>
        /// 高度调整的权重贴图;
        /// </summary>
        [XmlElement("HeightBlendTex")]
        public TextureInfo HeightBlendTex { get; set; }

        /// <summary>
        /// 漫反射贴图名;
        /// </summary>
        [XmlElement("DiffuseTex")]
        public TextureInfo DiffuseTex { get; set; }

        /// <summary>
        /// 漫反射混合贴图名;
        /// </summary>
        [XmlElement("DiffuseBlendTex")]
        public TextureInfo DiffuseBlendTex { get; set; }
    }
}
