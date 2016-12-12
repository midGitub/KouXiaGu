using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace KouXiaGu.Grids
{

    /// <summary>
    /// 对网格类型的结构拓展;
    /// </summary>
    public static class GridsExtensions
    {

        /// <summary>
        /// 获取到目标节点邻居节点,若节点不存在则不返回;
        /// </summary>
        public static IEnumerable<CoordPack<TC, TD, T>> GetNeighbours<TC, TD, T>(this IMap<TC, T> map, TC target)
             where TC : IGridPoint<TC, TD>
        {
            T item;
            foreach (var direction in target.Directions)
            {
                TC offsetCoord = (TC)target.GetDirection(direction);
                if (map.TryGetValue(offsetCoord, out item))
                {
                    yield return new CoordPack<TC, TD, T>(direction, offsetCoord, item);
                }
            }
        }

        /// <summary>
        /// 获取到目标节点和其邻居节点,若节点不存在则不返回;(存在本身方向,且最先返回);
        /// </summary>
        public static IEnumerable<CoordPack<TC, TD, T>> GetNeighboursAndSelf<TC, TD, T>(this IMap<TC, T> map, TC target)
             where TC : IGridPoint<TC, TD>
        {
            T item;
            foreach (var direction in target.DirectionsAndSelf)
            {
                TC offsetCoord = (TC)target.GetDirection(direction);
                if (map.TryGetValue(offsetCoord, out item))
                {
                    yield return new CoordPack<TC, TD, T>(direction, offsetCoord, item);
                }
            }
        }

        /// <summary>
        /// 获取到满足条件的方向;若方向不存在节点则为不满足;
        /// </summary>
        public static int GetNeighboursAndSelfMask<TC, TD, T>(this IMap<TC, T> map, TC target, Func<T, bool> func)
            where TC : IGridPoint<TC, TD>
            where TD : IConvertible
        {
            int directions = 0;
            T item;
            IEnumerable<TD> aroundDirection = target.DirectionsAndSelf;
            foreach (var direction in aroundDirection)
            {
                TC vePoint = (TC)target.GetDirection(direction);
                if (map.TryGetValue(vePoint, out item))
                {
                    if (func(item))
                        directions |= direction.ToInt32(null);
                }
            }
            return directions;
        }

    }

}
