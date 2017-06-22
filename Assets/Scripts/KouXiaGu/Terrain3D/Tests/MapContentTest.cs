using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using KouXiaGu.Grids;
using KouXiaGu.World.Map;
using UniRx;
using KouXiaGu.World;

namespace KouXiaGu.Terrain3D.Tests
{

    /// <summary>
    /// 将鼠标所指向地图节点的内容输出为文本;
    /// </summary>
    [DisallowMultipleComponent]
    class MapContentTest : MonoBehaviour, IStateObserver<IWorldComplete>
    {

        [SerializeField]
        Text textObject = null;

        IWorld world;

        IDictionary<CubicHexCoord, MapNode> map
        {
            get { return world.WorldData.MapData.Map; }
        }

        MapData mapData
        {
            get { return world.WorldData.MapData.data; }
        }

        void Awake()
        {
            enabled = false;
            WorldSceneManager.OnWorldInitializeComplted.Subscribe(this);
        }

        void IStateObserver<IWorldComplete>.OnCompleted(IWorldComplete item)
        {
            world = item;
            enabled = true;
        }

        void IStateObserver<IWorldComplete>.OnFailed(Exception ex)
        {
            return;
        }

        void Update()
        {
            textObject.text = TextUpdate();

            Vector3 mousePoint;
            if (LandformRay.Instance.TryGetMouseRayPoint(out mousePoint))
            {
                if (Input.GetKey(KeyCode.LeftAlt) || Input.GetKey(KeyCode.RightAlt))
                {
                    if (Input.GetKeyDown(KeyCode.B))
                    {
                        DestroyBuildingAt(mousePoint);
                    }
                }
                else
                {
                    if (Input.GetKeyDown(KeyCode.Mouse0))
                    {
                        OnMouse0Down(mousePoint);
                    }
                    else if (Input.GetKeyDown(KeyCode.Mouse1))
                    {
                        OnMouse1Down(mousePoint);
                    }
                    else if (Input.GetKeyDown(KeyCode.B))
                    {
                        BuildBuildingAt(mousePoint);
                    }
                }
            }
        }

        void OnMouse0Down(Vector3 mousePoint)
        {
            CubicHexCoord coord = mousePoint.GetTerrainCubic();
            MapNode node = map[coord];
            node.Road = node.Road.Update(mapData, 1);
            map[coord] = node;
        }

        void OnMouse1Down(Vector3 mousePoint)
        {
            CubicHexCoord coord = mousePoint.GetTerrainCubic();
            MapNode node = map[coord];
            node.Road = node.Road.Destroy();
            map[coord] = node;
        }

        void BuildBuildingAt(Vector3 mousePoint)
        {
            CubicHexCoord coord = mousePoint.GetTerrainCubic();
            MapNode node = map[coord];
            node.Building = node.Building.Update(mapData, 1, 0);
            map[coord] = node;
        }

        void DestroyBuildingAt(Vector3 mousePoint)
        {
            CubicHexCoord coord = mousePoint.GetTerrainCubic();
            MapNode node = map[coord];
            node.Building = node.Building.Destroy();
            map[coord] = node;
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
            return "Empty Content";
        }
    }
}
