using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using KouXiaGu.World;
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
        public Texture DiffuseTex { get; internal set; }
        public Texture DiffuseBlendTex { get; internal set; }
        public Texture HeightAdjustTex { get; internal set; }
        public Texture HeightAdjustBlendTex { get; internal set; }

        public bool IsEmpty
        {
            get
            {
                return
                    DiffuseTex == null &&
                    DiffuseBlendTex == null &&
                    HeightAdjustTex == null &&
                    HeightAdjustBlendTex == null;
            }
        }

        public bool IsLoadComplete
        {
            get
            {
                return
                    DiffuseTex != null &&
                    DiffuseBlendTex != null &&
                    HeightAdjustTex != null &&
                    HeightAdjustBlendTex != null;
            }
        }

        public void Dispose()
        {
            Destroy(DiffuseTex);
            DiffuseTex = null;

            Destroy(DiffuseBlendTex);
            DiffuseBlendTex = null;

            Destroy(HeightAdjustTex);
            HeightAdjustTex = null;

            Destroy(HeightAdjustBlendTex);
            HeightAdjustBlendTex = null;
        }

        void Destroy(UnityEngine.Object item)
        {
#if UNITY_EDITOR
            GameObject.DestroyImmediate(item);
#else
            GameObject.Destroy(item);
#endif
        }
    }


    /// <summary>
    /// 道路资源读取;
    /// </summary>
    public class TerrainRoadReader : AssetReadRequest<Dictionary<int, TerrainRoad>>
    {
        public TerrainRoadReader(AssetBundle assetBundle, ISegmented segmented, IEnumerable<RoadInfo> infos) 
            : base(assetBundle, segmented)
        {
            this.infos = infos;
            dictionary = new Dictionary<int, TerrainRoad>();
        }

        IEnumerable<RoadInfo> infos;
        Dictionary<int, TerrainRoad> dictionary;

        protected override IEnumerator Operate()
        {
            OnCompleted(dictionary);
            throw new NotImplementedException();
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
            return item;
        }

        Texture LoadTexture(AssetBundle assetBundle, string name)
        {
            return assetBundle.LoadAsset<Texture>(name);
        }

    }

}
