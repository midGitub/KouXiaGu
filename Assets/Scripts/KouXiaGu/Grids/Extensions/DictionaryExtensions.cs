using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KouXiaGu.Grids
{


    public static class DictionaryExtensions
    {

        /// <summary>
        /// 获取到邻居节点;
        /// </summary>
        public static IEnumerable<CoordPack<TVector, TDirection, T>> GetNeighbours<TVector, TDirection, T>(
            this IDictionary<TVector, T> dictionary,
            TVector target)
            where TVector : IGrid<TVector, TDirection>
        {
            foreach (var neighbour in target.GetNeighbours())
            {
                T item;
                if (dictionary.TryGetValue(neighbour.Point, out item))
                {
                    yield return new CoordPack<TVector, TDirection, T>(neighbour, item);
                }
            }
        }


    }



}
