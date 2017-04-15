using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using KouXiaGu.Collections;
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
        [XmlElement("HeightAdjustTex")]
        public string HeightAdjustTex;

        /// <summary>
        /// 高度调整的权重贴图;
        /// </summary>
        [XmlElement("HeightAdjustBlendTex")]
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
    public class TerrainRoad : TerrainElementInfo<RoadInfo>, IDisposable
    {
        public TerrainRoad(RoadInfo info) : base(info)
        {
        }

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
    public class RoadReadRequest : AssetReadRequest<Dictionary<int, TerrainRoad>>
    {

        public RoadReadRequest(AssetBundle assetBundle, ISegmented segmented, IEnumerable<RoadInfo> infos) 
            : base(assetBundle, segmented)
        {
            this.infos = infos;
            dictionary = new Dictionary<int, TerrainRoad>();
        }

        public RoadReadRequest(AssetBundle assetBundle, ISegmented segmented, WorldElementResource elementInfo)
            : this(assetBundle, segmented, elementInfo.RoadInfos.Values)
        {
        }

        IEnumerable<RoadInfo> infos;
        Dictionary<int, TerrainRoad> dictionary;

        protected override void OnFaulted(Exception ex)
        {
            base.OnFaulted(ex);
            dictionary.Values.DisposeAll();
            dictionary.Clear();
        }

        protected override IEnumerator Operate()
        {
            foreach (var info in infos)
            {
                TerrainRoad item;
                if (TryReadAndReport(info, out item))
                    dictionary.AddOrUpdate(info.ID, item);
                yield return null;
            }
            OnCompleted(dictionary);
        }

        bool TryReadAndReport(RoadInfo info, out TerrainRoad item)
        {
            if (TryRead(info, out item))
            {
                return true;
            }
            Debug.LogWarning("无法读取[TerrainRoad],Info:" + info.ToString());
            return false;
        }

        bool TryRead(RoadInfo info, out TerrainRoad item)
        {
            TerrainRoadInfo tInfo = info.Terrain;
            item = new TerrainRoad(info)
            {
                DiffuseTex = ReadTexture(tInfo.DiffuseTex),
                DiffuseBlendTex = ReadTexture(tInfo.DiffuseBlendTex),
                HeightAdjustTex = ReadTexture(tInfo.HeightAdjustTex),
                HeightAdjustBlendTex = ReadTexture(tInfo.HeightAdjustTex),
            };
            return item.IsLoadComplete;
        }

    }

}
