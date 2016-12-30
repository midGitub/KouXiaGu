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

        public TerrainMapDescr description;

        public static IDictionary<CubicHexCoord, TerrainNode> ActivatedMap
        {
            get { return TerrainController.CurrentMap.Map; }
        }

        /// <summary>
        /// 创建地图到预制目录下;
        /// </summary>
        public static TerrainMap CreateMap(TerrainMapDescr description)
        {
            var map = new TerrainMap(description);
            map.Save();
            return map;
        }

        /// <summary>
        /// 生成一个随机地图,并且加入到现在激活的地图内(忽略重复);
        /// </summary>
        public static void RandomMap(int randomMapSize)
        {
            Dictionary<CubicHexCoord, TerrainNode> map = RandomMap(randomMapSize, LandformRes.initializedInstances.Keys.ToArray());
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
                    terrainMap.Add(item, GetRandomNode(id));
                }
                catch (ArgumentException)
                {
                    continue;
                }
            }
            return terrainMap;
        }

        static TerrainNode GetRandomNode(int[] id)
        {
            return new TerrainNode()
            {
                Landform = id[UnityEngine.Random.Range(0, id.Length)],
                LandformAngle = UnityEngine.Random.Range(0, 360),
                Road = 1,
            };
        }

    }

}
