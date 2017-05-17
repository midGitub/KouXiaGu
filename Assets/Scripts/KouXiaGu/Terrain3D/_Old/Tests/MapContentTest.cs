﻿using System;
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
    class MapContentTest : MonoBehaviour, IObserver<IWorld>
    {

        [SerializeField]
        Text textObject = null;

        IWorld world;

        IDictionary<CubicHexCoord, MapNode> data
        {
            get { return world.Data.Map.Data; }
        }

        PredefinedMap mapData
        {
            get { return world.Data.Map.PredefinedMap; }
        }

        void Awake()
        {
            enabled = false;
            SceneObject.GetObject<WorldInitializer>().Subscribe(this);
        }

        void IObserver<IWorld>.OnNext(IWorld value)
        {
            world = value;
            enabled = true;
        }

        void IObserver<IWorld>.OnCompleted() { }
        void IObserver<IWorld>.OnError(Exception error) { }

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
            MapNode node = data[coord];
            node = node.CreateRoad(mapData, 1);
            data[coord] = node;
        }

        void OnMouse1Down(Vector3 mousePoint)
        {
            CubicHexCoord coord = mousePoint.GetTerrainCubic();
            MapNode node = data[coord];
            node = node.DestroyRoad();
            data[coord] = node;
        }

        string TextUpdate()
        {
            Vector3 mousePoint;
            if (LandformRay.Instance.TryGetMouseRayPoint(out mousePoint))
            {
                CubicHexCoord coord = mousePoint.GetTerrainCubic();
                MapNode node;

                if (data.TryGetValue(coord, out node))
                {
                    return
                        "坐标:" + coord.ToString()
                        + "\nLandform:" + node.Landform.LandformID
                        + "\nRoad:" + node.Road.Type + ",ID:" + node.Road.ID + ",ExistRoad:" + node.ExistRoad()
                        + "\nBuilding:" + node.Building.Type + ",ExistBuilding:" + node.Building.Exist() + ",Angle:" + node.Building.Angle
                        ;
                }
            }
            return "Empty Content";
        }

    }

}
