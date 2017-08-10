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
        Unknown = 0,
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
        Unknown = 0,
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
    /// 所有有效坐标都满足 X + Y + Z = 0;
    /// </summary>
    [ProtoContract, Serializable]
    public struct CubicHexCoord : IEquatable<CubicHexCoord>, IGrid, IGrid<CubicHexCoord>, IGrid<CubicHexCoord, HexDirections>
    {
        public CubicHexCoord(short x, short y, short z)
        {
            OutOfRangeException(x, y, z);

            this.x = x;
            this.y = y;
            this.z = z;
        }

        public CubicHexCoord(short x, short y) : this()
        {
            SetValue(x, y);
        }

        public CubicHexCoord(int x, int y, int z)
        {
            OutOfRangeException((short)x, (short)y, (short)z);
            this.x = (short)x;
            this.y = (short)y;
            this.z = (short)z;
        }

        public CubicHexCoord(float x, float y, float z)
        {
            int intX = (int)(Math.Round(x));
            int intY = (int)(Math.Round(y));
            int intZ = (int)(Math.Round(z));
            double x_diff = Math.Abs(intX - x);
            double y_diff = Math.Abs(intY - y);
            double z_diff = Math.Abs(intZ - z);
            if (x_diff > y_diff && x_diff > z_diff)
            {
                intX = -intY - intZ;
            }
            else
            {
                if (y_diff > z_diff)
                {
                    intY = -intX - intZ;
                }
                else
                {
                    intZ = -intX - intY;
                }
            }

            OutOfRangeException(intX, intY, intZ);

            this.x = intX;
            this.y = intY;
            this.z = intZ;
        }

        /// <summary>
        /// 存在方向数,包括本身;
        /// </summary>
        public const int DirectionsNumber = 7;

        /// <summary>
        /// 存在的对角线方向数目;
        /// </summary>
        public const int DiagonalsNumber = 7;

        const int maxDirectionMark = (int)HexDirections.Self;
        const int minDirectionMark = (int)HexDirections.North;

        /// <summary>
        /// (0,0,0)
        /// </summary>
        public static readonly CubicHexCoord Self = new CubicHexCoord(0, 0, 0);

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

        [ProtoMember(1), SerializeField]
        int x;
        [ProtoMember(2), SerializeField]
        int y;
        [SerializeField]
        int z;

        public int X
        {
            get { return x; }
        }

        public int Y
        {
            get { return y; }
        }

        /// <summary>
        /// Z = - X - Y;
        /// </summary>
        public int Z
        {
            get { return z; }
        }

        /// <summary>
        /// 0 获取到 x, 1 获取到 y, 2 获取到 z, 其它 返回异常;
        /// </summary>
        public int this[int index]
        {
            get {
                switch (index)
                {
                    case 0:
                        return x;
                    case 1:
                        return y;
                    case 2:
                        return z;
                    default:
                        throw new IndexOutOfRangeException();
                }
            }
            set {
                switch (index)
                {
                    case 0:
                         x = value;
                        break;
                    case 1:
                        y = value;
                        break;
                    case 2:
                        z = value;
                        break;
                    default:
                        throw new IndexOutOfRangeException();
                }
            }
        }

        /// <summary>
        /// 在反序列化后调用;
        /// </summary>
        [ProtoAfterDeserialization]
        void ProtoAfterDeserialization()
        {
            z = (short)(-x - y);
        }

        public void SetValue(short x, short y)
        {
            this.x = x;
            this.y = y;
            z = (short)(-x - y);
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

        /// <summary>
        /// 超出定义范围返回异常;
        /// </summary>
        public static void OutOfRangeException(int x, int y, int z)
        {
            if ((x + y + z) != 0)
                throw new ArgumentOutOfRangeException("坐标必须满足 (x + y + z) == 0");
        }

        public override bool Equals(object obj)
        {
            if (!(obj is CubicHexCoord))
                return false;
            return (CubicHexCoord)obj == this;
        }

        public bool Equals(CubicHexCoord other)
        {
            return X == other.X && Y == other.Y && Z == other.Z;
        }

        public override int GetHashCode()
        {
            //int hashCode = x ^ y;

            //在数值不大于10位的时候获得最好的性能;
            int hashCode = x << 21 & -0x7FE00000;
            hashCode |= y << 10 & 0x1FFC00;
            hashCode |= z & 0x3FF;
            return hashCode;
        }

        ///// <summary>
        ///// 将哈希值转换成坐标;
        ///// </summary>
        //public static CubicHexCoord HashCodeToCoord(int hashCode)
        //{
        //    short x = (short)(hashCode >> 16);
        //    short y = (short)((hashCode & 0xFFFF) - short.MaxValue);
        //    return new CubicHexCoord(x, y);
        //}

        public override string ToString()
        {
            return string.Concat("(", X, ",", Y, ",", Z, ")");
        }

        /// <summary>
        /// 获取到这个方向的坐标;
        /// </summary>
        public CubicHexCoord GetDirection(HexDirections direction)
        {
            return this + GetDirectionOffset(direction);
        }

        /// <summary>
        /// 获取到这个方向的反方向坐标;
        /// </summary>
        public CubicHexCoord GetOpposite(HexDirections direction)
        {
            return this + GetOppositeDirectionOffset(direction);
        }

        /// <summary>
        /// 获取到这个对角线的偏移量;
        /// </summary>
        public CubicHexCoord GetDiagonal(HexDiagonals diagonal)
        {
            return this + GetDiagonalOffset(diagonal);
        }


        /// <summary>
        /// 按标记为从 高位到低位 循序返回的迭代结构;不包含本身
        /// </summary>
        public static IEnumerable<HexDirections> Directions
        {
            get { return DirectionsArray; }
        }

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
        /// 获取到这个点周围的方向和坐标;从 HexDirection 高位标记开始返回;
        /// </summary>
        public IEnumerable<CoordPack<CubicHexCoord, HexDirections>> GetNeighbours()
        {
            foreach (var direction in DirectionsArray)
            {
                CubicHexCoord point = GetDirection(direction);
                yield return new CoordPack<CubicHexCoord, HexDirections>(point, direction);
            }
        }

        /// <summary>
        /// 获取到目标点的邻居节点;
        /// </summary>
        public IEnumerable<CoordPack<CubicHexCoord, HexDirections>> GetNeighbours(HexDirections directions)
        {
            foreach (var direction in GetDirections(directions))
            {
                CubicHexCoord point = GetDirection(direction);
                yield return new CoordPack<CubicHexCoord, HexDirections>(point, direction);
            }
        }

        IEnumerable<CubicHexCoord> IGrid<CubicHexCoord>.GetNeighbours()
        {
            return GetNeighbours().Select(coord => coord.Point);
        }

        IEnumerable<IGrid> IGrid.GetNeighbours()
        {
            return GetNeighbours().Select(coord => coord.Point).Cast<IGrid>();
        }


        /// <summary>
        /// 获取到从 高位到低位 顺序返回的迭代结构;包括本身;
        /// </summary>
        public static IEnumerable<HexDirections> DirectionsAndSelf
        {
            get { return DirectionsAndSelfArray; }
        }

        /// <summary>
        /// 按标记为从 高位到低位 循序排列的数组;
        /// </summary>
        static readonly HexDirections[] DirectionsAndSelfArray = new HexDirections[]
        {
            HexDirections.Self,
            HexDirections.Northwest,
            HexDirections.Southwest,
            HexDirections.South,
            HexDirections.Southeast,
            HexDirections.Northeast,
            HexDirections.North,
        };

        /// <summary>
        /// 获取到目标点的邻居节点,但是也返回自己本身;
        /// </summary>
        public IEnumerable<CoordPack<CubicHexCoord, HexDirections>> GetNeighboursAndSelf()
        {
            foreach (var direction in DirectionsAndSelf)
            {
                CubicHexCoord point = GetDirection(direction);
                yield return new CoordPack<CubicHexCoord, HexDirections>(point, direction);
            }
        }

        IEnumerable<CubicHexCoord> IGrid<CubicHexCoord>.GetNeighboursAndSelf()
        {
            return GetNeighboursAndSelf().Select(coord => coord.Point);
        }

        IEnumerable<IGrid> IGrid.GetNeighboursAndSelf()
        {
            return GetNeighboursAndSelf().Select(coord => coord.Point).Cast<IGrid>();
        }


        /// <summary>
        /// 获取到方向集表示的所有方向;
        /// </summary>
        public static IEnumerable<HexDirections> GetDirections(HexDirections directions)
        {
            int mask = (int)directions;
            for (int intDirection = minDirectionMark; intDirection <= maxDirectionMark; intDirection <<= 1)
            {
                if ((intDirection & mask) != 0)
                {
                    yield return (HexDirections)intDirection;
                }
            }
        }

        /// <summary>
        /// 曼哈顿距离;
        /// </summary>
        public static int ManhattanDistances(CubicHexCoord a, CubicHexCoord b)
        {
            CubicHexCoord hex = a - b;
            return (Math.Abs(hex.X) + Math.Abs(hex.Y) + Math.Abs(hex.Z)) / 2;
        }


        /// <summary>
        /// 方向偏移量;
        /// </summary>
        static readonly Dictionary<int, CubicHexCoord> directionVectors = new Dictionary<int, CubicHexCoord>()
        {
            { (int)HexDirections.North, DIR_North },
            { (int)HexDirections.Northeast, DIR_Northeast },
            { (int)HexDirections.Southeast, DIR_Southeast },
            { (int)HexDirections.South, DIR_South },
            { (int)HexDirections.Southwest, DIR_Southwest },
            { (int)HexDirections.Northwest, DIR_Northwest },
            { (int)HexDirections.Self, Self },
        };

        /// <summary>
        /// 获取到方向偏移量;
        /// </summary>
        public static CubicHexCoord GetDirectionOffset(HexDirections direction)
        {
            return directionVectors[(int)direction];
        }

        /// <summary>
        /// 使其根据(0,0,0)坐标进行翻转\类似镜像,一般对方向变量使用,获取到其相反方向;
        /// </summary>
        public static CubicHexCoord GetOppositeDirectionOffset(HexDirections direction)
        {
            return GetDirectionOffset(direction) * -1;
        }


        /// <summary>
        /// 相对方向映射;
        /// </summary>
        static readonly Dictionary<int, HexDirections> oppositeDirection = new Dictionary<int, HexDirections>()
        {
            { (int)HexDirections.North, HexDirections.South },
            { (int)HexDirections.Northeast, HexDirections.Southwest },
            { (int)HexDirections.Southeast, HexDirections.Northwest },
            { (int)HexDirections.South, HexDirections.North },
            { (int)HexDirections.Southwest, HexDirections.Northeast },
            { (int)HexDirections.Northwest, HexDirections.Southeast },
            { (int)HexDirections.Self, HexDirections.Self },
        };

        /// <summary>
        /// 获取到这个方向的相反方向;
        /// </summary>
        public static HexDirections GetOppositeDirection(HexDirections direction)
        {
            return oppositeDirection[(int)direction];
        }


        /// <summary>
        /// 对角线偏移量;
        /// </summary>
        static readonly Dictionary<int, CubicHexCoord> diagonalVectors = new Dictionary<int, CubicHexCoord>()
        {
            { (int)HexDiagonals.Northeast, DIA_Northeast },
            { (int)HexDiagonals.East, DIA_East },
            { (int)HexDiagonals.Southeast, DIA_Southeast },
            { (int)HexDiagonals.Southwest, DIA_Southwest },
            { (int)HexDiagonals.West, DIA_West },
            { (int)HexDiagonals.Northwest, DIA_Northwest },
            { (int)HexDiagonals.Self, Self },
        };

        /// <summary>
        /// 获取到对角线偏移量;
        /// </summary>
        public static CubicHexCoord GetDiagonalOffset(HexDiagonals diagonal)
        {
            return diagonalVectors[(int)diagonal];
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
        /// 获取到六边形的范围;
        /// 半径覆盖到的节点;
        /// </summary>
        public static IEnumerable<CubicHexCoord> Range(CubicHexCoord target, int step)
        {
            if (step < 0)
                throw new ArgumentOutOfRangeException();

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
        /// 按 螺旋 形状返回点;
        /// </summary>
        /// <param name="radius">需要大于0</param>
        public static List<CubicHexCoord> Spiral(CubicHexCoord center, int radius)
        {
            if (radius <= 0)
                throw new ArgumentOutOfRangeException();

            List<CubicHexCoord> coords = new List<CubicHexCoord>();
            Spiral(center, radius, ref coords);
            return coords;
        }

        /// <summary>
        /// 按 螺旋 形状返回点;
        /// </summary>
        /// <param name="center">中心点</param>
        /// <param name="radius">半径</param>
        /// <param name="coords">保存坐标的合集</param>
        /// <returns></returns>
        public static void Spiral(CubicHexCoord center, int radius, ref List<CubicHexCoord> coords)
        {
            if (radius <= 0)
                throw new ArgumentOutOfRangeException();
            if (coords == null)
                throw new ArgumentNullException();

            for (int k = 1; k <= radius; k++)
            {
                coords.AddRange(Ring(center, k));
            }
        }

        /// <summary>
        ///  按 环状 返回点;
        /// </summary>
        /// <param name="radius">需要大于0</param>
        public static IEnumerable<CubicHexCoord> Ring(CubicHexCoord center, int radius)
        {
            if (radius <= 0)
                throw new ArgumentOutOfRangeException();

            var cube = center + (GetDirectionOffset(HexDirections.Northeast) * radius);

            foreach (var direction in Directions)
            {
                for (int j = 0; j < radius; j++)
                {
                    yield return cube;
                    cube = cube.GetDirection(direction);
                }
            }
        }


        public static bool operator ==(CubicHexCoord a, CubicHexCoord b)
        {
            return a.Equals(b);
        }

        public static bool operator !=(CubicHexCoord a, CubicHexCoord b)
        {
            return !a.Equals(b);
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
    }
}
