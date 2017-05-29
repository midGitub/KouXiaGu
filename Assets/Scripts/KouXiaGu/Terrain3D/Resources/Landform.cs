using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using KouXiaGu.World;
using UnityEngine;
using KouXiaGu.Collections;
using KouXiaGu.Resources;
using System.Linq;

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

        [XmlAttribute("tag")]
        public string Tags { get; set; }

        [XmlElement("Terrain")]
        public TerrainLandformInfo TerrainInfo { get; set; }

        [XmlIgnore]
        public int TagsMask { get; set; }

        [XmlIgnore]
        public LandformResource Terrain { get; set; }
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

    class LandformInfoXmlSerializer : ElementsXmlSerializer<LandformInfo>
    {
        public LandformInfoXmlSerializer() : base(new LandformFile())
        {
        }

        public LandformInfoXmlSerializer(IFilePath file) : base(file)
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

    public class LandformResourcesReader : AsyncOperation
    {
        public IEnumerator ReadAsync(ISegmented stopwatch, AssetBundle assetBundle, IDictionary<int, LandformInfo> infoDictionary)
        {
            foreach (var info in infoDictionary.Values.ToArray())
            {
                TerrainLandformInfo tInfo = info.TerrainInfo;

                info.Terrain = new LandformResource(info)
                {
                    DiffuseTex = ReadTexture(assetBundle, tInfo.DiffuseTex),
                    DiffuseBlendTex = ReadTexture(assetBundle, tInfo.DiffuseBlendTex),
                    HeightTex = ReadTexture(assetBundle, tInfo.HeightTex),
                    HeightBlendTex = ReadTexture(assetBundle, tInfo.HeightBlendTex),
                };

                if (!info.Terrain.IsLoadComplete)
                {
                    Debug.LogWarning("无法读取[TerrainLandform],Info:" + info.ToString());
                    infoDictionary.Remove(info.ID);
                }

                if (stopwatch.Await())
                {
                    yield return null;
                    stopwatch.Restart();
                }
            }
            OnCompleted();
        }

        private Texture ReadTexture(AssetBundle assetBundle, string name)
        {
            return assetBundle.LoadAsset<Texture>(name);
        }
    }
}
