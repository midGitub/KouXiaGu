using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using KouXiaGu.World;
using UnityEngine;

namespace KouXiaGu.Terrain3D
{

    /// <summary>
    /// 地貌信息;
    /// </summary>
    [XmlType("TerrainLandform")]
    public class TerrainLandformInfo
    {
        /// <summary>
        /// 高度调整贴图名;
        /// </summary>
        [XmlElement("HeightTex")]
        public string HeightTex { get; set; }

        /// <summary>
        /// 高度调整的权重贴图;
        /// </summary>
        [XmlElement("HeightBlendTex")]
        public string HeightBlendTex { get; set; }

        /// <summary>
        /// 漫反射贴图名;
        /// </summary>
        [XmlElement("DiffuseTex")]
        public string DiffuseTex { get; set; }

        /// <summary>
        /// 漫反射混合贴图名;
        /// </summary>
        [XmlElement("DiffuseBlendTex")]
        public string DiffuseBlendTex { get; set; }
    }


    public class TerrainLandform : TerrainElementInfo<LandformInfo>, IDisposable
    {
        public TerrainLandform(LandformInfo info) : base(info)
        {
        }

        public Texture DiffuseTex { get; internal set; }
        public Texture DiffuseBlendTex { get; internal set; }
        public Texture HeightTex { get; internal set; }
        public Texture HeightBlendTex { get; internal set; }

        public bool IsEmpty
        {
            get {
                return 
                    DiffuseTex == null &&
                    DiffuseBlendTex == null &&
                    HeightTex == null &&
                    HeightBlendTex == null;
            }
        }

        public bool IsLoadComplete
        {
            get {
                return 
                    DiffuseTex != null &&
                    DiffuseBlendTex != null &&
                    HeightTex != null && 
                    HeightBlendTex != null;
            }
        }

        public void Dispose()
        {
            if (!IsEmpty)
            {
                GameObject.Destroy(DiffuseTex);
                DiffuseTex = null;

                GameObject.Destroy(DiffuseBlendTex);
                DiffuseBlendTex = null;

                GameObject.Destroy(HeightTex);
                HeightTex = null;

                GameObject.Destroy(HeightBlendTex);
                HeightBlendTex = null;
            }
        }
    }


    public class LandformReader : TerrainAssetReader<TerrainLandform, LandformInfo>
    {



        public override bool TryRead(AssetBundle asset, LandformInfo info, out TerrainLandform item)
        {
            TerrainLandformInfo tInfo = info.Terrain;
            item = new TerrainLandform(info)
            {
                DiffuseTex = ReadTexture(asset, tInfo.DiffuseTex),
                DiffuseBlendTex = ReadTexture(asset, tInfo.DiffuseBlendTex),
                HeightTex = ReadTexture(asset, tInfo.HeightTex),
                HeightBlendTex = ReadTexture(asset, tInfo.HeightBlendTex),
            };
            return item.IsLoadComplete;
        }

    }

}
