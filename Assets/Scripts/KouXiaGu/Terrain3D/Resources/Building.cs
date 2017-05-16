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
            Info = info;
            Prefab = prefab;
            Building = Prefab == null ? null : Prefab.GetComponent<ILandformBuilding>();
        }

        public TerrainBuildingInfo Info { get; private set; }
        public GameObject Prefab { get; internal set; }
        public ILandformBuilding Building { get; internal set; }

        public bool IsLoadComplete
        {
            get { return Prefab != null && Building != null; }
        }

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
                TryRead(info, out resource);
                Result.Add(info.ID, resource);

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
            item = new BuildingResource(bInfo, prefab);

            if (item.IsLoadComplete)
            {
                return true;
            }
            else
            {
                Debug.LogWarning("无法读取[BuildingResourceReader],Info:" + info.ToString());
                return false;
            }
        }
    }
}
