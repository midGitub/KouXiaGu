// Generated code -- http://www.redblobgames.com/grids/hexagons/

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace KouXiaGu
{

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

        public static readonly Vector3 OriginPixelPoint = Vector3.zero;

        #region 坐标转换;

        /// <summary>
        /// 立方体坐标 转换为 偏移坐标;
        /// </summary>
        public static ShortVector2 CubeToOffset(CubeCoordinate cube)
        {
            int x = cube.q;
            int y = cube.r + (int)((cube.q + 1 * (cube.q & 1)) / 2);
            return new ShortVector2(x, y);
        }

        /// <summary>
        /// 偏移坐标 转换成 立方体坐标;
        /// </summary>
        public static CubeCoordinate OffsetToCube(ShortVector2 offset)
        {
            int q = offset.x;
            int r = offset.y - (int)((offset.x + 1 * (offset.x & 1)) / 2);
            int s = -q - r;
            return new CubeCoordinate(q, r, s);
        }

        /// <summary>
        /// 从像素坐标获取到所在的 偏移坐标;
        /// </summary>
        public static ShortVector2 Pixel2DToOffset(Vector2 point)
        {
            CubeCoordinate cube = Pixel2DToCube(point);
            return CubeToOffset(cube);
        }

        /// <summary>
        /// 3D像素坐标 转换到 偏移坐标;
        /// </summary>
        public static ShortVector2 PixelToOffset(Vector3 point)
        {
            CubeCoordinate cube = Pixel2DToCube(new Vector2(point.x, point.z));
            return CubeToOffset(cube);
        }

        /// <summary>
        /// 从像素坐标获取到所在的 立方体坐标;
        /// </summary>
        public static CubeCoordinate Pixel2DToCube(Vector2 point)
        {
            Vector2 pt = new Vector2((point.x - Origin.x) / OuterRadius, (point.y - Origin.y) / OuterRadius);
            float q = (float)(2.0 / 3.0 * pt.x);
            float r = (float)(-1.0 / 3.0 * pt.x + Math.Sqrt(3.0) / 3.0 * pt.y);
            return new CubeCoordinate(q, r, (-q - r));
        }

        public static CubeCoordinate PixelToCube(Vector3 point)
        {
            Vector2 v2 = new Vector2(point.x, point.z);
            return Pixel2DToCube(v2);
        }


        /// <summary>
        /// 立方体坐标 转换成 2D像素坐标;
        /// </summary>
        public static Vector2 CubeToPixel2D(CubeCoordinate cube)
        {
            float x = OuterRadius * 1.5f * cube.q;
            float y = (float)(Math.Sqrt(3.0) / 2.0 * cube.q + Math.Sqrt(3.0) * cube.r) * OuterRadius;
            return new Vector2(x, y);
        }

        /// <summary>
        /// 立方体坐标 转换成 3D像素坐标;
        /// </summary>
        public static Vector3 CubeToPixel(CubeCoordinate cube, float y = 0)
        {
            Vector2 v2 = CubeToPixel2D(cube);
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
        public static int ManhattanDistances(CubeCoordinate a, CubeCoordinate b)
        {
            CubeCoordinate hex = a - b;
            return (int)((Math.Abs(hex.q) + Math.Abs(hex.r) + Math.Abs(hex.s)) / 2);
        }

        /// <summary>
        /// 曼哈顿距离;
        /// </summary>
        public static int ManhattanDistances(ShortVector2 offset1, ShortVector2 offset2)
        {
            CubeCoordinate hex1 = OffsetToCube(offset1);
            CubeCoordinate hex2 = OffsetToCube(offset2);
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
        static Dictionary<int, CubeCoordinate> directions = GetDirections();

        /// <summary>
        /// 获取到方向偏移量;
        /// </summary>
        static Dictionary<int, CubeCoordinate> GetDirections()
        {
            Dictionary<int, CubeCoordinate> directions = new Dictionary<int, CubeCoordinate>(DirectionNumber);

            directions.Add((int)HexDirection.North, new CubeCoordinate(0, 1, -1));
            directions.Add((int)HexDirection.Northeast, new CubeCoordinate(1, 0, -1));
            directions.Add((int)HexDirection.Southeast, new CubeCoordinate(1, -1, 0));
            directions.Add((int)HexDirection.South, new CubeCoordinate(0, -1, 1));
            directions.Add((int)HexDirection.Southwest, new CubeCoordinate(-1, 0, 1));
            directions.Add((int)HexDirection.Northwest, new CubeCoordinate(-1, 1, 0));
            directions.Add((int)HexDirection.Self, new CubeCoordinate(0, 0, 0));

            return directions;
        }

        /// <summary>
        /// 获取到方向偏移量;
        /// </summary>
        public static CubeCoordinate CubeDirectionVector(HexDirection direction)
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
        /// 获取到这个点周围的坐标;从 HexDirection 高位标记开始返回;
        /// </summary>
        public static IEnumerable<ShortVector2> GetNeighboursPoints(ShortVector2 target)
        {
            foreach (var direction in HexDirections())
            {
                ShortVector2 point = OffSetDirectionVector(target, direction) + target;
                yield return point;
            }
        }

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
        /// 获取到这个点本身和周围的坐标;从 HexDirection 高位标记开始返回;
        /// </summary>
        public static IEnumerable<ShortVector2> GetNeighboursAndSelfPoints(ShortVector2 target)
        {
            foreach (var direction in HexDirectionsAndSelf())
            {
                ShortVector2 point = OffSetDirectionVector(target, direction) + target;
                yield return point;
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
        /// 获取到这个点这些方向的坐标;
        /// </summary>
        public static IEnumerable<ShortVector2> GetNeighboursPoints(ShortVector2 target, HexDirection directions)
        {
            foreach (var direction in HexDirections(directions))
            {
                ShortVector2 point = OffSetDirectionVector(target, direction) + target;
                yield return point;
            }
        }

        /// <summary>
        /// 获取到这个点这些方向的坐标和方向;
        /// </summary>
        public static IEnumerable<KeyValuePair<HexDirection, ShortVector2>> GetNeighbours(ShortVector2 target, HexDirection directions)
        {
            foreach (var direction in HexDirections(directions))
            {
                ShortVector2 point = OffSetDirectionVector(target, direction) + target;
                yield return new KeyValuePair<HexDirection, ShortVector2>(direction, point);
            }
        }

    }

    /// <summary>
    /// 六边形立方体坐标;
    /// </summary>
    public struct CubeCoordinate
    {

        public short q;
        public short r;
        public short s;

        public CubeCoordinate(short q, short r, short s)
        {
            this.q = q;
            this.r = r;
            this.s = s;
        }

        public CubeCoordinate(int q, int r, int s)
        {
            this.q = (short)q;
            this.r = (short)r;
            this.s = (short)s;
        }

        public CubeCoordinate(float q, float r, float s)
        {
            int intQ = (short)(Math.Round(q));
            int intR = (short)(Math.Round(r));
            int intS = (short)(Math.Round(s));
            double q_diff = Math.Abs(intQ - q);
            double r_diff = Math.Abs(intR - r);
            double s_diff = Math.Abs(intS - s);
            if (q_diff > r_diff && q_diff > s_diff)
            {
                intQ = -intR - intS;
            }
            else
            {
                if (r_diff > s_diff)
                {
                    intR = -intQ - intS;
                }
                else
                {
                    intS = -intQ - intR;
                }
            }
            this.q = (short)intQ;
            this.r = (short)intR;
            this.s = (short)intS;
        }

        public static CubeCoordinate operator +(CubeCoordinate a, CubeCoordinate b)
        {
            return new CubeCoordinate(a.q + b.q, a.r + b.r, a.s + b.s);
        }

        public static CubeCoordinate operator -(CubeCoordinate a, CubeCoordinate b)
        {
            return new CubeCoordinate(a.q - b.q, a.r - b.r, a.s - b.s);
        }

        public static CubeCoordinate operator *(CubeCoordinate a, int k)
        {
            return new CubeCoordinate(a.q * k, a.r * k, a.s * k);
        }

        public static CubeCoordinate operator /(CubeCoordinate a, int k)
        {
            return new CubeCoordinate(a.q / k, a.r / k, a.s / k);
        }

        public override string ToString()
        {
            return string.Concat("(", q, ",", r, ",", s, ")");
        }

    }

}