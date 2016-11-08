using System;
using System.Collections.Generic;
using UnityEngine;

namespace KouXiaGu.Map
{

    /// <summary>
    /// 六边形地图转换;
    /// </summary>
    public static class HexMapConvert
    {

        #region 世界位置转换蜂窝地图位置;

        public static PointPair GetClosePoint(this Hexagon hexagon, Vector2 point)
        {
            short x1, x2, y1, y2;
            var points = new PointPair[4];

            GetInterval(point.x, hexagon.DistanceX, out x1, out x2);
            GetInterval(point.y, hexagon.DistanceY, out y1, out y2);

            points[0] = new PointPair(hexagon, x1, y1);
            points[1] = new PointPair(hexagon, x1, y2);
            points[2] = new PointPair(hexagon, x2, y1);
            points[3] = new PointPair(hexagon, x2, y2);

            PointPair pointPair = Nearest(point, points);
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
        private static void GetInterval(float point, float spacing, out short intPoint1, out short intPoint2)
        {
            intPoint1 = (short)(point / spacing);
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
            float minDistance = Vector2.Distance(target, points[0].worldPoint);
            PointPair minPointPair = points[0];

            for (int i = 1; i < points.Length; i++)
            {
                float distance = Vector2.Distance(target, points[i].worldPoint);
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
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        private static Vector2 NumberToPosition(this Hexagon hexagon, IntVector2 mapPoint)
        {
            Vector2 position = new Vector2();
            position.x = hexagon.DistanceX * mapPoint.x;
            position.y = hexagon.DistanceY * mapPoint.y;

            if ((mapPoint.x & 1) == 1)
                position.y -= (hexagon.InnerDiameter / 2);

            return position;
        }

        public struct PointPair
        {
            public PointPair(Hexagon hexagon, short mapX, short mapY)
            {
                mapPoint = new IntVector2(mapX, mapY);
                worldPoint = NumberToPosition(hexagon, mapPoint);
            }

            public IntVector2 mapPoint { get; private set; }
            public Vector2 worldPoint { get; private set; }

            public override string ToString()
            {
                string str = string.Concat("地图坐标:", mapPoint, "  世界坐标:", worldPoint);
                return str;
            }
        }

        #endregion


        #region 方向向量获取\转换;

        private const int DirectionNumber = 6;

        private static readonly Dictionary<int, DirectionVector> DirectionVectorSet = GetDirectionVector();

        private static Dictionary<int, DirectionVector> GetDirectionVector()
        {
            var directionVectorSet = new Dictionary<int, DirectionVector>(DirectionNumber);

            directionVectorSet.Add(HexDirection.North, new IntVector2(0, 1), new IntVector2(0, 1));
            directionVectorSet.Add(HexDirection.Northeast, new IntVector2(1, 0), new IntVector2(1, 1));
            directionVectorSet.Add(HexDirection.Southeast, new IntVector2(1, -1), new IntVector2(1, 0));
            directionVectorSet.Add(HexDirection.South, new IntVector2(0, -1), new IntVector2(0, -1));
            directionVectorSet.Add(HexDirection.Southwest, new IntVector2(-1, -1), new IntVector2(-1, 0));
            directionVectorSet.Add(HexDirection.Northwest, new IntVector2(-1, 0), new IntVector2(-1, 1));

            return directionVectorSet;
        }

        private static void Add(this Dictionary<int, DirectionVector> directionVectorDictionary,
            HexDirection direction, IntVector2 oddVector, IntVector2 evenVector)
        {
            DirectionVector directionVector = new DirectionVector(direction, oddVector, evenVector);
            directionVectorDictionary.Add((int)direction, directionVector);
        }

        /// <summary>
        /// 获取到这个地图坐标这个方向需要偏移的量;
        /// </summary>
        /// <param name="target"></param>
        /// <param name="direction"></param>
        public static IntVector2 GetVector(IntVector2 target, HexDirection direction)
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
            public DirectionVector(HexDirection direction, IntVector2 oddVector, IntVector2 evenVector)
            {
                this.Direction = direction;
                this.OddVector = oddVector;
                this.EvenVector = evenVector;
            }

            public HexDirection Direction { get; private set; }
            public IntVector2 OddVector { get; private set; }
            public IntVector2 EvenVector { get; private set; }
        }

        #endregion

    }

}
