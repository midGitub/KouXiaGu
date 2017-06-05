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
            get { return world.WorldData.MapData.Data; }
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
            if (Input.GetKeyDown(KeyCode.Mouse0) && LandformRay.Instance.TryGetMouseRayPoint(out mousePoint))
            {
                OnMouse0Down(mousePoint);
            }
            else if (Input.GetKeyDown(KeyCode.Mouse1) && LandformRay.Instance.TryGetMouseRayPoint(out mousePoint))
            {
                OnMouse1Down(mousePoint);
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

        string TextUpdate()
        {
            Vector3 mousePoint;
            if (LandformRay.Instance.TryGetMouseRayPoint(out mousePoint))
            {
                CubicHexCoord coord = mousePoint.GetTerrainCubic();
                MapNode node;

                if (map.TryGetValue(coord, out node))
                {
                    return
                        "坐标:" + coord.ToString()
                        + "\nLandform:" + node.Landform.LandformType
                        + "\nRoad:" + node.Road.RoadType + ",ID:" + node.Road.ID + ",ExistRoad:" + node.Road.Exist()
                        + "\nBuilding:" + node.Building.Items.ToLog(string.Empty) + ",ExistBuilding:" + node.Building.Exist()
                        + "\nTags:"
                        ;
                }
            }
            return "Empty Content";
        }
    }
}
