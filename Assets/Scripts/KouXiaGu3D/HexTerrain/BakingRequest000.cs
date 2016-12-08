//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using UnityEngine;

//namespace KouXiaGu.HexTerrain
//{




//    /// <summary>
//    /// 生成烘焙地貌的请求;
//    /// </summary>
//    public struct BakingRequest : IEquatable<BakingRequest>
//    {

//        public ShortVector2 MapPoint { get; private set; }

//        public IReadOnlyMap2D<LandformNode> TerrainMap { get; private set; }

//        /// <summary>
//        /// 初始化需要烘焙的节点;
//        /// </summary>
//        public BakingRequest(ShortVector2 mapPoint, IReadOnlyMap2D<LandformNode> terrainMap)
//        {
//            this.MapPoint = mapPoint;
//            this.TerrainMap = terrainMap;

//            ExceptionOutOfMapRange();
//        }

//        /// <summary>
//        /// 烘焙的节点是否合法?不合法返回异常;
//        /// 若地图不存在这个节点则不合法;
//        /// </summary>
//        void ExceptionOutOfMapRange()
//        {
//            if (!IsOutOfMapRange(MapPoint, TerrainMap))
//            {
//                throw new IndexOutOfRangeException("超出地图范围的烘焙;");
//            }
//        }

//        /// <summary>
//        /// 烘焙的节点是否合法?
//        /// 若地图不存在这个节点则不合法;
//        /// </summary>
//        public static bool IsOutOfMapRange(ShortVector2 mapPoint, IReadOnlyMap2D<LandformNode> terrainMap)
//        {
//            if (!terrainMap.Contains(mapPoint))
//            {
//                return false;
//            }
//            return true;
//        }

//        /// <summary>
//        /// 包含所有方向需要进行烘焙的节点信息,不需要渲染的方向 Landform 置为NULL;
//        /// </summary>
//        public IEnumerable<HexDirectionPack<ShortVector2, Landform>> GetBakingRange()
//        {
//            foreach (var pair in TerrainMap.GetNeighboursAndSelfOrDefault(MapPoint))
//            {
//                LandformNode landformNode = pair.Item;
//                Landform landform = GetLandform(landformNode);
//                yield return new HexDirectionPack<ShortVector2, Landform>(pair.Direction, pair.Point, landform);
//            }
//        }

//        /// <summary>
//        /// 根据地貌节点获取到地貌信息;
//        /// </summary>
//        Landform GetLandform(LandformNode landformNode)
//        {
//            if (landformNode.ID == 0)
//                return null;

//            Landform landform = LandformManager.GetInstance[landformNode.ID];
//            return landform;
//        }

//        public bool Equals(BakingRequest other)
//        {
//            return this.MapPoint == other.MapPoint &&
//                this.TerrainMap == other.TerrainMap;
//        }

//    }

//}
