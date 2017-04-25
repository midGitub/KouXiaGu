//using System.Collections.Generic;
//using UnityEngine;
//using System.Linq;
//using KouXiaGu.Grids;
//using KouXiaGu.Collections;

//namespace KouXiaGu.Terrain3D
//{

//    /// <summary>
//    /// 建筑创建,仅创建"非联动创建的建筑",就是不包括城墙啦;
//    /// </summary>
//    public class BuildingCreater 
//    {

//        public BuildingCreater(BuildingData data)
//        {
//            buildings = new CustomDictionary<CubicHexCoord, GameObject>();
//            this.Data = data;
//        }


//        CustomDictionary<CubicHexCoord, GameObject> buildings;
//        public BuildingData Data { get; private set; }


//        /// <summary>
//        /// 场景存在实例的建筑信息;
//        /// </summary>
//        public IEnumerable<KeyValuePair<CubicHexCoord, GameObject>> InstancedBuildings
//        {
//            get { return buildings.Where(pair => pair.Value != null); }
//        }

//        /// <summary>
//        /// 已经请求过实例化到场景的建筑信息(有些节点建筑信息为NULL);
//        /// </summary>
//        public IReadOnlyDictionary<CubicHexCoord, GameObject> Buildings
//        {
//            get { return buildings; }
//        }


//        /// <summary>
//        /// 创建建筑到节点,若已经实例化则返回false;
//        /// </summary>
//        public bool Create(CubicHexCoord coord)
//        {
//            if (buildings.ContainsKey(coord))
//                return false;

//            GameObject buildingObject = Instantiate(coord);
//            buildings.Add(coord, buildingObject);
//            return true;
//        }

//        /// <summary>
//        /// 创建建筑到节点,若已经创建则更新其;
//        /// </summary>
//        public void CreateOrUpdate(CubicHexCoord coord)
//        {
//            GameObject buildingObject;
//            if (buildings.TryGetValue(coord, out buildingObject))
//            {
//                GameObject.Destroy(buildingObject);
//                buildings[coord] = Instantiate(coord);
//            }
//            else
//            {
//                buildingObject = Instantiate(coord);
//                buildings.Add(coord, buildingObject);
//            }
//        }

//        /// <summary>
//        /// 销毁这个位置的建筑,若不存在则返回 false;
//        /// </summary>
//        public bool Destroy(CubicHexCoord coord)
//        {
//            GameObject buildingObject;
//            if (buildings.TryGetValue(coord, out buildingObject))
//            {
//                if(buildingObject != null)
//                    GameObject.Destroy(buildingObject);

//                buildings.Remove(coord);
//                return true;
//            }
//            return false;
//        }

//        /// <summary>
//        /// 实例化该节点建筑,若不存在则返回 null;
//        /// </summary>
//        GameObject Instantiate(CubicHexCoord coord)
//        {
//            BuildingNode node;
//            if (Data.TryGetValue(coord, out node))
//            {
//                GameObject prefab = GetBuildRes(node.ID).Prefab;
//                Vector3 position = GetPixel(coord);
//                float angle = node.Angle;

//                return GameObject.Instantiate(prefab, position, Quaternion.Euler(0, angle, 0));
//            }
//            return null;
//        }

//        Vector3 GetPixel(CubicHexCoord coord)
//        {
//            Vector3 pos = coord.GetTerrainPixel();
//            return GetHeight(pos);
//        }

//        Vector3 GetHeight(Vector3 pos)
//        {
//            pos.y = TerrainData.GetHeight(pos);
//            return pos;
//        }

//        /// <summary>
//        /// 获取到建筑资源信息;
//        /// </summary>
//        BuildingRes GetBuildRes(int id)
//        {
//            try
//            {
//                return BuildingRes.initializedInstances[id];
//            }
//            catch (KeyNotFoundException ex)
//            {
//                throw new KeyNotFoundException("缺少建筑资源 :" + id, ex);
//            }
//        }

//        /// <summary>
//        /// 重设物体高度;
//        /// </summary>
//        public bool ResetHeight(CubicHexCoord coord)
//        {
//            GameObject buildingObject;
//            if (buildings.TryGetValue(coord, out buildingObject))
//            {
//                ResetHeight(buildingObject);
//                return true;
//            }
//            return false;
//        }

//        /// <summary>
//        /// 重设物体高度;
//        /// </summary>
//        void ResetHeight(GameObject buildingObject)
//        {
//            buildingObject.transform.position = GetHeight(buildingObject.transform.position);
//        }

//        /// <summary>
//        /// 清空所有建筑信息;
//        /// </summary>
//        public void Clear()
//        {
//            buildings.Clear();
//        }

//    }

//}
