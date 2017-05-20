using KouXiaGu.World;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using UnityEngine;

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

        //public bool IsLoadComplete
        //{
        //    get { return Prefab != null && BuildingPrefab != null; }
        //}

        public void Dispose()
        {
            GameObject.Destroy(Prefab);
            Prefab = null;
        }
    }

    public class BuildingResourceReader : AsyncOperation<Dictionary<int, BuildingResource>>
    {
        public BuildingResourceReader(ISegmented stopwatch, AssetBundle assetBundle, IEnumerable<BuildingInfo> infos)
        {
            this.stopwatch = stopwatch;
            this.assetBundle = assetBundle;
            this.infos = infos;
        }

        readonly ISegmented stopwatch;
        readonly AssetBundle assetBundle;
        readonly IEnumerable<BuildingInfo> infos;

        /// <summary>
        /// 在协程读取;
        /// </summary>
        public IEnumerator ReadAsync()
        {
            Result = new Dictionary<int, BuildingResource>();
            foreach (BuildingInfo info in infos)
            {
                BuildingResource resource;
                if (TryRead(info, out resource))
                {
                    Result.Add(info.ID, resource);

                }

                if (stopwatch.Await())
                {
                    yield return null;
                    stopwatch.Restart();
                }
            }
            OnCompleted();
        }

        public bool TryRead(BuildingInfo info, out BuildingResource item)
        {
            TerrainBuildingInfo bInfo = info.Terrain;
            GameObject prefab = assetBundle.LoadAsset<GameObject>(bInfo.PrefabName);

            try
            {
                item = new BuildingResource(bInfo, prefab);
                return true;
            }
            catch (Exception ex)
            {
                Debug.LogWarning("无法读取[BuildingResourceReader],Info:" + info.ToString() + ",因为:" + ex);
                item = default(BuildingResource);
                return false;
            }
        }
    }
}
