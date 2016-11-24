using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace KouXiaGu.World2D
{


    /// <summary>
    /// 对世界坐标和其它类型进行转换;
    /// </summary>
    public static partial class WorldConvert
    {

        #region 六边形定义

        /// <summary>
        /// 地图以外径为2的正六边形排列;
        /// </summary>
        private static readonly Hexagon hexagon = new Hexagon() { OuterDiameter = 4 };

        /// <summary>
        /// 地图所使用的六边形;
        /// </summary>
        public static Hexagon MapHexagon
        {
            get { return hexagon; }
        }

        #endregion


        #region 世界位置转换蜂窝地图位置;

        /// <summary>
        /// 将浮点类型向量转换成地图点;
        /// </summary>
        public static PointPair PlaneToHexPair(Vector2 planePoint)
        {
            int x1, x2, y1, y2;
            var points = new PointPair[4];

            GetInterval(planePoint.x, hexagon.DistanceX, out x1, out x2);
            GetInterval(planePoint.y, hexagon.InnerDiameter, out y1, out y2);

            points[0] = new PointPair(hexagon, x1, y1);
            points[1] = new PointPair(hexagon, x1, y2);
            points[2] = new PointPair(hexagon, x2, y1);
            points[3] = new PointPair(hexagon, x2, y2);

            PointPair pointPair = Nearest(planePoint, points);
            return pointPair;
        }

        /// <summary>
        /// 获取到这个数所在的整数区间;
        /// intPoint1 为靠近0的点, intPoint2 为远离0的点;
        /// </summary>
        static void GetInterval(float point, float spacing, out int intPoint1, out int intPoint2)
        {
            intPoint1 = (int)(point / spacing);
            if (point > 0)
            {
                intPoint2 = intPoint1++;
            }
            else if (point < 0)
            {
                intPoint2 = intPoint1--;
            }
            else
            {
                intPoint2 = intPoint1++;
            }
        }

        /// <summary>
        /// 获取 坐标集 内世界坐标离目标最短的 坐标集;
        /// </summary>
        static PointPair Nearest(Vector2 target, params PointPair[] points)
        {
            float minDistance = Vector2.Distance(target, points[0].HexPoint);
            PointPair minPointPair = points[0];

            for (int i = 1; i < points.Length; i++)
            {
                float distance = Vector2.Distance(target, points[i].HexPoint);
                if (distance < minDistance)
                {
                    minPointPair = points[i];
                    minDistance = distance;
                }
            }

            return minPointPair;
        }

        /// <summary>
        /// 将 六边形编号 转换成 六边形中心点;
        /// </summary>
        public static Vector2 MapToHex(ShortVector2 mapPoint)
        {
            Vector2 position = new Vector2();
            position.x = hexagon.DistanceX * mapPoint.x;
            position.y = hexagon.InnerDiameter * mapPoint.y;

            if ((mapPoint.x & 1) == 1)
                position.y -= (hexagon.InnerDiameter / 2);

            return position;
        }

        /// <summary>
        /// 平面坐标 转换成 六边形中心点(任意的中心点);
        /// </summary>
        public static Vector2 PlaneToHex(Vector2 planePoint)
        {
            float distanceX2 = (hexagon.DistanceX * 2);
            planePoint.x = ((int)(planePoint.x / distanceX2)) * distanceX2;
            planePoint.y = ((int)(planePoint.y / hexagon.InnerDiameter)) * hexagon.InnerDiameter;
            return planePoint;
        }

        public struct PointPair
        {
            public PointPair(Hexagon hexagon, int mapX, int mapY)
            {
                MapPoint = new ShortVector2(mapX, mapY);
                HexPoint = MapToHex(MapPoint);
            }

            public ShortVector2 MapPoint { get; private set; }
            public ShortVector2 ShortMapPoint { get { return (ShortVector2)MapPoint; } }
            public Vector2 HexPoint { get; private set; }

            public override string ToString()
            {
                string str = string.Concat("地图坐标:", MapPoint, "六边形坐标:", HexPoint);
                return str;
            }

            public static implicit operator ShortVector2(PointPair item)
            {
                return item.MapPoint;
            }
            public static implicit operator Vector2(PointPair item)
            {
                return item.HexPoint;
            }
        }

        #endregion


        #region 鼠标

        /// <summary>
        /// 定义在Unity内触发器所在的层(重要)!
        /// </summary>
        static readonly int SceneAssistMask = LayerMask.GetMask("SceneAssist");

        /// <summary>
        /// 射线最大距离;
        /// </summary>
        const float RayMaxDistance = 50;

        /// <summary>
        /// 获取视窗鼠标所在水平面上的坐标;
        /// </summary>
        public static Vector2 MouseToPlane(this Camera camera)
        {
            Ray ray = camera.ScreenPointToRay(Input.mousePosition);
            RaycastHit raycastHit;
            if (Physics.Raycast(ray, out raycastHit, RayMaxDistance, SceneAssistMask, QueryTriggerInteraction.Collide))
            {
                return raycastHit.point;
            }
            else
            {
                throw new Exception("坐标无法确定!检查摄像机之前地面是否存在3D碰撞模块!");
            }
        }

        /// <summary>
        /// 获取主摄像机视窗鼠标所在水平面上的坐标;
        /// </summary>
        public static Vector2 MouseToPlane()
        {
            return MouseToPlane(Camera.main);
        }

        #endregion

    }

}
