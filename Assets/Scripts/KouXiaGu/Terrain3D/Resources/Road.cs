using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;
using KouXiaGu.Resources;
using System.Linq;

namespace KouXiaGu.Terrain3D
{

    /// <summary>
    /// 道路信息;
    /// </summary>
    [XmlType("Road")]
    public class RoadInfo : IElement
    {
        [XmlAttribute("id")]
        public int ID { get; set; }

        [XmlElement("Terrain")]
        public TerrainRoadInfo TerrainInfo { get; set; }

        [XmlIgnore]
        public RoadResource Terrain { get; set; }
    }

    class RoadFile : MultipleFilePath
    {
        [CustomFilePath("道路资源描述文件;", true)]
        public const string fileName = "World/Terrain/Road.xml";

        public override string FileName
        {
            get { return fileName; }
        }
    }

    class RoadInfoXmlSerializer : ElementsXmlSerializer<RoadInfo>
    {
        public RoadInfoXmlSerializer() : base(new RoadFile())
        {
        }

        public RoadInfoXmlSerializer(IFilePath file) : base(file)
        {
        }
    }

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

    public class RoadResourcesReader
    {
        public IEnumerator ReadAsync(ISegmented stopwatch, AssetBundle assetBundle, IDictionary<int, RoadInfo> infoDictionary)
        {
            foreach (var info in infoDictionary.Values.ToArray())
            {
                TerrainRoadInfo bInfo = info.TerrainInfo;

                info.Terrain = new RoadResource(info)
                {
                    DiffuseTex = ReadTexture(assetBundle, bInfo.DiffuseTex),
                    DiffuseBlendTex = ReadTexture(assetBundle, bInfo.DiffuseBlendTex),
                    HeightAdjustTex = ReadTexture(assetBundle, bInfo.HeightAdjustTex),
                    HeightAdjustBlendTex = ReadTexture(assetBundle, bInfo.HeightAdjustTex),
                };

                if (!info.Terrain.IsLoadComplete)
                {
                    Debug.LogWarning("无法读取[TerrainRoad],Info:" + info.ToString());
                    infoDictionary.Remove(info.ID);
                }

                if (stopwatch.Await())
                {
                    yield return null;
                    stopwatch.Restart();
                }
            }
        }

        private Texture ReadTexture(AssetBundle assetBundle, string name)
        {
            return assetBundle.LoadAsset<Texture>(name);
        }
    }
}
