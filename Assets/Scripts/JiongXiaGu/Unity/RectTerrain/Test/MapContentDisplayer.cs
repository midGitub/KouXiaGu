using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using JiongXiaGu.Grids;
using JiongXiaGu.Unity.RectMaps;
using JiongXiaGu.Unity;
using System.Threading;
using UnityEngine.UI;

namespace JiongXiaGu.Unity.RectTerrain
{

    /// <summary>
    /// 将鼠标所指向地图节点的内容输出为文本;
    /// </summary>
    [DisallowMultipleComponent]
    class MapContentDisplayer : MonoBehaviour, IWorldCompletedHandle
    {

        [SerializeField]
        Text textObject;
        IDictionary<RectCoord, MapNode> map;

        void IWorldCompletedHandle.OnWorldCompleted()
        {
            map = RectMapController.Instance.WorldMap.Map;
        }

        void Update()
        {
            if (map != null)
            {
                textObject.text = TextUpdate();
            }
        }

        string TextUpdate()
        {
            Vector3 mousePoint;
            if (LandformRay.Instance.TryGetMouseRayPoint(out mousePoint))
            {
                RectCoord pos = mousePoint.ToRectTerrainRect();
                MapNode node;

                if (map.TryGetValue(pos, out node))
                {
                    NodeLandformInfo landform = node.Landform;
                    //RoadNode roadNode = node.Road;
                    //BuildingNode buildingNode = node.Building;
                    return
                        "坐标:" + pos.ToString()
                        + "\nLandform:" + landform.TypeID + ",Angle:" + landform.Angle;
                        //+ "\nRoad:" + roadNode.ID + ",Type:" + roadNode.RoadType + ",ExistRoad:" + roadNode.Exist()
                        //+ "\nBuilding:" + buildingNode.ID + ",Types:" + node.Building.BuildingType + ",ExistBuilding:" + node.Building.Exist()
                        //+ "\nTags:"
                        //+ "\nTown:" + node.Town.TownID;
                }
            }
            return "检测不到地形;";
        }
    }
}
