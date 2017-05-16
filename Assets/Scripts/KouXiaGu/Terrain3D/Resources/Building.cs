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

    public class Building : IDisposable
    {
        public Building(TerrainBuildingInfo info, GameObject prefab)
        {
            Info = info;
            Prefab = prefab;
        }

        public TerrainBuildingInfo Info { get; private set; }
        public GameObject Prefab { get; private set; }

        public bool IsLoadComplete
        {
            get { return Prefab != null; }
        }

        public void Dispose()
        {
            GameObject.Destroy(Prefab);
            Prefab = null;
        }
    }

    [Serializable]
    public class BuildingReader : AsyncOperation<Dictionary<int, Building>>
    {
        [SerializeField]
        Stopwatch stopwatch;
        AssetBundle assetBundle;

        public IEnumerator ReadAsync(AssetBundle assetBundle, IEnumerable<BuildingInfo> infos)
        {
            Result = new Dictionary<int, Building>();
            foreach (BuildingInfo info in infos)
            {
                Building item;
                TryRead(info.Terrain, out item);
                Result.Add(info.ID, item);

                if (stopwatch.Await())
                {
                    yield return null;
                    stopwatch.Restart();
                }
            }
            OnCompleted();
        }

        public bool TryRead(TerrainBuildingInfo info, out Building item)
        {
            GameObject prefab = assetBundle.LoadAsset<GameObject>(info.PrefabName);
            item = new Building(info, prefab);

            if (item.IsLoadComplete)
            {
                return true;
            }
            else
            {
                Debug.LogWarning("[地形建筑读取]未找到对应的预制物体;Name:" + info.PrefabName);
                return false;
            }
        }
    }

}
