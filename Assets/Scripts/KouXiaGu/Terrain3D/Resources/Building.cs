using KouXiaGu.Resources;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;
using System.Linq;
using KouXiaGu.World;
using KouXiaGu.Grids;

namespace KouXiaGu.Terrain3D
{

    /// <summary>
    /// 地形信息;
    /// </summary>
    [XmlType("Building")]
    public class BuildingInfo : IElement
    {
        [XmlAttribute("id")]
        public int ID { get; set; }

        [XmlAttribute("tags")]
        public string Tags { get; set; }

        [XmlElement("Terrain")]
        public TerrainBuildingInfo TerrainInfo { get; set; }

        [XmlIgnore]
        public int TagsMask { get; internal set; }

        [XmlIgnore]
        public BuildingResource Terrain { get; internal set; }
    }

    [XmlType("TerrainBuilding")]
    public class TerrainBuildingInfo
    {
        /// <summary>
        /// 建筑预制物体名;
        /// </summary>
        [XmlElement("PrefabName")]
        public string PrefabName { get; set; }
    }

    class BuildingInfoXmlSerializer : XmlElementsReaderWriter<BuildingInfo>
    {
        public BuildingInfoXmlSerializer() : base(new BuildingFile())
        {
        }
    }

    class BuildingFile : MultipleFilePath
    {
        [CustomFilePath("建筑资源描述文件;", true)]
        public const string fileName = "World/Terrain/Building.xml";

        public override string FileName
        {
            get { return fileName; }
        }
    }

    public class BuildingResource : IDisposable
    {
        public BuildingResource(TerrainBuildingInfo info, GameObject prefab)
        {
            if (info == null)
                throw new ArgumentNullException("info");
            if (prefab == null)
                throw new ArgumentNullException("prefab");

            Info = info;
            Prefab = prefab;
            BuildingPrefab = Prefab.GetComponent<IBuildingPrefab>();

            if (BuildingPrefab == null)
                throw new ArgumentException("预制物体未挂载对应建筑接口;");
        }

        public TerrainBuildingInfo Info { get; private set; }
        public GameObject Prefab { get; private set; }
        public IBuildingPrefab BuildingPrefab { get; private set; }

        /// <summary>
        /// 创建一个实例到该位置;
        /// </summary>
        public IBuilding BuildAt(IWorld world, CubicHexCoord position, float angele)
        {
            return BuildingPrefab.BuildAt(world, position, angele, this);
        }

        public void Dispose()
        {
            GameObject.Destroy(Prefab);
            Prefab = null;
        }
    }

    public class BuildingResourcesReader : AsyncOperation
    {
        public IEnumerator ReadAsync(ISegmented stopwatch, AssetBundle assetBundle, IDictionary<int, BuildingInfo> infoDictionary)
        {
            foreach (var info in infoDictionary.Values.ToArray())
            {
                TerrainBuildingInfo bInfo = info.TerrainInfo;
                GameObject prefab = assetBundle.LoadAsset<GameObject>(bInfo.PrefabName);

                try
                {
                    info.Terrain = new BuildingResource(bInfo, prefab);
                }
                catch (Exception ex)
                {
                    Debug.LogWarning("无法读取[BuildingResourceReader],Info:" + info.ToString() + ",因为:" + ex);
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
    }
}
