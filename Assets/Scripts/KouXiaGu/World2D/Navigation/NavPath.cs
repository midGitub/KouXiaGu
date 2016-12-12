using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KouXiaGu.Grids;
using UnityEngine;

namespace KouXiaGu.World2D.Navigation
{

    /// <summary>
    /// 导航路径;将地图路径转换成导航路径提供单独实例使用;
    /// </summary>
    public class NavPath
    {
        public NavPath(LinkedList<RectCoord> wayPath,
            IHexMap<RectCoord, WorldNode> worldMap, 
            TopographiessData topographiessData)
        {
            this.wayPath = wayPath;
            this.worldMap = worldMap;
            this.topographiessData = topographiessData;

            Reset();
        }

        /// <summary>
        /// 行走的地图;
        /// </summary>
        IHexMap<RectCoord, WorldNode> worldMap;

        /// <summary>
        /// 地貌信息;
        /// </summary>
        TopographiessData topographiessData;

        /// <summary>
        /// 当前行走到;
        /// </summary>
        LinkedListNode<RectCoord> current;

        /// <summary>
        /// 起点;
        /// </summary>
        public Vector2 Starting
        {
            get { return MapPointToPlanePoint(wayPath.First.Value); }
        }

        /// <summary>
        /// 寻路的终点;
        /// </summary>
        public Vector2 Destination
        {
            get { return MapPointToPlanePoint(wayPath.Last.Value); }
        }

        /// <summary>
        /// 下一步行走到的点;
        /// </summary>
        public Vector2 Current
        {
            get { return MapPointToPlanePoint(current.Value); }
        }

        /// <summary>
        /// 路径点合集;
        /// </summary>
        LinkedList<RectCoord> wayPath;

        /// <summary>
        /// 获取到下一步行走到的点和行走的速度;
        /// 获取成功返回true,否则返回false;
        /// </summary>
        public bool TryNext(out Vector2 planePoint, out float percentage)
        {
            if (current == null)
            {
                planePoint = default(Vector2);
                percentage = default(float);
                return false;
            }

            RectCoord mapPoint = current.Value;
            planePoint = GetNextPoint();
            percentage = GetPercentageOfMovement(mapPoint);

            current = current.Next;
            return true;
        }

        /// <summary>
        /// 重设导航路径到起点;
        /// </summary>
        public void Reset()
        {
            this.current = wayPath.First;
        }

        /// <summary>
        /// 获取到将要行走到的下一个点;
        /// </summary>
        Vector2 GetNextPoint()
        {
            Vector2 currentPlanePoint = MapPointToPlanePoint(current.Value);

            if (current.Next != null)
            {
                Vector2 nextPlanePoint = MapPointToPlanePoint(current.Next.Value);
                return GetMidpoint(currentPlanePoint, nextPlanePoint);
            }
            else
            {
                return currentPlanePoint;
            }
        }

        /// <summary>
        /// 获取到这两个点的中点;
        /// </summary>
        Vector2 GetMidpoint(Vector2 point1, Vector2 point2)
        {
            Vector2 newPoint = (point1 + point2) / 2;
            return newPoint;
        }

        /// <summary>
        /// 若未获取到地图节点返回的默认速度;
        /// </summary>
        const float outMapRangePercentageOfMovement = 1;

        /// <summary>
        /// 获取到这个点的移动百分比;
        /// </summary>
        float GetPercentageOfMovement(RectCoord mapPoint)
        {
            WorldNode worldNode;
            if (worldMap.TryGetValue(mapPoint, out worldNode))
            {
                int topographyID = worldNode.TopographyID;
                TopographyInfo topographyInfo = topographiessData.GetInfoWithID(topographyID);
                float percentage = topographyInfo.PercentageOfMovement;
                return percentage;
            }
            Debug.LogWarning("未能获取到地图点" + mapPoint + "但是导航路径经过上面;");
            return 1;
        }

        /// <summary>
        /// 地图坐标转换成平面坐标;
        /// </summary>
        Vector2 MapPointToPlanePoint(RectCoord mapPoint)
        {
            return WorldConvert.MapToHex(mapPoint);
        }

    }

}
