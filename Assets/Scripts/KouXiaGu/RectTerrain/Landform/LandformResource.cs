using System;
using System.Xml.Serialization;
using UnityEngine;
using KouXiaGu.Concurrent;
using KouXiaGu.World.Resources;

namespace KouXiaGu.RectTerrain
{

    /// <summary>
    /// 地貌资源信息;
    /// </summary>
    [XmlRoot("LandformResourceInfo")]
    public class LandformResourceInfo
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
    public class LandformResource
    {
        public LandformResourceInfo Info { get; protected set; }
        public Texture DiffuseTex { get; protected set; }
        public Texture DiffuseBlendTex { get; protected set; }
        public Texture HeightTex { get; protected set; }
        public Texture HeightBlendTex { get; protected set; }

        /// <summary>
        /// 是否为空?
        /// </summary>
        public bool IsEmpty
        {
            get
            {
                return
                    DiffuseTex == null &&
                    DiffuseBlendTex == null &&
                    HeightTex == null &&
                    HeightBlendTex == null;
            }
        }

        /// <summary>
        /// 是否所有都不为空?
        /// </summary>
        public bool IsComplete
        {
            get
            {
                return
                    DiffuseTex != null &&
                    DiffuseBlendTex != null &&
                    HeightTex != null &&
                    HeightBlendTex != null;
            }
        }

        /// <summary>
        /// 销毁所有贴图;
        /// </summary>
        public void Destroy()
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

    /// <summary>
    /// 地貌资源读取;
    /// </summary>
    public class LandformResourceAsyncReader : LandformResource, IAsyncRequest
    {
        public LandformResourceAsyncReader(AssetBundle assetBundle, LandformResourceInfo info)
        {
            this.assetBundle = assetBundle;
            Info = info;
        }

        AssetBundle assetBundle;
        public bool InQueue { get; private set; }

        void IAsyncRequest.OnAddQueue()
        {
            InQueue = true;
        }

        void IAsyncRequest.OnQuitQueue()
        {
            InQueue = false;
            assetBundle = null;
        }

        bool IAsyncRequest.Prepare()
        {
            return true;
        }

        bool IAsyncRequest.Operate()
        {
            DiffuseTex = ReadTexture(Info.DiffuseTex);
            DiffuseBlendTex = ReadTexture(Info.DiffuseBlendTex);
            HeightTex = ReadTexture(Info.HeightTex);
            HeightBlendTex = ReadTexture(Info.HeightBlendTex);

            if (!IsComplete)
            {
                Debug.LogWarning("无法读取[LandformResource],Info:" + ToString());
            }
            return false;
        }

        Texture ReadTexture(string name)
        {
            return assetBundle.LoadAsset<Texture>(name);
        }
    }
}
