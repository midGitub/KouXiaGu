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


    public class LandformReader : AssetReadRequest<Dictionary<int, TerrainLandform>>
    {
        public LandformReader(AssetBundle assetBundle, ISegmented segmented, WorldElementResource elementInfo) 
            : base(assetBundle, segmented)
        {
            this.elementInfo = elementInfo;
            dictionary = new Dictionary<int, TerrainLandform>();
        }

        WorldElementResource elementInfo;
        Dictionary<int, TerrainLandform> dictionary;

        protected override IEnumerator Operate()
        {
            foreach (var info in elementInfo.LandformInfos)
            {
                TerrainLandform item;
                if (TryReadAndReport(info.Value, out item))
                    dictionary.Add(info.Key, item);
                throw new Exception();
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
            Debug.LogWarning("无法读取[" + typeof(TerrainLandform).Name + "],Info:" + info.ToString());
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

    public class OLandformReader : TerrainAssetReader<TerrainLandform, LandformInfo>
    {
        public OLandformReader(ISegmented segmented) : base(segmented)
        {
        }

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
