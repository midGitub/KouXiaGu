using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using KouXiaGu.Grids;

namespace KouXiaGu.Terrain3D
{

    /// <summary>
    /// 道路创建；
    /// </summary>
    public class RoadBuilder : MonoBehaviour
    {
        RoadBuilder() { }


        HashSet<CubicHexCoord> closePoints;

        void Awake()
        {
            closePoints = new HashSet<CubicHexCoord>();
        }

        /// <summary>
        /// 获取到地图内所以的道路路径点；
        /// </summary>
        public IEnumerable<List<CubicHexCoord>> Create(IDictionary<CubicHexCoord, TerrainNode> map)
        {
            IEnumerator<KeyValuePair<CubicHexCoord, TerrainNode>> mapEnumerator = map.GetEnumerator();

            mapEnumerator.MoveNext();


            foreach (var pair in map)
            {

            }

            throw new NotImplementedException();
        }


        List<CubicHexCoord> CreatePath(IDictionary<CubicHexCoord, TerrainNode> map, CubicHexCoord target, int id)
        {
            List<CubicHexCoord> path = new List<CubicHexCoord>();
            path.Add(target);

            //while ()
            //{

            //}

            return path;
        }



    }

}
