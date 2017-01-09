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

        public MapDescr description;

        public int[] landforms;

        public static IDictionary<CubicHexCoord, TerrainNode> ActivatedMap
        {
            get { return TerrainInitializer.CurrentMap.Map; }
        }

        /// <summary>
        /// 创建地图到预制目录下;
        /// </summary>
        public static TerrainMap CreateMap(MapDescr description)
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
            Dictionary<CubicHexCoord, TerrainNode> map = RandomMap1(randomMapSize, LandformRes.initializedInstances.Keys.ToArray());
            ActivatedMap.AddOrUpdate(map);
        }

        /// <summary>
        /// 生成一个随机地图,并且加入到现在激活的地图内(忽略重复);
        /// </summary>
        public static void RandomMap(int randomMapSize, int[] landforms)
        {
            Dictionary<CubicHexCoord, TerrainNode> map = RandomMap1(randomMapSize, landforms);
            ActivatedMap.AddOrUpdate(map);
        }

        /// <summary>
        /// 返回一个随机地图;
        /// </summary>
        static Dictionary<CubicHexCoord, TerrainNode> RandomMap1(int size, params int[] id)
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
            int landform = id[UnityEngine.Random.Range(0, id.Length)];
            int road = landform == 20 ? 1 : 0;

            return new TerrainNode()
            {
                Landform = landform,
                LandformAngle = UnityEngine.Random.Range(0, 360),
                Road = road,
            };
        }

    }

}
