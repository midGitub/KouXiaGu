using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

        void Destroy(Texture tex)
        {
#if UNITY_EDITOR
            GameObject.DestroyImmediate(tex);
#else
            GameObject.Destroy(tex);
#endif
        }

    }

    /// <summary>
    /// 资源读取;
    /// </summary>
    public class LandformReadRequest : AssetReadRequest<Dictionary<int, TerrainLandform>>
    {

        public LandformReadRequest(AssetBundle assetBundle, ISegmented segmented, IEnumerable<LandformInfo> infos)
            : base(assetBundle, segmented)
        {
            this.infos = infos;
            dictionary = new Dictionary<int, TerrainLandform>();
        }

        public LandformReadRequest(AssetBundle assetBundle, ISegmented segmented, WorldElementResource elementInfo) 
            : this(assetBundle, segmented, elementInfo.LandformInfos.Values)
        {
        }

        IEnumerable<LandformInfo> infos;
        Dictionary<int, TerrainLandform> dictionary;

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
                TerrainLandform item;
                if (TryReadAndReport(info, out item))
                    dictionary.Add(info.ID, item);
                yield return null;
            }
            OnCompleted(dictionary);
        }

        /// <summary>
        /// 尝试读取到,若无法读取则输出错误信息,并返回false;
        /// </summary>
        bool TryReadAndReport(LandformInfo info, out TerrainLandform item)
        {
            if (TryRead(info, out item))
            {
                return true;
            }
            Debug.LogWarning("无法读取[TerrainLandform],Info:" + info.ToString());
            return false;
        }

        bool TryRead(LandformInfo info, out TerrainLandform item)
        {
            TerrainLandformInfo tInfo = info.Terrain;
            item = new TerrainLandform(info)
            {
                DiffuseTex = ReadTexture(tInfo.DiffuseTex),
                DiffuseBlendTex = ReadTexture(tInfo.DiffuseBlendTex),
                HeightTex = ReadTexture(tInfo.HeightTex),
                HeightBlendTex = ReadTexture(tInfo.HeightBlendTex),
            };
            return item.IsLoadComplete;
        }
    }

}
