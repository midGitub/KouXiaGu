//using System;
//using System.Threading.Tasks;
//using UnityEngine;
//using System.Threading;
//using JiongXiaGu.Unity.Resources.Archives;
//using JiongXiaGu.Unity.RectTerrain.Resources;

//namespace JiongXiaGu.Unity.RectMaps
//{

//    /// <summary>
//    /// 地图全局控制器;
//    /// </summary>
//    [DisallowMultipleComponent]
//    public sealed class RectMapController : SceneSington<RectMapController>
//    {
//        RectMapController()
//        {
//        }

//        [SerializeField]
//        bool isUseRandomMap;
//        [SerializeField]
//        int randomMapRadius;

//        /// <summary>
//        /// 是否使用随机地图?
//        /// </summary>
//        public bool IsUseRandomMap
//        {
//            get { return isUseRandomMap; }
//            set { isUseRandomMap = value; }
//        }

//        /// <summary>
//        /// 生成的随机地图大小;
//        /// </summary>
//        public int RandomMapRadius
//        {
//            get { return randomMapRadius; }
//            set { randomMapRadius = value; }
//        }

//        /// <summary>
//        /// 获取到一个游戏使用的地图;
//        /// </summary>
//        public WorldMap GetWorldMap(Archive archive, CancellationToken token)
//        {
//            throw new NotImplementedException();
//        }

//        WorldMap GetRandomMap()
//        {
//            RectTerrainResources rectTerrainResources = RectTerrainResourcesInitializer.RectTerrainResources;

//            if (rectTerrainResources == null)
//                throw new ArgumentException("RectTerrainResources 未初始化完成!");

//            var mapGenerator = new SimpleMapDataGenerator(rectTerrainResources);
//            Map map = new Map("RandomMap");
//            mapGenerator.Generate(map.Data, randomMapRadius);
//            return new WorldMap(map);
//        }
//    }
//}
