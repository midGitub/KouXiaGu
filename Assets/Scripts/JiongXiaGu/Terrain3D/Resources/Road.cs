using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;
using JiongXiaGu.Resources;
using System.Linq;
using JiongXiaGu.Concurrent;
using JiongXiaGu.World.Resources;

namespace JiongXiaGu.Terrain3D
{

    /// <summary>
    /// 道路资源定义;
    /// </summary>
    [XmlType("TerrainRoad")]
    public class TerrainRoadInfo
    {
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
    public class RoadResource : IDisposable
    {
        public RoadResource(RoadInfo info)
        {
            Info = info;
        }

        public RoadInfo Info { get; internal set; }
        public Texture DiffuseTex { get; internal set; }
        public Texture DiffuseBlendTex { get; internal set; }

        public bool IsEmpty
        {
            get
            {
                return
                    DiffuseTex == null &&
                    DiffuseBlendTex == null;
            }
        }

        public bool IsLoadComplete
        {
            get
            {
                return
                    DiffuseTex != null &&
                    DiffuseBlendTex != null;
            }
        }

        public void Dispose()
        {
            Destroy(DiffuseTex);
            DiffuseTex = null;

            Destroy(DiffuseBlendTex);
            DiffuseBlendTex = null;
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


    public class RoadResourceReader : AsyncRequest
    {
        public RoadResourceReader(AssetBundle assetBundle, RoadInfo info)
        {
            this.assetBundle = assetBundle;
            this.info = info;
        }

        AssetBundle assetBundle;
        RoadInfo info;

        protected override bool Operate()
        {
            base.Operate();
            TerrainRoadInfo bInfo = info.TerrainInfo;

            var resource = new RoadResource(info)
            {
                DiffuseTex = ReadTexture(bInfo.DiffuseTex),
                DiffuseBlendTex = ReadTexture(bInfo.DiffuseBlendTex),
            };

            if (resource.IsLoadComplete)
            {
                info.Terrain = resource;
            }
            else
            {
                Debug.LogWarning("无法读取[TerrainRoad],Info:" + info.ToString());
            }
            return false;
        }

        Texture ReadTexture(string name)
        {
            return assetBundle.LoadAsset<Texture>(name);
        }
    }
}
