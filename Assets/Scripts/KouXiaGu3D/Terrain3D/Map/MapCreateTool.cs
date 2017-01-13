using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KouXiaGu.Grids;
using UnityEngine;

namespace KouXiaGu.Terrain3D
{

    /// <summary>
    /// 游戏创建工具;
    /// </summary>
    [DisallowMultipleComponent]
    public sealed class MapCreateTool : UnitySington<MapCreateTool>
    {
        MapCreateTool() { }

        static readonly CubicHexCoord ORIGIN = CubicHexCoord.Self;

        static readonly System.Random random = new System.Random();

        static int[] landforms;
        static int[] roads;
        static int[] buildings;

        TerrainMap Map;

        /// <summary>
        /// 初始化必要的信息;
        /// </summary>
        public void Initialize()
        {
        }

        /// <summary>
        /// 随机的节点;
        /// </summary>
        public TerrainNode RamdomNode()
        {
            TerrainNode node = new TerrainNode()
            {
                Landform = RamdomFromArray(landforms),
                LandformAngle = RamdomAngle(),

                Road = RamdomFromArray(roads),

                Building = RamdomFromArray(buildings),
                BuildingAngle = RamdomAngle(),
            };
            return node;
        }

        T RamdomFromArray<T>(T[] array)
        {
            return array[random.Next(0, array.Length - 1)];
        }

        float RamdomAngle()
        {
            return (float)random.NextDouble();
        }

        /// <summary>
        /// 使用节点填满地图;
        /// </summary>
        public void Fill(TerrainMap map, int size, TerrainNode node)
        {
            foreach (var coord in Range(size))
            {
                if (!map.ContainsKey(coord))
                {
                    map.Add(coord, node);
                }
            }
        }

        /// <summary>
        /// 替换整个地图;
        /// </summary>
        public void Replace(TerrainMap map, int size, TerrainNode node)
        {
            foreach (var coord in Range(size))
            {
                map.AddOrUpdate(coord, node);
            }
        }

        /// <summary>
        /// 随机填满;
        /// </summary>
        public void Ramdom(TerrainMap map, int size)
        {
            foreach (var coord in Range(size))
            {
                map.AddOrUpdate(coord, RamdomNode());
            }
        }

        /// <summary>
        /// 获取到范围;
        /// </summary>
        public IEnumerable<CubicHexCoord> Range(int size)
        {
            return CubicHexCoord.Range(ORIGIN, size);
        }


    }

}
