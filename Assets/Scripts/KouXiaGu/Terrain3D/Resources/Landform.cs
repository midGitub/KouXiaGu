using System;
using System.Xml.Serialization;
using UnityEngine;
using KouXiaGu.Concurrent;
using KouXiaGu.World.Resources;

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
        public string HeightTex;

        /// <summary>
        /// 高度调整的权重贴图;
        /// </summary>
        [XmlElement("HeightBlendTex")]
        public string HeightBlendTex;

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

    public class LandformResourceReader : AsyncRequest
    {
        public LandformResourceReader(AssetBundle assetBundle, LandformInfo info)
        {
            this.assetBundle = assetBundle;
            this.info = info;
        }

        AssetBundle assetBundle;
        LandformInfo info;

        protected override bool Operate()
        {
            base.Operate();
            TerrainLandformInfo tInfo = info.TerrainInfo;
            var resource = new LandformResource(info)
            {
                DiffuseTex = ReadTexture(tInfo.DiffuseTex),
                DiffuseBlendTex = ReadTexture(tInfo.DiffuseBlendTex),
                HeightTex = ReadTexture(tInfo.HeightTex),
                HeightBlendTex = ReadTexture(tInfo.HeightBlendTex),
            };

            if (resource.IsLoadComplete)
            {
                info.Terrain = resource;
            }
            else
            {
                Debug.LogWarning("无法读取[TerrainLandform],Info:" + info.ToString());
            }
            return false;
        }

        Texture ReadTexture(string name)
        {
            return assetBundle.LoadAsset<Texture>(name);
        }
    }
}
