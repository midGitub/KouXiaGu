using System;
using System.Collections.Generic;
using UnityEngine;
using KouXiaGu.Grids;
using KouXiaGu.Collections;

namespace KouXiaGu.Terrain3D
{


    class BuildingCreater
    {

        static BuildingData data
        {
            get { return MapDataManager.ActiveData.Building; }
        }

        public BuildingCreater()
        {
            buildings = new CustomDictionary<CubicHexCoord, Building>();
        }

        /// <summary>
        /// 存在场景中的建筑物;
        /// </summary>
        CustomDictionary<CubicHexCoord, Building> buildings;


        /// <summary>
        /// 创建这个位置的建筑物,若已经存在 或者 节点不存在建筑物 则返回 false,;
        /// </summary>
        public bool Create(CubicHexCoord coord)
        {
            if (buildings.ContainsKey(coord))
                return false;

            GameObject buildingObject;
            if (TryInstantiate(coord, out buildingObject))
            {
                buildings.Add(coord, new Building(buildingObject));
                return true;
            }
            return false;
        }

        /// <summary>
        /// 创建这个位置的建筑物,或者更新其;
        /// </summary>
        public void CreateOrUpdate(CubicHexCoord coord)
        {
            Building building;
            if (buildings.TryGetValue(coord, out building))
            {
                GameObject buildingObject;
                if (TryInstantiate(coord, out buildingObject))
                {
                    building.SetBuildingObject(buildingObject);
                }
                else
                {
                    Remove(coord, building);
                }
            }
            else
            {
                GameObject buildingObject;
                if (TryInstantiate(coord, out buildingObject))
                {
                    buildings.Add(coord, new Building(buildingObject));
                }
            }
        }

        /// <summary>
        /// 移除这个位置的建筑物;
        /// </summary>
        public bool Remove(CubicHexCoord coord)
        {
            Building building;
            if (buildings.TryGetValue(coord, out building))
            {
                Remove(coord, building);
                return true;
            }
            return false;
        }

        void Remove(CubicHexCoord coord, Building building)
        {
            building.Dispose();
            buildings.Remove(coord);
        }

        /// <summary>
        /// 实例化该节点建筑;
        /// </summary>
        bool TryInstantiate(CubicHexCoord coord, out GameObject buildingObject)
        {
            BuildingNode node;
            if (data.TryGetValue(coord, out node))
            {
                GameObject prefab = GetBuildRes(node.ID).Prefab;
                Vector3 position = coord.GetTerrainPixel();
                float angle = node.Angle;

                buildingObject = GameObject.Instantiate(prefab, position, Quaternion.Euler(0, angle, 0));
                return true;
            }
            buildingObject = default(GameObject);
            return false;
        }

        /// <summary>
        /// 获取到建筑资源信息;
        /// </summary>
        BuildingRes GetBuildRes(int id)
        {
            try
            {
                return BuildingRes.initializedInstances[id];
            }
            catch (KeyNotFoundException ex)
            {
                throw new KeyNotFoundException("缺少建筑资源 :" + id, ex);
            }
        }


        /// <summary>
        /// 单个建筑信息;
        /// </summary>
        public class Building : IDisposable
        {
            public Building(GameObject buildingObject)
            {
                SetBuildingObject(buildingObject);
            }


            public GameObject BuildingObject { get; private set; }


            public void SetBuildingObject(GameObject buildingObject)
            {
                DestroyBuildingObject();
                this.BuildingObject = buildingObject;
                buildingObject.transform.position = GetHeight(buildingObject.transform.position);
            }

            Vector3 GetPixel(CubicHexCoord coord)
            {
                Vector3 pos = coord.GetTerrainPixel();
                return GetHeight(pos);
            }

            Vector3 GetHeight(Vector3 pos)
            {
                pos.y = TerrainData.GetHeight(pos);
                return pos;
            }

            void DestroyBuildingObject()
            {
                if (BuildingObject != null)
                {
                    GameObject.Destroy(BuildingObject);
                    BuildingObject = null;
                }
            }

            public void Dispose()
            {
                DestroyBuildingObject();
            }

        }

    }

}
