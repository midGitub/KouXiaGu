using System;
using System.Xml.Serialization;
using UnityEngine;
using KouXiaGu.Concurrent;
using KouXiaGu.World.Resources;

namespace KouXiaGu.Terrain3D
{
    [XmlType("TerrainBuilding")]
    public class TerrainBuildingInfo
    {
        /// <summary>
        /// 建筑预制物体名;
        /// </summary>
        [XmlElement("PrefabName")]
        public string PrefabName { get; set; }

        /// <summary>
        /// 是否关联邻居节点?当此建筑进行创建或者销毁时,通知到所有邻居节点的相同建筑类型的实例;
        /// </summary>
        [XmlElement("AssociativeNeighbor")]
        public bool AssociativeNeighbor { get; set; }
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

        public void Dispose()
        {
            GameObject.Destroy(Prefab);
            Prefab = null;
        }
    }

    public class BuildingResourceReader : AsyncRequest
    {
        public BuildingResourceReader(AssetBundle assetBundle, BuildingInfo info)
        {
            this.assetBundle = assetBundle;
            this.info = info;
        }

        AssetBundle assetBundle;
        BuildingInfo info;

        protected override bool Operate()
        {
            TerrainBuildingInfo tInfo = info.TerrainInfo;
            GameObject prefab = assetBundle.LoadAsset<GameObject>(tInfo.PrefabName);

            try
            {
                var resource = new BuildingResource(tInfo, prefab);
                info.Terrain = resource;
            }
            catch (Exception ex)
            {
                Debug.LogWarning("无法读取[BuildingResourceReader],Info:" + info.ToString() + ",因为:" + ex);
            }
            return false;
        }
    }
}
