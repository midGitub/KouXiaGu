using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KouXiaGu.Grids
{

    /// <summary>
    /// 矩形网格存在的方向;
    /// </summary>
    [Flags]
    public enum RecDirections
    {
        North = 1,
        Northeast = 2,
        East = 4,
        Southeast = 8,
        South = 16,
        Southwest = 32,
        West = 64,
        Northwest = 128,
        Self = 256,
    }

    /// <summary>
    /// 矩形网格拓展方法;
    /// </summary>
    public static partial class RecGrids
    {


        #region 方向

        /// <summary>
        /// 存在方向数;
        /// </summary>
        public const int DirectionsNumber = 9;

        /// <summary>
        /// 方向偏移量;
        /// </summary>
        static Dictionary<int, ShortVector2> directions = RecirectionDictionary();

        /// <summary>
        /// 获取到方向偏移量;
        /// </summary>
        static Dictionary<int, ShortVector2> RecirectionDictionary()
        {
            Dictionary<int, ShortVector2> directions = new Dictionary<int, ShortVector2>(DirectionsNumber);

            directions.Add((int)RecDirections.North, ShortVector2.Up);
            directions.Add((int)RecDirections.Northeast, ShortVector2.Up + ShortVector2.Right);
            directions.Add((int)RecDirections.East, ShortVector2.Right);
            directions.Add((int)RecDirections.Southeast, ShortVector2.Down + ShortVector2.Right);
            directions.Add((int)RecDirections.South, ShortVector2.Down);
            directions.Add((int)RecDirections.Southwest, ShortVector2.Down + ShortVector2.Left);
            directions.Add((int)RecDirections.West, ShortVector2.Left);
            directions.Add((int)RecDirections.Northwest, ShortVector2.Up + ShortVector2.Left);
            directions.Add((int)RecDirections.Self, ShortVector2.Zero);

            return directions;
        }

        /// <summary>
        /// 获取到方向偏移量;
        /// </summary>
        public static ShortVector2 GetDirection(RecDirections direction)
        {
            return directions[(int)direction];
        }

        /// <summary>
        /// 获取到这个方向的坐标;
        /// </summary>
        public static ShortVector2 GetDirection(this ShortVector2 coord, RecDirections direction)
        {
            return coord + GetDirection(direction);
        }




        const int maxDirectionMark = (int)RecDirections.Self;
        const int minDirectionMark = (int)RecDirections.North;

        /// <summary>
        /// 按标记为从 高位到低位 循序排列的数组;不包含本身
        /// </summary>
        static readonly RecDirections[] DirectionsArray = new RecDirections[]
        {
            RecDirections.Northwest,
            RecDirections.West,
            RecDirections.Southwest,
            RecDirections.South,
            RecDirections.Southeast,
            RecDirections.East,
            RecDirections.Northeast,
            RecDirections.North,
        };

        /// <summary>
        /// 按标记为从 高位到低位 循序排列的数组(存在本身方向,且在最高位);
        /// </summary>
        static readonly RecDirections[] DirectionsAndSelfArray = Enum.GetValues(typeof(RecDirections)).
            Cast<RecDirections>().Reverse().ToArray();

        /// <summary>
        /// 按标记为从 高位到低位 循序返回的迭代结构;不包含本身
        /// </summary>
        public static IEnumerable<RecDirections> Directions
        {
            get { return DirectionsArray; }
        }

        /// <summary>
        /// 获取到从 高位到低位 顺序返回的迭代结构;(存在本身方向,且在最高位);
        /// </summary>
        public static IEnumerable<RecDirections> DirectionsAndSelf
        {
            get { return DirectionsAndSelfArray; }
        }

        /// <summary>
        /// 获取到方向集表示的所有方向;
        /// </summary>
        public static IEnumerable<RecDirections> GetDirections(this RecDirections directions)
        {
            int mask = (int)directions;
            for (int intDirection = minDirectionMark; intDirection <= maxDirectionMark; intDirection <<= 1)
            {
                if ((intDirection & mask) == 1)
                {
                    yield return (RecDirections)intDirection;
                }
            }
        }

        #endregion


        /// <summary>
        /// 获取到所属的块编号;
        /// </summary>
        public static ShortVector2 Block(this ShortVector2 target, int size)
        {
            short x = (short)Math.Round(target.x / (float)size);
            short y = (short)Math.Round(target.y / (float)size);
            return new ShortVector2(x, y);
        }

        



        /// <summary>
        /// 获取到目标点的邻居节点;
        /// </summary>
        public static IEnumerable<ShortVector2> GetNeighbours(this ShortVector2 target)
        {
            foreach (var direction in Directions)
            {
                yield return target.GetDirectionOffset(direction);
            }
        }

        /// <summary>
        /// 获取到目标点的邻居节点;
        /// </summary>
        public static IEnumerable<ShortVector2> GetNeighbours(this ShortVector2 target, RecDirections directions)
        {
            foreach (var direction in GetDirections(directions))
            {
                yield return target.GetDirectionOffset(direction);
            }
        }

        /// <summary>
        /// 获取到目标点的邻居节点,但是也返回自己本身;
        /// </summary>
        public static IEnumerable<ShortVector2> GetNeighboursAndSelf(this ShortVector2 target)
        {
            foreach (var direction in DirectionsAndSelf)
            {
                yield return target.GetDirectionOffset(direction);
            }
        }


        /// <summary>
        /// 广度遍历;
        /// </summary>
        /// <param name="close">这个节点是否为关闭状态,若为关闭状态则返回,也不返回其邻居节点;</param>
        /// <param name="capacity">估计返回的节点数</param>
        public static IEnumerable<ShortVector2> BreadthTraversal(this ShortVector2 target, Func<ShortVector2, bool> close, int capacity = 81)
        {
            if (!close(target))
            {
                yield return target;

                Queue<ShortVector2> waitPoints = new Queue<ShortVector2>(capacity);
                HashSet<ShortVector2> returnedPoints = new HashSet<ShortVector2>();

                waitPoints.Enqueue(target);
                returnedPoints.Add(target);

                while (waitPoints.Count != 0)
                {
                    ShortVector2 point = waitPoints.Dequeue();

                    foreach (var neighbour in point.GetNeighbours())
                    {
                        if (!returnedPoints.Contains(neighbour))
                        {
                            if (close(neighbour))
                            {
                                returnedPoints.Add(neighbour);
                            }
                            else
                            {
                                yield return neighbour;
                                returnedPoints.Add(neighbour);
                                waitPoints.Enqueue(neighbour);
                            }
                        }
                    }
                }
            }
        }

    }

}
