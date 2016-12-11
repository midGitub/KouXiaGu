using System;
using System.Collections.Generic;
using System.Linq;
using ProtoBuf;
using UnityEngine;

namespace KouXiaGu.Grids
{

    /// <summary>
    /// 平顶六边形边对应的方向;
    /// </summary>
    [Flags]
    public enum HexDirections
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
    /// 平顶六边形对应的对角线方向;
    /// </summary>
    [Flags]
    public enum HexDiagonals
    {
        Northeast = 1,
        East = 2,
        Southeast = 4,
        Southwest = 8,
        West = 16,
        Northwest = 32,
        Self = 64,
    }

    /// <summary>
    /// 六边形立方体坐标;
    /// </summary>
    [ProtoContract]
    public struct CubicHexCoord : IGridPoint, IGridPoint<CubicHexCoord, HexDirections>
    {

        [ProtoMember(1)]
        public short X { get; private set; }
        [ProtoMember(2)]
        public short Y { get; private set; }
        public short Z { get; private set; }

        /// <summary>
        /// (0,0,0)
        /// </summary>
        public static readonly CubicHexCoord Zero = new CubicHexCoord(0, 0, 0);

        //(平顶)存在的方向偏移量;
        public static readonly CubicHexCoord DIR_North = new CubicHexCoord(0, 1, -1);
        public static readonly CubicHexCoord DIR_Northeast = new CubicHexCoord(1, 0, -1);
        public static readonly CubicHexCoord DIR_Southeast = new CubicHexCoord(1, -1, 0);
        public static readonly CubicHexCoord DIR_South = new CubicHexCoord(0, -1, 1);
        public static readonly CubicHexCoord DIR_Southwest = new CubicHexCoord(-1, 0, 1);
        public static readonly CubicHexCoord DIR_Northwest = new CubicHexCoord(-1, 1, 0);

        //(平顶)存在的对角线方向偏移量;
        public static readonly CubicHexCoord DIA_Northeast = new CubicHexCoord(1, 1, -2);
        public static readonly CubicHexCoord DIA_East = new CubicHexCoord(2, -1, -1);
        public static readonly CubicHexCoord DIA_Southeast = new CubicHexCoord(1, -2, 1);
        public static readonly CubicHexCoord DIA_Southwest = new CubicHexCoord(-1, -1, 2);
        public static readonly CubicHexCoord DIA_West = new CubicHexCoord(-2, 1, 1);
        public static readonly CubicHexCoord DIA_Northwest = new CubicHexCoord(-1, 2, -1);


        public CubicHexCoord(short x, short y, short z)
        {
            OutOfRangeException(x, y, z);

            this.X = x;
            this.Y = y;
            this.Z = z;
        }

        public CubicHexCoord(short x, short y)
        {
            this.X = x;
            this.Y = y;
            this.Z = (short)(-x - y);
        }

        public CubicHexCoord(int x, int y, int z)
        {
            OutOfRangeException((short)x, (short)y, (short)z);
            this.X = (short)x;
            this.Y = (short)y;
            this.Z = (short)z;
        }

        public CubicHexCoord(float x, float y, float z)
        {
            int intQ = (short)(Math.Round(x));
            int intR = (short)(Math.Round(y));
            int intS = (short)(Math.Round(z));
            double q_diff = Math.Abs(intQ - x);
            double r_diff = Math.Abs(intR - y);
            double s_diff = Math.Abs(intS - z);
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

            OutOfRangeException((short)intQ, (short)intR, (short)intS);

            this.X = (short)intQ;
            this.Y = (short)intR;
            this.Z = (short)intS;
        }

        /// <summary>
        /// 定义的值是否超出定义范围?
        /// </summary>
        public static bool IsOutOfRange(short x, short y, short z)
        {
            if ((x + y + z) != 0)
                return false;
            else
                return true;
        }

        static void OutOfRangeException(short x, short y, short z)
        {
            if ((x + y + z) != 0)
                throw new ArgumentOutOfRangeException("坐标必须满足 (x + y + z) == 0");
        }

        /// <summary>
        /// 在反序列化后调用;
        /// </summary>
        [ProtoAfterDeserialization]
        void ProtoAfterDeserialization()
        {
            this.Z = (short)(-this.X - this.Y);
        }

        /// <summary>
        /// 曼哈顿距离;
        /// </summary>
        public static int ManhattanDistances(CubicHexCoord a, CubicHexCoord b)
        {
            CubicHexCoord hex = a - b;
            return (int)((Math.Abs(hex.X) + Math.Abs(hex.Y) + Math.Abs(hex.Z)) / 2);
        }


        #region 方向;

        #region 立方体坐标方向;

        /// <summary>
        /// 存在方向数;
        /// </summary>
        public const int DirectionsNumber = 7;

        /// <summary>
        /// 方向偏移量;
        /// </summary>
        static Dictionary<int, CubicHexCoord> directions = HexDirectionDictionary();

        /// <summary>
        /// 获取到方向偏移量;
        /// </summary>
        static Dictionary<int, CubicHexCoord> HexDirectionDictionary()
        {
            Dictionary<int, CubicHexCoord> directions = new Dictionary<int, CubicHexCoord>(DirectionsNumber);

            directions.Add((int)HexDirections.North, CubicHexCoord.DIR_North);
            directions.Add((int)HexDirections.Northeast, CubicHexCoord.DIR_Northeast);
            directions.Add((int)HexDirections.Southeast, CubicHexCoord.DIR_Southeast);
            directions.Add((int)HexDirections.South, CubicHexCoord.DIR_South);
            directions.Add((int)HexDirections.Southwest, CubicHexCoord.DIR_Southwest);
            directions.Add((int)HexDirections.Northwest, CubicHexCoord.DIR_Northwest);
            directions.Add((int)HexDirections.Self, CubicHexCoord.Zero);

            return directions;
        }

        /// <summary>
        /// 获取到方向偏移量;
        /// </summary>
        public static CubicHexCoord GetDirectionOffset(HexDirections direction)
        {
            return directions[(int)direction];
        }

        /// <summary>
        /// 获取到这个方向的坐标;
        /// </summary>
        public CubicHexCoord GetDirection(HexDirections direction)
        {
            return this + GetDirectionOffset(direction);
        }

        #endregion

        #region 立方体坐标对角线方向;

        /// <summary>
        /// 存在的对角线方向数目;
        /// </summary>
        public const int DiagonalsNumber = 7;

        /// <summary>
        /// 对角线偏移量;
        /// </summary>
        static Dictionary<int, CubicHexCoord> diagonals = HexDiagonalDictionary();

        /// <summary>
        /// 获取到对角线偏移量;
        /// </summary>
        static Dictionary<int, CubicHexCoord> HexDiagonalDictionary()
        {
            Dictionary<int, CubicHexCoord> diagonals = new Dictionary<int, CubicHexCoord>(DiagonalsNumber);

            diagonals.Add((int)HexDiagonals.Northeast, CubicHexCoord.DIA_Northeast);
            diagonals.Add((int)HexDiagonals.East, CubicHexCoord.DIA_East);
            diagonals.Add((int)HexDiagonals.Southeast, CubicHexCoord.DIA_Southeast);
            diagonals.Add((int)HexDiagonals.Southwest, CubicHexCoord.DIA_Southwest);
            diagonals.Add((int)HexDiagonals.West, CubicHexCoord.DIA_West);
            diagonals.Add((int)HexDiagonals.Northwest, CubicHexCoord.DIA_Northwest);
            diagonals.Add((int)HexDiagonals.Self, CubicHexCoord.Zero);

            return diagonals;
        }

        /// <summary>
        /// 获取到对角线偏移量;
        /// </summary>
        public static CubicHexCoord GetDiagonal(HexDiagonals diagonal)
        {
            return diagonals[(int)diagonal];
        }

        /// <summary>
        /// 获取到这个对角线的偏移量;
        /// </summary>
        public static CubicHexCoord GetDiagonal(CubicHexCoord coord, HexDiagonals diagonal)
        {
            return coord + GetDiagonal(diagonal);
        }

        #endregion

        #endregion


        #region 经过排序的方向;

        const int maxDirectionMark = (int)HexDirections.Self;
        const int minDirectionMark = (int)HexDirections.North;

        /// <summary>
        /// 按标记为从 高位到低位 循序排列的数组;不包含本身
        /// </summary>
        static readonly HexDirections[] DirectionsArray = new HexDirections[]
        {
            HexDirections.Northwest,
            HexDirections.Southwest,
            HexDirections.South,
            HexDirections.Southeast,
            HexDirections.Northeast,
            HexDirections.North,
        };

        /// <summary>
        /// 按标记为从 高位到低位 循序排列的数组;
        /// </summary>
        static readonly HexDirections[] DirectionsAndSelfArray = Enum.GetValues(typeof(HexDirections)).
            Cast<HexDirections>().Reverse().ToArray();

        /// <summary>
        /// 按标记为从 高位到低位 循序返回的迭代结构;不包含本身
        /// </summary>
        public static IEnumerable<HexDirections> Directions
        {
           get { return DirectionsArray; }
        }

        /// <summary>
        /// 获取到从 高位到低位 顺序返回的迭代结构;包括本身;
        /// </summary>
        public static IEnumerable<HexDirections> DirectionsAndSelf
        {
            get { return DirectionsAndSelfArray; }
        }

        /// <summary>
        /// 获取到方向集表示的所有方向;
        /// </summary>
        public static IEnumerable<HexDirections> GetDirections(HexDirections directions)
        {
            int mask = (int)directions;
            for (int intDirection = minDirectionMark; intDirection <= maxDirectionMark; intDirection <<= 1)
            {
                if ((intDirection & mask) == 1)
                {
                    yield return (HexDirections)intDirection;
                }
            }
        }

        #endregion


        /// <summary>
        /// 获取到所属的块编号;
        /// </summary>
        public CubicHexCoord Block(int size)
        {
            short x = (short)Math.Round(this.X / (float)size);
            short y = (short)Math.Round(this.Y / (float)size);
            return new CubicHexCoord(x, y);
        }



        /// <summary>
        /// 获取到这个点周围的方向和坐标;从 HexDirection 高位标记开始返回;
        /// </summary>
        public IEnumerable<CubicHexCoord> GetNeighbours()
        {
            foreach (var direction in Directions)
            {
                CubicHexCoord point = this.GetDirection(direction);
                yield return point;
            }
        }

        /// <summary>
        /// 获取到目标点的邻居节点;
        /// </summary>
        public IEnumerable<CubicHexCoord> GetNeighbours(HexDirections directions)
        {
            foreach (var direction in GetDirections(directions))
            {
                yield return this.GetDirection(direction);
            }
        }

        /// <summary>
        /// 获取到目标点的邻居节点,但是也返回自己本身;
        /// </summary>
        public IEnumerable<CubicHexCoord> GetNeighboursAndSelf()
        {
            foreach (var direction in DirectionsAndSelf)
            {
                yield return this.GetDirection(direction);
            }
        }



        /// <summary>
        /// 获取到六边形的范围;
        /// 半径覆盖到的节点;
        /// </summary>
        public static IEnumerable<CubicHexCoord> GetHexRange(CubicHexCoord target, int step)
        {
            for (int x = -step; x <= step; x++)
            {
                for (int y = Math.Max(-step, -x - step); y <= Math.Min(step, -x + step); y++)
                {
                    int z = -x - y;
                    yield return new CubicHexCoord(x, y, z) + target;
                }
            }
        }


        /// <summary>
        /// 获取到目标所在的半径;
        /// </summary>
        public static int GetRadius(CubicHexCoord center, CubicHexCoord target)
        {
            var cube = target - center;
            return GetRadius(cube);
        }

        /// <summary>
        /// 获取到目标所在 (0,0,0) 的半径;
        /// </summary>
        public static int GetRadius(CubicHexCoord target)
        {
            return Mathf.Max(Mathf.Abs(target.X), Mathf.Abs(target.Y), Mathf.Abs(target.Z));
        }


        /// <summary>
        ///  按 环状 返回点;
        /// </summary>
        /// <param name="radius">需要大于0</param>
        public static IEnumerable<CubicHexCoord> GetHexRing(CubicHexCoord center, int radius)
        {
            var cube = center + (CubicHexCoord.GetDirectionOffset(HexDirections.Northeast) * radius);

            foreach (var direction in Directions)
            {
                for (int j = 0; j < radius; j++)
                {
                    yield return cube;
                    cube = cube.GetDirection(direction);
                }
            }
        }

        /// <summary>
        /// 按 螺旋 形状返回点;
        /// </summary>
        /// <param name="radius">需要大于0</param>
        public static List<CubicHexCoord> GetHexSpiral(CubicHexCoord center, int radius)
        {
            List<CubicHexCoord> coords = new List<CubicHexCoord>();
            for (int k = 1; k <= radius; k++)
            {
                coords.AddRange(GetHexRing(center, k));
            }
            return coords;
        }



        IEnumerable<HexDirections> IGridPoint<CubicHexCoord, HexDirections>.Directions
        {
            get { return Directions; }
        }

        IEnumerable<HexDirections> IGridPoint<CubicHexCoord, HexDirections>.DirectionsAndSelf
        {
            get { return DirectionsAndSelf; }
        }

        IEnumerable<HexDirections> IGridPoint<CubicHexCoord, HexDirections>.GetDirections(HexDirections directions)
        {
            return GetDirections(directions);
        }

        IGridPoint IGridPoint<CubicHexCoord, HexDirections>.GetDirection(HexDirections direction)
        {
            return GetDirection(direction);
        }

        IEnumerable<IGridPoint> IGridPoint.GetNeighbours()
        {
            return GetNeighbours().Cast<IGridPoint>();
        }

        IEnumerable<IGridPoint> IGridPoint.GetNeighboursAndSelf()
        {
            return GetNeighboursAndSelf().Cast<IGridPoint>();
        }





        /// <summary>
        /// 将哈希值转换成坐标;
        /// </summary>
        public static CubicHexCoord HashCodeToCoord(int hashCode)
        {
            short x = (short)(hashCode >> 16);
            short y = (short)((hashCode & 0xFFFF) - short.MaxValue);
            return new CubicHexCoord(x, y);
        }

        public override int GetHashCode()
        {
            int hashCode = X << 16;
            hashCode += short.MaxValue + Y;
            return hashCode;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is CubicHexCoord))
                return false;
            return (CubicHexCoord)obj == this;
        }

        public override string ToString()
        {
            return string.Concat("(", X, ",", Y, ",", Z, ")");
        }

        public static CubicHexCoord operator +(CubicHexCoord a, CubicHexCoord b)
        {
            return new CubicHexCoord(a.X + b.X, a.Y + b.Y, a.Z + b.Z);
        }

        public static CubicHexCoord operator -(CubicHexCoord a, CubicHexCoord b)
        {
            return new CubicHexCoord(a.X - b.X, a.Y - b.Y, a.Z - b.Z);
        }

        public static CubicHexCoord operator *(CubicHexCoord a, int k)
        {
            return new CubicHexCoord(a.X * k, a.Y * k, a.Z * k);
        }

        public static CubicHexCoord operator /(CubicHexCoord a, int k)
        {
            return new CubicHexCoord(a.X / k, a.Y / k, a.Z / k);
        }

        public static bool operator ==(CubicHexCoord a, CubicHexCoord b)
        {
            return a.X == b.X && a.Y == b.Y && a.Z == b.Z;
        }

        public static bool operator !=(CubicHexCoord a, CubicHexCoord b)
        {
            return !(a == b);
        }

    }


}
