//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using KouXiaGu.Grids;
//using UnityEngine;
//using KouXiaGu.Initialization;
//using KouXiaGu.Collections;

//namespace KouXiaGu.Terrain3D
//{

//    /// <summary>
//    /// 游戏创建工具;
//    /// </summary>
//    [DisallowMultipleComponent]
//    public sealed class MapCreateTool : GlobalSington<MapCreateTool>
//    {
//        MapCreateTool() { }

//        static readonly CubicHexCoord ORIGIN = CubicHexCoord.Self;

//        static readonly System.Random random = new System.Random();

//        [SerializeField]
//        int[] landforms;

//        [SerializeField]
//        bool haveRoad;
//        [SerializeField]
//        int[] roads;
//        [SerializeField]
//        int[] buildings;

//        [SerializeField]
//        int mapSize = 20;

//        protected override void Awake()
//        {
//            base.Awake();
//            GameObject.FindObjectOfType<GameScheduler>().OnGameInitializedEvent += Initialize;
//        }

//        /// <summary>
//        /// 初始化必要的信息;
//        /// </summary>
//        public void Initialize()
//        {
//            landforms = LandformRes.initializedInstances.Keys.ToArray();
//            roads = RoadRes.initializedInstances.Keys.ToArray();
//            buildings = BuildingRes.initializedInstances.Keys.ToArray();
//        }

//        /// <summary>
//        /// 随机的节点;
//        /// </summary>
//        public static TerrainNode RamdomNode()
//        {
//            TerrainNode node = new TerrainNode()
//            {
//                Landform = new LandformNode()
//                {
//                    ID = RamdomFromArray(GetInstance.landforms),
//                    Angle = RamdomAngle(),
//                },

//                Building = new BuildingNode()
//                {
//                    ID = RamdomFromArray(GetInstance.buildings),
//                    Angle = RamdomAngle(),
//                },

//            };
//            return node;
//        }

//        static T RamdomFromArray<T>(T[] array)
//        {
//            return array.Length == 0 ? default(T) : array[random.Next(0, array.Length)];
//        }

//        static float RamdomAngle()
//        {
//            return (float)random.Next(0, 360);
//        }


//        /// <summary>
//        /// 使用节点填满地图;
//        /// </summary>
//        public static void Fill(OObservableDictionary<CubicHexCoord, TerrainNode> map, TerrainNode node)
//        {
//            foreach (var coord in Range())
//            {
//                if (!map.ContainsKey(coord))
//                {
//                    map.Add(coord, node);
//                }
//            }
//        }

//        /// <summary>
//        /// 使用随机节点填满地图;
//        /// </summary>
//        public static void Fill(OObservableDictionary<CubicHexCoord, TerrainNode> map)
//        {
//            foreach (var coord in Range())
//            {
//                if (!map.ContainsKey(coord))
//                {
//                    TerrainNode node = RamdomNode();
//                    map.Add(coord, node);
//                }
//            }
//        }

//        /// <summary>
//        /// 替换整个地图;
//        /// </summary>
//        public static void Replace(OObservableDictionary<CubicHexCoord, TerrainNode> map, TerrainNode node)
//        {
//            foreach (var coord in Range())
//            {
//                map.AddOrUpdate(coord, node);
//            }
//        }

//        /// <summary>
//        /// 随机节点替换整个地图;
//        /// </summary>
//        public static void Replace(OObservableDictionary<CubicHexCoord, TerrainNode> map)
//        {
//            foreach (var coord in Range())
//            {
//                TerrainNode node = RamdomNode();
//                map.AddOrUpdate(coord, node);
//            }
//        }


//        /// <summary>
//        /// 获取到范围;
//        /// </summary>
//        public static IEnumerable<CubicHexCoord> Range()
//        {
//            return CubicHexCoord.Range(ORIGIN, GetInstance.mapSize);
//        }


//    }

//}
