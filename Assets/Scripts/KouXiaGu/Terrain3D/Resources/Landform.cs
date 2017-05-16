using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using KouXiaGu.World;
using UnityEngine;
using KouXiaGu.Collections;

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


    /// <summary>
    /// 地貌贴图信息;
    /// </summary>
    public class LandformResource : IDisposable
    {
        public LandformResource(LandformInfo info)
        {
            Info = info;
        }

        public LandformInfo Info { get; internal set; }
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
                Destroy(DiffuseTex);
                DiffuseTex = null;

                Destroy(DiffuseBlendTex);
                DiffuseBlendTex = null;

                Destroy(HeightTex);
                HeightTex = null;

                Destroy(HeightBlendTex);
                HeightBlendTex = null;
            }
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

    [Serializable]
    public class LandformResourceReader : AsyncOperation<Dictionary<int, LandformResource>>
    {
        public LandformResourceReader(ISegmented stopwatch, AssetBundle assetBundle, IEnumerable<LandformInfo> infos)
        {
            this.stopwatch = stopwatch;
            this.assetBundle = assetBundle;
            this.infos = infos;
        }

        readonly ISegmented stopwatch;
        readonly AssetBundle assetBundle;
        readonly IEnumerable<LandformInfo> infos;

        public IEnumerator ReadAsync()
        {
            Result = new Dictionary<int, LandformResource>();
            foreach (var info in infos)
            {
                LandformResource resource;
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

        bool TryRead(LandformInfo info, out LandformResource item)
        {
            TerrainLandformInfo tInfo = info.Terrain;
            item = new LandformResource(info)
            {
                DiffuseTex = ReadTexture(tInfo.DiffuseTex),
                DiffuseBlendTex = ReadTexture(tInfo.DiffuseBlendTex),
                HeightTex = ReadTexture(tInfo.HeightTex),
                HeightBlendTex = ReadTexture(tInfo.HeightBlendTex),
            };

            if (item.IsLoadComplete)
            {
                return true;
            }
            else
            {
                Debug.LogWarning("无法读取[TerrainLandform],Info:" + info.ToString());
                return false;
            }
        }

        private Texture ReadTexture(string name)
        {
            return assetBundle.LoadAsset<Texture>(name);
        }
    }

    //[Obsolete]
    //public class LandformReadRequest : AssetReadRequest<Dictionary<int, LandformResource>>
    //{

    //    public LandformReadRequest(AssetBundle assetBundle, ISegmented segmented, IEnumerable<LandformInfo> infos)
    //        : base(assetBundle, segmented)
    //    {
    //        this.infos = infos;
    //        dictionary = new Dictionary<int, LandformResource>();
    //    }

    //    public LandformReadRequest(AssetBundle assetBundle, ISegmented segmented, WorldElementResource elementInfo) 
    //        : this(assetBundle, segmented, elementInfo.LandformInfos.Values)
    //    {
    //    }

    //    IEnumerable<LandformInfo> infos;
    //    Dictionary<int, LandformResource> dictionary;

    //    protected override void OnFaulted(Exception ex)
    //    {
    //        base.OnFaulted(ex);
    //        dictionary.Values.DisposeAll();
    //        dictionary.Clear();
    //    }

    //    protected override IEnumerator Operate()
    //    {
    //        foreach (var info in infos)
    //        {
    //            LandformResource item;
    //            if (TryReadAndReport(info, out item))
    //                dictionary.AddOrUpdate(info.ID, item);
    //            yield return null;
    //        }
    //        OnCompleted(dictionary);
    //    }

    //    /// <summary>
    //    /// 尝试读取到,若无法读取则输出错误信息,并返回false;
    //    /// </summary>
    //    bool TryReadAndReport(LandformInfo info, out LandformResource item)
    //    {
    //        if (TryRead(info, out item))
    //        {
    //            return true;
    //        }
    //        Debug.LogWarning("无法读取[TerrainLandform],Info:" + info.ToString());
    //        return false;
    //    }

    //    bool TryRead(LandformInfo info, out LandformResource item)
    //    {
    //        TerrainLandformInfo tInfo = info.Terrain;
    //        item = new LandformResource(info)
    //        {
    //            DiffuseTex = ReadTexture(tInfo.DiffuseTex),
    //            DiffuseBlendTex = ReadTexture(tInfo.DiffuseBlendTex),
    //            HeightTex = ReadTexture(tInfo.HeightTex),
    //            HeightBlendTex = ReadTexture(tInfo.HeightBlendTex),
    //        };
    //        return item.IsLoadComplete;
    //    }
    //}

}
