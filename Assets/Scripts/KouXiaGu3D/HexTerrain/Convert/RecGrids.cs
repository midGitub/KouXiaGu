using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KouXiaGu.HexTerrain
{

    /// <summary>
    /// 矩形网格存在的方向;
    /// </summary>
    [Flags]
    enum RecDirections
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
    public static class RecGrids
    {



        /// <summary>
        /// 获取到目标点的邻居节点;
        /// </summary>
        public static IEnumerable<ShortVector2> GetNeighbours<T>(this IMap<ShortVector2, T> map)
        {
            throw new NotImplementedException();
        }


        /// <summary>
        /// 广度遍历;
        /// </summary>
        public static IEnumerable<ShortVector2> BreadthTraversal<T>(this IMap<ShortVector2, T> map)
        {
            throw new NotImplementedException();
        }



    }

}
