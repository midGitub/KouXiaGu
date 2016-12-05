using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace KouXiaGu
{

    /// <summary>
    /// 平顶六边形边对应的方向;
    /// </summary>
    [Flags]
    public enum HexDirection
    {
        North = 1,
        Northeast = 2,
        Southeast = 4,
        South = 8,
        Southwest = 16,
        Northwest = 32,
        Self = 64,
    }


    /// <summary>
    /// 游戏六边形网格拓展;
    /// 六边形为平顶的偶数垂直布局;
    /// </summary>
    public static partial class HexGrids
    {

        /// <summary>
        /// 六边形半径定义(单位像素);
        /// </summary>
        public const int OuterRadius = 1;

        /// <summary>
        /// 六边形起点;
        /// </summary>
        public static readonly ShortVector2 Origin = new ShortVector2(0, 0);
        /// <summary>
        /// 六边形的像素坐标起点;
        /// </summary>
        public static readonly Vector3 OriginPixelPoint = Vector3.zero;


        #region 坐标转换;

        /// <summary>
        /// 立方体坐标 转换为 偏移坐标;
        /// </summary>
        public static ShortVector2 HexToOffset(CubicHexCoord hex)
        {
            int x = hex.X;
            int y = hex.Y + (int)((hex.X + 1 * (hex.X & 1)) / 2);
            return new ShortVector2(x, y);
        }

        /// <summary>
        /// 偏移坐标 转换成 立方体坐标;
        /// </summary>
        public static CubicHexCoord OffsetToHex(ShortVector2 offset)
        {
            int x = offset.x;
            int y = offset.y - (int)((offset.x + 1 * (offset.x & 1)) / 2);
            int z = -x - y;
            return new CubicHexCoord(x, y, z);
        }


        /// <summary>
        /// 从像素坐标获取到所在的 偏移坐标;
        /// </summary>
        public static ShortVector2 Pixel2DToOffset(Vector2 point)
        {
            CubicHexCoord hex = Pixel2DToHex(point);
            return HexToOffset(hex);
        }

        /// <summary>
        /// 3D像素坐标 转换到 偏移坐标;
        /// </summary>
        public static ShortVector2 PixelToOffset(Vector3 point)
        {
            CubicHexCoord hex = Pixel2DToHex(new Vector2(point.x, point.z));
            return HexToOffset(hex);
        }

        /// <summary>
        /// 从像素坐标获取到所在的 立方体坐标;
        /// </summary>
        public static CubicHexCoord Pixel2DToHex(Vector2 point)
        {
            Vector2 pt = new Vector2((point.x - Origin.x) / OuterRadius, (point.y - Origin.y) / OuterRadius);
            float x = (float)(2.0 / 3.0 * pt.x);
            float y = (float)(-1.0 / 3.0 * pt.x + Math.Sqrt(3.0) / 3.0 * pt.y);
            return new CubicHexCoord(x, y, (-x - y));
        }

        /// <summary>
        /// 像素转换到立方体坐标;
        /// </summary>
        public static CubicHexCoord PixelToHex(Vector3 point)
        {
            Vector2 v2 = new Vector2(point.x, point.z);
            return Pixel2DToHex(v2);
        }

        /// <summary>
        /// 立方体坐标 转换成 2D像素坐标;
        /// </summary>
        public static Vector2 HexToPixel2D(CubicHexCoord hex)
        {
            float x = OuterRadius * 1.5f * hex.X;
            float y = (float)(Math.Sqrt(3.0) / 2.0 * hex.X + Math.Sqrt(3.0) * hex.Y) * OuterRadius;
            return new Vector2(x, y);
        }

        /// <summary>
        /// 立方体坐标 转换成 3D像素坐标;
        /// </summary>
        public static Vector3 HexToPixel(CubicHexCoord hex, float y = 0)
        {
            Vector2 v2 = HexToPixel2D(hex);
            return new Vector3(v2.x, y, v2.y);
        }

        /// <summary>
        /// 偏移坐标 转换成 像素坐标;
        /// </summary>
        public static Vector2 OffsetToPixel2D(ShortVector2 offset)
        {
            float x = (float)(OuterRadius * 1.5f * offset.x);
            float y = (float)(OuterRadius * Math.Sqrt(3) * (offset.y - 0.5 * (offset.x & 1)));
            return new Vector2(x, y);
        }

        /// <summary>
        /// 偏移坐标 转换成 3D的像素坐标;
        /// </summary>
        public static Vector3 OffsetToPixel(ShortVector2 offset, float y = 0)
        {
            Vector2 v2 = OffsetToPixel2D(offset);
            return new Vector3(v2.x, y, v2.y);
        }

        #endregion


        #region 距离

        /// <summary>
        /// 曼哈顿距离;
        /// </summary>
        public static int ManhattanDistances(this CubicHexCoord a, CubicHexCoord b)
        {
            CubicHexCoord hex = a - b;
            return (int)((Math.Abs(hex.X) + Math.Abs(hex.Y) + Math.Abs(hex.Z)) / 2);
        }

        /// <summary>
        /// 曼哈顿距离;
        /// </summary>
        public static int ManhattanDistances(ShortVector2 offset1, ShortVector2 offset2)
        {
            CubicHexCoord hex1 = OffsetToHex(offset1);
            CubicHexCoord hex2 = OffsetToHex(offset2);
            return ManhattanDistances(hex1, hex2);
        }

        #endregion


        #region 方向;

        #region 立方体坐标方向;

        /// <summary>
        /// 存在方向数;
        /// </summary>
        public const int DirectionNumber = 7;

        /// <summary>
        /// 方向偏移量;
        /// </summary>
        static Dictionary<int, CubicHexCoord> directions = GetDirections();

        /// <summary>
        /// 获取到方向偏移量;
        /// </summary>
        static Dictionary<int, CubicHexCoord> GetDirections()
        {
            Dictionary<int, CubicHexCoord> directions = new Dictionary<int, CubicHexCoord>(DirectionNumber);

            directions.Add((int)HexDirection.North, new CubicHexCoord(0, 1, -1));
            directions.Add((int)HexDirection.Northeast, new CubicHexCoord(1, 0, -1));
            directions.Add((int)HexDirection.Southeast, new CubicHexCoord(1, -1, 0));
            directions.Add((int)HexDirection.South, new CubicHexCoord(0, -1, 1));
            directions.Add((int)HexDirection.Southwest, new CubicHexCoord(-1, 0, 1));
            directions.Add((int)HexDirection.Northwest, new CubicHexCoord(-1, 1, 0));
            directions.Add((int)HexDirection.Self, new CubicHexCoord(0, 0, 0));

            return directions;
        }

        /// <summary>
        /// 获取到方向偏移量;
        /// </summary>
        public static CubicHexCoord HexDirectionVector(HexDirection direction)
        {
            return directions[(int)direction];
        }

        #endregion


        #region 偏移坐标方向;

        /// <summary>
        /// 方向转换偏移量合集;
        /// </summary>
        static readonly Dictionary<int, DirectionVector> DirectionVectorSet = GetDirectionVector();

        static Dictionary<int, DirectionVector> GetDirectionVector()
        {
            var directionVectorSet = new Dictionary<int, DirectionVector>(DirectionNumber);

            AddIn(directionVectorSet, HexDirection.North, new ShortVector2(0, 1), new ShortVector2(0, 1));
            AddIn(directionVectorSet, HexDirection.Northeast, new ShortVector2(1, 0), new ShortVector2(1, 1));
            AddIn(directionVectorSet, HexDirection.Southeast, new ShortVector2(1, -1), new ShortVector2(1, 0));
            AddIn(directionVectorSet, HexDirection.South, new ShortVector2(0, -1), new ShortVector2(0, -1));
            AddIn(directionVectorSet, HexDirection.Southwest, new ShortVector2(-1, -1), new ShortVector2(-1, 0));
            AddIn(directionVectorSet, HexDirection.Northwest, new ShortVector2(-1, 0), new ShortVector2(-1, 1));
            AddIn(directionVectorSet, HexDirection.Self, new ShortVector2(0, 0), new ShortVector2(0, 0));

            return directionVectorSet;
        }

        static void AddIn(Dictionary<int, DirectionVector> directionVectorDictionary,
            HexDirection direction, ShortVector2 oddVector, ShortVector2 evenVector)
        {
            DirectionVector directionVector = new DirectionVector(direction, oddVector, evenVector);
            directionVectorDictionary.Add((int)direction, directionVector);
        }

        /// <summary>
        /// 获取到这个地图坐标这个方向需要偏移的量(不进行相加,仅是偏移量);
        /// </summary>
        public static ShortVector2 OffSetDirectionVector(ShortVector2 target, HexDirection direction)
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
        struct DirectionVector
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

        #endregion


        #region 经过排序的方向;

        const int maxDirectionMark = (int)HexDirection.Self;
        const int minDirectionMark = (int)HexDirection.North;

        /// <summary>
        /// 按标记为从 高位到低位 循序排列的数组;不包含本身
        /// </summary>
        static readonly HexDirection[] HexDirectionsArray = new HexDirection[]
        {
            HexDirection.Northwest,
            HexDirection.Southwest,
            HexDirection.South,
            HexDirection.Southeast,
            HexDirection.Northeast,
            HexDirection.North,
        };

        /// <summary>
        /// 按标记为从 高位到低位 循序排列的数组;
        /// </summary>
        static readonly HexDirection[] HexDirectionsAndSelfArray = Enum.GetValues(typeof(HexDirection)).
            Cast<HexDirection>().Reverse().ToArray();

        /// <summary>
        /// 按标记为从 高位到低位 循序返回的迭代结构;不包含本身
        /// </summary>
        public static IEnumerable<HexDirection> HexDirections()
        {
            return HexDirectionsArray;
        }

        /// <summary>
        /// 获取到从 高位到低位 顺序返回的迭代结构;包括本身;
        /// </summary>
        public static IEnumerable<HexDirection> HexDirectionsAndSelf()
        {
            return HexDirectionsAndSelfArray;
        }

        /// <summary>
        /// 获取到方向集表示的所有方向;
        /// </summary>
        public static IEnumerable<HexDirection> HexDirections(HexDirection directions)
        {
            int mask = (int)directions;
            for (int intDirection = minDirectionMark; intDirection <= maxDirectionMark; intDirection <<= 1)
            {
                if ((intDirection & mask) == 1)
                {
                    yield return (HexDirection)intDirection;
                }
            }
        }

        #endregion


        /// <summary>
        /// 获取到这个点周围的方向和坐标;从 HexDirection 高位标记开始返回;
        /// </summary>
        public static IEnumerable<KeyValuePair<HexDirection, ShortVector2>> GetNeighbours(ShortVector2 target)
        {
            foreach (var direction in HexDirections())
            {
                ShortVector2 point = OffSetDirectionVector(target, direction) + target;
                yield return new KeyValuePair<HexDirection, ShortVector2>(direction, point);
            }
        }

        /// <summary>
        /// 获取到这个点本身和周围的方向和坐标;从 HexDirection 高位标记开始返回;
        /// </summary>
        public static IEnumerable<KeyValuePair<HexDirection, ShortVector2>> GetNeighboursAndSelf(ShortVector2 target)
        {
            foreach (var direction in HexDirectionsAndSelf())
            {
                ShortVector2 point = OffSetDirectionVector(target, direction) + target;
                yield return new KeyValuePair<HexDirection, ShortVector2>(direction, point);
            }
        }

        /// <summary>
        /// 获取到这个点这些方向的坐标和方向,从 HexDirection 高位标记开始返回;
        /// </summary>
        public static IEnumerable<KeyValuePair<HexDirection, ShortVector2>> GetNeighbours(ShortVector2 target, HexDirection directions)
        {
            foreach (var direction in HexDirections(directions))
            {
                ShortVector2 point = OffSetDirectionVector(target, direction) + target;
                yield return new KeyValuePair<HexDirection, ShortVector2>(direction, point);
            }
        }



        /// <summary>
        /// 获取到这个点周围的方向和坐标;从 HexDirection 高位标记开始返回;
        /// </summary>
        public static IEnumerable<KeyValuePair<HexDirection, CubicHexCoord>> GetNeighbours(CubicHexCoord target)
        {
            foreach (var direction in HexDirections())
            {
                CubicHexCoord point = HexDirectionVector(direction) + target;
                yield return new KeyValuePair<HexDirection, CubicHexCoord>(direction, point);
            }
        }

        /// <summary>
        /// 获取到这个点本身和周围的方向和坐标;从 HexDirection 高位标记开始返回;
        /// </summary>
        public static IEnumerable<KeyValuePair<HexDirection, CubicHexCoord>> GetNeighboursAndSelf(CubicHexCoord target)
        {
            foreach (var direction in HexDirectionsAndSelf())
            {
                CubicHexCoord point = HexDirectionVector(direction) + target;
                yield return new KeyValuePair<HexDirection, CubicHexCoord>(direction, point);
            }
        }

        /// <summary>
        /// 获取到这个点这些方向的坐标和方向,从 HexDirection 高位标记开始返回;
        /// </summary>
        public static IEnumerable<KeyValuePair<HexDirection, CubicHexCoord>> GetNeighbours(CubicHexCoord target, HexDirection directions)
        {
            foreach (var direction in HexDirections(directions))
            {
                CubicHexCoord point = HexDirectionVector(direction) + target;
                yield return new KeyValuePair<HexDirection, CubicHexCoord>(direction, point);
            }
        }

    }

}