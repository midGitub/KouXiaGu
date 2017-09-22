using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using JiongXiaGu.Grids;
using JiongXiaGu.World.Map;
using JiongXiaGu.World;
using JiongXiaGu.Terrain3D;

namespace JiongXiaGu.Tests
{

    /// <summary>
    /// 将鼠标所指向地图节点的内容输出为文本;
    /// </summary>
    [DisallowMultipleComponent]
    class MapContent : MonoBehaviour
    {
        MapContent()
        {
        }

        [SerializeField]
        Text textObject = null;
        IWorld world;

        IDictionary<CubicHexCoord, MapNode> map
        {
            get { return world.WorldData.MapData.Map; }
        }

        void Update()
        {
            world = WorldSceneManager.World;
            if (world != null)
            {
                textObject.text = TextUpdate();
            }
        }

        string TextUpdate()
        {
            Vector3 mousePoint;
            if (LandformRay.Instance.TryGetMouseRayPoint(out mousePoint))
            {
                CubicHexCoord coord = mousePoint.GetTerrainCubic();
                MapNode node;

                if (map.TryGetValue(coord, out node))
                {
                    LandformNode landformNode = node.Landform;
                    RoadNode roadNode = node.Road;
                    BuildingNode buildingNode = node.Building;
                    return
                        "坐标:" + coord.ToString()
                        + "\nLandform:" + landformNode.ID + ",Type:" + landformNode.LandformType + ",Exist:" + landformNode.Exist()
                        + "\nRoad:" + roadNode.ID + ",Type:" + roadNode.RoadType + ",ExistRoad:" + roadNode.Exist()
                        + "\nBuilding:" + buildingNode.ID + ",Types:" + node.Building.BuildingType + ",ExistBuilding:" + node.Building.Exist()
                        + "\nTags:"
                        + "\nTown:" + node.Town.TownID;
                }
            }
            return "检测不到地形;";
        }

        //void OnMouse0Down(Vector3 mousePoint)
        //{
        //    CubicHexCoord coord = mousePoint.GetTerrainCubic();
        //    MapNode node = map[coord];
        //    node.Road = node.Road.Update(mapData, 1);
        //    map[coord] = node;
        //}

        //void OnMouse1Down(Vector3 mousePoint)
        //{
        //    CubicHexCoord coord = mousePoint.GetTerrainCubic();
        //    MapNode node = map[coord];
        //    node.Road = node.Road.Destroy();
        //    map[coord] = node;
        //}

        //void BuildBuildingAt(Vector3 mousePoint)
        //{
        //    CubicHexCoord coord = mousePoint.GetTerrainCubic();
        //    MapNode node = map[coord];
        //    node.Building = node.Building.Update(mapData, 1, 0);
        //    map[coord] = node;
        //}

        //void DestroyBuildingAt(Vector3 mousePoint)
        //{
        //    CubicHexCoord coord = mousePoint.GetTerrainCubic();
        //    MapNode node = map[coord];
        //    node.Building = node.Building.Destroy();
        //    map[coord] = node;
        //}
    }
}
