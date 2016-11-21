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
    [DisallowMultipleComponent]
    public static class WorldConvert
    {

        #region 地图

        /// <summary>
        /// 地图以外径为2的正六边形排列;
        /// </summary>
        private static readonly Hexagon hexagon = new Hexagon() { OuterDiameter = 2 };

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
        /// <param name="point"></param>
        /// <param name="spacing"></param>
        /// <param name="intPoint1"></param>
        /// <param name="intPoint2"></param>
        private static void GetInterval(float point, float spacing, out int intPoint1, out int intPoint2)
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
        /// <param name="target"></param>
        /// <param name="points"></param>
        /// <returns></returns>
        private static PointPair Nearest(Vector2 target, params PointPair[] points)
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
        public static Vector2 MapToHex(IntVector2 mapPoint)
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
                MapPoint = new IntVector2(mapX, mapY);
                HexPoint = MapToHex(MapPoint);
            }

            public IntVector2 MapPoint { get; private set; }
            public ShortVector2 ShortMapPoint { get { return (ShortVector2)MapPoint; } }
            public Vector2 HexPoint { get; private set; }

            public override string ToString()
            {
                string str = string.Concat("地图坐标:", MapPoint, "六边形坐标:", HexPoint);
                return str;
            }

            public static implicit operator IntVector2(PointPair item)
            {
                return item.MapPoint;
            }
            public static implicit operator Vector2(PointPair item)
            {
                return item.HexPoint;
            }
        }

        #endregion


        #region 方向向量获取\转换;

        private const int DirectionNumber = 6;

        private static readonly Dictionary<int, DirectionVector> DirectionVectorSet = GetDirectionVector();

        private static Dictionary<int, DirectionVector> GetDirectionVector()
        {
            var directionVectorSet = new Dictionary<int, DirectionVector>(DirectionNumber);

            directionVectorSet.Add(HexDirection.North, new ShortVector2(0, 1), new ShortVector2(0, 1));
            directionVectorSet.Add(HexDirection.Northeast, new ShortVector2(1, 0), new ShortVector2(1, 1));
            directionVectorSet.Add(HexDirection.Southeast, new ShortVector2(1, -1), new ShortVector2(1, 0));
            directionVectorSet.Add(HexDirection.South, new ShortVector2(0, -1), new ShortVector2(0, -1));
            directionVectorSet.Add(HexDirection.Southwest, new ShortVector2(-1, -1), new ShortVector2(-1, 0));
            directionVectorSet.Add(HexDirection.Northwest, new ShortVector2(-1, 0), new ShortVector2(-1, 1));

            return directionVectorSet;
        }

        private static void Add(this Dictionary<int, DirectionVector> directionVectorDictionary,
            HexDirection direction, ShortVector2 oddVector, ShortVector2 evenVector)
        {
            DirectionVector directionVector = new DirectionVector(direction, oddVector, evenVector);
            directionVectorDictionary.Add((int)direction, directionVector);
        }

        /// <summary>
        /// 获取到这个地图坐标这个方向需要偏移的量;
        /// </summary>
        /// <param name="target"></param>
        /// <param name="direction"></param>
        public static ShortVector2 GetVector(ShortVector2 target, HexDirection direction)
        {
            DirectionVector directionVector = DirectionVectorSet[(int)direction];
            if ((target.x & 1) == 1)
            {
                return directionVector.OddVector;
            }
            else
            {
                return directionVector.EvenVector;
            }
        }

        /// <summary>
        /// 六边形 x轴奇数位和偶数位 对应方向的偏移向量;
        /// </summary>
        private struct DirectionVector
        {
            public DirectionVector(HexDirection direction, ShortVector2 oddVector, ShortVector2 evenVector)
            {
                this.Direction = direction;
                this.OddVector = oddVector;
                this.EvenVector = evenVector;
            }

            public HexDirection Direction { get; private set; }
            public ShortVector2 OddVector { get; private set; }
            public ShortVector2 EvenVector { get; private set; }
        }

        #endregion


        #region 鼠标

        private static readonly int SceneAssistMask = LayerMask.GetMask("SceneAssist");

        private const float RayMaxDistance = 50;

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
