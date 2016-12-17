using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KouXiaGu.Grids;
using UnityEngine;

namespace KouXiaGu.Terrain3D
{

    /// <summary>
    /// 地形编辑工具;
    /// </summary>
    public class TerrainEditor : MonoBehaviour
    {
        TerrainEditor() { }

        [SerializeField]
        public MapDescription description;

        /// <summary>
        /// 创建地图到预制目录下;
        /// </summary>
        public static TerrainMap CreateMap(MapDescription description)
        {
            var map = new TerrainMap(description);
            return map;
        }

        /// <summary>
        /// 
        /// </summary>
        public static void RandomMap(TerrainMap map)
        {

        }

        /// <summary>
        /// 返回一个随机地图;
        /// </summary>
        static Map<CubicHexCoord, TerrainNode> RandomMap(int size, params int[] aa)
        {
            Map<CubicHexCoord, TerrainNode> terrainMap = new Map<CubicHexCoord, TerrainNode>();

            foreach (var item in CubicHexCoord.GetHexRange(CubicHexCoord.Self, size))
            {
                try
                {
                    terrainMap.Add(item, new TerrainNode(aa[UnityEngine.Random.Range(0, aa.Length)], UnityEngine.Random.Range(0, 360)));
                }
                catch (ArgumentException)
                {

                }
            }
            return terrainMap;
        }

    }

}
