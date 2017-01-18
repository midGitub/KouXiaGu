//using System;
//using System.Collections.Generic;
//using System.Linq;
//using KouXiaGu.Collections;
//using KouXiaGu.Grids;
//using UnityEngine;

//namespace KouXiaGu.Terrain3D
//{

//    /// <summary>
//    /// 随机地形创建;
//    /// </summary>
//    public class MapCreater
//    {
//        static readonly CubicHexCoord ORIGIN = CubicHexCoord.Self;

//        static readonly System.Random random = new System.Random();


//        /// <summary>
//        /// 前提,若不满足则返回异常;
//        /// </summary>
//        public static void Precondition()
//        {
//            if (ResInitializer.IsInitialized == false)
//                throw new PremiseNotInvalidException();
//        }


//        public MapCreater()
//        {
//            Precondition();

//            landforms = LandformRes.initializedInstances.Keys.ToArray();
//            roads = RoadRes.initializedInstances.Keys.ToArray();
//            buildings = BuildingRes.initializedInstances.Keys.ToArray();
//        }

//        int[] landforms;
//        int[] roads;
//        int[] buildings;

//        /// <summary>
//        /// 随机的节点;
//        /// </summary>
//        public TerrainNode RamdomNode()
//        {
//            TerrainNode node = new TerrainNode()
//            {
//                Landform = RamdomFromArray(landforms),
//                LandformAngle = RamdomAngle(),

//                Road = RamdomFromArray(roads),

//                Building = RamdomFromArray(buildings),
//                BuildingAngle = RamdomAngle(),
//            };
//            return node;
//        }

//        T RamdomFromArray<T>(T[] array)
//        {
//            return array[random.Next(0, array.Length - 1)];
//        }

//        float RamdomAngle()
//        {
//            return (float)random.NextDouble();
//        }

//        /// <summary>
//        /// 使用节点填满地图;
//        /// </summary>
//        public void Fill(IDictionary<CubicHexCoord, TerrainNode> map, int size, TerrainNode node)
//        {
//            foreach (var coord in Range(size))
//            {
//                if (!map.ContainsKey(coord))
//                {
//                    map.Add(coord, node);
//                }
//            }
//        }

//        /// <summary>
//        /// 替换整个地图;
//        /// </summary>
//        public void Replace(IDictionary<CubicHexCoord, TerrainNode> map, int size, TerrainNode node)
//        {
//            foreach (var coord in Range(size))
//            {
//                map.AddOrUpdate(coord, node);
//            }
//        }

//        /// <summary>
//        /// 随机填满;
//        /// </summary>
//        public void Ramdom(IDictionary<CubicHexCoord, TerrainNode> map, int size)
//        {
//            foreach (var coord in Range(size))
//            {
//                map.AddOrUpdate(coord, RamdomNode());
//            }
//        }

//        /// <summary>
//        /// 获取到范围;
//        /// </summary>
//        public IEnumerable<CubicHexCoord> Range(int size)
//        {
//            return CubicHexCoord.Range(ORIGIN, size);
//        }

//    }

//}
