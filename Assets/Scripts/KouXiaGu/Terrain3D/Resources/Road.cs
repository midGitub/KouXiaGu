using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using KouXiaGu.Collections;
using KouXiaGu.World;
using UnityEngine;
using KouXiaGu.Resources;

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
    public class RoadResource : IDisposable
    {
        public RoadResource(RoadInfo info)
        {
            Info = info;
        }

        public RoadInfo Info { get; internal set; }
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
    public class RoadResourceReader : AsyncOperation<Dictionary<int, RoadResource>>
    {
        public RoadResourceReader(ISegmented stopwatch, AssetBundle assetBundle, IEnumerable<RoadInfo> infos) 
        {
            this.stopwatch = stopwatch;
            this.assetBundle = assetBundle;
            this.infos = infos;
        }

        readonly ISegmented stopwatch;
        readonly AssetBundle assetBundle;
        readonly IEnumerable<RoadInfo> infos;

        protected override void OnFaulted(Exception ex)
        {
            base.OnFaulted(ex);
            Result.Values.DisposeAll();
            Result.Clear();
        }

        public IEnumerator ReadAsync()
        {
            Result = new Dictionary<int, RoadResource>();
            foreach (var info in infos)
            {
                RoadResource resource;
                TryRead(info, out resource);
                Result.Add(info.ID, resource);

                if (stopwatch.Await())
                {
                    yield return null;
                    stopwatch.Restart();
                }
            }
            OnCompleted();
        }

        bool TryRead(RoadInfo info, out RoadResource item)
        {
            TerrainRoadInfo tInfo = info.Terrain;
            item = new RoadResource(info)
            {
                DiffuseTex = ReadTexture(tInfo.DiffuseTex),
                DiffuseBlendTex = ReadTexture(tInfo.DiffuseBlendTex),
                HeightAdjustTex = ReadTexture(tInfo.HeightAdjustTex),
                HeightAdjustBlendTex = ReadTexture(tInfo.HeightAdjustTex),
            };

            if (item.IsLoadComplete)
            {
                return true;
            }
            else
            {
                Debug.LogWarning("无法读取[TerrainRoad],Info:" + info.ToString());
                return false;
            }
        }

        private Texture ReadTexture(string name)
        {
            return assetBundle.LoadAsset<Texture>(name);
        }
    }

}
