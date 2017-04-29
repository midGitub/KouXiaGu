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

        void IObserver<IWorld>.OnCompleted()
        {
            throw new NotImplementedException();
        }

        void IObserver<IWorld>.OnError(Exception error)
        {
            throw new NotImplementedException();
        }

        void Update()
        {
            textObject.text = TextUpdate();

            Vector3 mousePoint;
            if (Input.GetKeyDown(KeyCode.Mouse0) && LandformRay.Instance.TryGetMouseRayPoint(out mousePoint))
            {
                CubicHexCoord coord = mousePoint.GetTerrainCubic();
                MapNode node = data[coord];
                node = node.CreateRoad(mapData, 1);
                data[coord] = node;
            }
            else if (Input.GetKeyDown(KeyCode.Mouse1) && LandformRay.Instance.TryGetMouseRayPoint(out mousePoint))
            {
                CubicHexCoord coord = mousePoint.GetTerrainCubic();
                MapNode node = data[coord];
                node = node.DestroyRoad();
                data[coord] = node;
            }
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
                        + "\nRoad:" + node.Road.Type + ",ExistRoad:" + node.ExistRoad()
                        + "\nBuilding:" + node.Building.BuildingID + ",ExistBuilding:" + true + ",Angle:" + node.Building.Angle
                        ;
                }
            }
            return "Empty Content";
        }

    }

}
