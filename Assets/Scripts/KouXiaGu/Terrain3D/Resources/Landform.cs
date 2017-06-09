using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using KouXiaGu.World;
using UnityEngine;
using KouXiaGu.Collections;
using KouXiaGu.Resources;
using System.Linq;
using KouXiaGu.Concurrent;

namespace KouXiaGu.Terrain3D
{

    /// <summary>
    /// 地形信息;
    /// </summary>
    [XmlType("Landform")]
    public class LandformInfo : IElement
    {
        [XmlAttribute("id")]
        public int ID { get; set; }

        [XmlAttribute("tags")]
        public string Tags { get; set; }

        [XmlElement("Terrain")]
        public TerrainLandformInfo TerrainInfo { get; set; }

        [XmlIgnore]
        public int TagsMask { get; internal set; }

        [XmlIgnore]
        public LandformResource Terrain { get; internal set; }

        public override string ToString()
        {
            return "[ID:" + ID + ",Tags:" + Tags + "]";
        }
    }

    class LandformFile : MultipleFilePath
    {
        [CustomFilePath("地形资源描述文件;", true)]
        public const string fileName = "World/Terrain/Landform.xml";

        public override string FileName
        {
            get { return fileName; }
        }
    }

    class LandformInfoXmlSerializer : XmlElementsReaderWriter<LandformInfo>
    {
        public LandformInfoXmlSerializer() : base(new LandformFile())
        {
        }
    }

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

    public class LandformResourceReader : IAsyncRequest
    {
        public LandformResourceReader(AssetBundle assetBundle, LandformInfo info)
        {
            this.assetBundle = assetBundle;
            this.info = info;
        }

        AssetBundle assetBundle;
        LandformInfo info;

        void IAsyncRequest.AddQueue() { }

        void IAsyncRequest.Operate()
        {
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
        }

        Texture ReadTexture(string name)
        {
            return assetBundle.LoadAsset<Texture>(name);
        }
    }
}
