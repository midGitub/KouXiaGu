using System;
using System.Collections.Generic;
using System.Linq;
using KouXiaGu.Grids;
using UnityEngine;

namespace KouXiaGu.Terrain3D
{

    /// <summary>
    /// 地形编辑工具(仅编辑模式);
    /// </summary>
    public class TerrainEditor : MonoBehaviour
    {
        TerrainEditor() { }

        /// <summary>
        /// 在保存预制地图时同时是否重置存档地图;
        /// </summary>
        public bool resetArchiveMap = true;

        public int randomMapSize = 10;

        public MapDescription description;

        public static IDictionary<CubicHexCoord, TerrainNode> ActivatedMap
        {
            get { return TerrainController.CurrentMap.Map; }
        }

        /// <summary>
        /// 创建地图到预制目录下;
        /// </summary>
        public static TerrainMap CreateMap(MapDescription description)
        {
            var map = new TerrainMap(description);
            map.SavePrefab(true);
            return map;
        }

        /// <summary>
        /// 生成一个随机地图,并且加入到现在激活的地图内(忽略重复);
        /// </summary>
        public static void RandomMap(int randomMapSize)
        {
            Dictionary<CubicHexCoord, TerrainNode> map = RandomMap(randomMapSize, Landform.Identifications.ToArray());
            ActivatedMap.AddOrUpdate(map);
        }

        /// <summary>
        /// 返回一个随机地图;
        /// </summary>
        static Dictionary<CubicHexCoord, TerrainNode> RandomMap(int size, params int[] id)
        {
            Dictionary<CubicHexCoord, TerrainNode> terrainMap = new Dictionary<CubicHexCoord, TerrainNode>();

            foreach (var item in CubicHexCoord.Range(CubicHexCoord.Self, size))
            {
                try
                {
                    terrainMap.Add(item, new TerrainNode(id[UnityEngine.Random.Range(0, id.Length)], UnityEngine.Random.Range(0, 360)));
                }
                catch (ArgumentException)
                {
                    continue;
                }
            }
            return terrainMap;
        }

    }

}
