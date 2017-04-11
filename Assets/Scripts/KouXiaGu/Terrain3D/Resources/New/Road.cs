using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using UnityEngine;

namespace KouXiaGu.Terrain3D
{

    /// <summary>
    /// 道路资源定义;
    /// </summary>
    [XmlType("TerrainRoad")]
    public class TerrainRoadInfo
    {
        /// <summary>
        /// 高度调整贴图名;
        /// </summary>
        [XmlElement("HeightTex")]
        public string HeightAdjustTex;

        /// <summary>
        /// 高度调整的权重贴图;
        /// </summary>
        [XmlElement("HeightBlendTex")]
        public string HeightAdjustBlendTex;

        /// <summary>
        /// 漫反射贴图名;
        /// </summary>
        [XmlElement("DiffuseTex")]
        public string DiffuseTex;

        /// <summary>
        /// 漫反射混合贴图名;
        /// </summary>
        [XmlElement("DiffuseBlendTex")]
        public string DiffuseBlendTex;
    }

    /// <summary>
    /// 道路 地形贴图资源;
    /// </summary>
    public class TerrainRoad : IDisposable
    {
        public Texture HeightAdjustTex { get; internal set; }
        public Texture HeightAdjustBlendTex { get; internal set; }
        public Texture DiffuseTex { get; internal set; }
        public Texture DiffuseBlendTex { get; internal set; }

        /// <summary>
        /// 是否全部不为 null;
        /// </summary>
        public bool IsComplete
        {
            get { return DiffuseTex != null && DiffuseBlendTex != null &&
                    HeightAdjustTex != null && HeightAdjustBlendTex != null;
            }
        }

        public void Dispose()
        {
            GameObject.Destroy(DiffuseTex);
            DiffuseTex = null;

            GameObject.Destroy(DiffuseBlendTex);
            DiffuseBlendTex = null;

            GameObject.Destroy(HeightAdjustTex);
            HeightAdjustTex = null;

            GameObject.Destroy(HeightAdjustBlendTex);
            HeightAdjustBlendTex = null;
        }

    }

    /// <summary>
    /// 道路资源读取;
    /// </summary>
    public class TerrainRoadReader
    {
        internal static readonly TerrainRoadReader DefaultReader = new TerrainRoadReader();


        public TerrainRoadReader()
        {
        }

        public TerrainRoad Read(TerrainRoadInfo info, AssetBundle terrain)
        {
            TerrainRoad item = new TerrainRoad()
            {
                DiffuseTex = LoadTexture(terrain, info.DiffuseTex),
                DiffuseBlendTex = LoadTexture(terrain, info.DiffuseBlendTex),
                HeightAdjustTex = LoadTexture(terrain, info.HeightAdjustTex),
                HeightAdjustBlendTex = LoadTexture(terrain, info.HeightAdjustTex),
            };

            if (!item.IsComplete)
            {
                item.Dispose();
                throw new ArgumentNullException("缺少贴图;");
            }
            return item;
        }

        Texture LoadTexture(AssetBundle assetBundle, string name)
        {
            return assetBundle.LoadAsset<Texture>(name);
        }

    }

}
