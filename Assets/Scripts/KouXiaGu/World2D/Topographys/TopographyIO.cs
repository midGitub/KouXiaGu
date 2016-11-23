//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using KouXiaGu.World2D.Map;
//using UnityEngine;

//namespace KouXiaGu.World2D
//{

//    /// <summary>
//    /// 地貌预制构建方法;
//    /// </summary>
//    public class TopographyIO : IMapNodeIO<TopographyNode>
//    {
//        public TopographyIO() { }

//        public TopographyIO(
//            IMap<IntVector2, WorldNode> readOnlyWorldMap,
//            Dictionary<int, TopographyPrefab> topographyPrefabDictionary)
//        {
//            this.ReadOnlyWorldMap = readOnlyWorldMap;
//            this.TopographyPrefabDictionary = topographyPrefabDictionary;
//        }

//        /// <summary>
//        /// 只读的世界地图信息;
//        /// </summary>
//        public IMap<IntVector2, WorldNode> ReadOnlyWorldMap { get; set; }
//        /// <summary>
//        /// 在其上构建的预制物体信息;
//        /// </summary>
//        public Dictionary<int, TopographyPrefab> TopographyPrefabDictionary { get; set; }


//        public Block<TopographyNode> Load(ShortVector2 address)
//        {
//            IEnumerable<IntVector2> mapPoints = BlockMap.GetBlockRange(address);
//            Block<TopographyNode> block = InitializationBlock(mapPoints);
//            return block;
//        }

//        Block<TopographyNode> InitializationBlock(IEnumerable<IntVector2> mapPoints)
//        {
//            Block<TopographyNode> block = new Block<TopographyNode>();
//            TopographyNode topographyNode;
//            foreach (var mapPoint in mapPoints)
//            {
//                topographyNode = InitializationNode(mapPoint);
//                AddNodeToBlock(block, mapPoint, topographyNode);
//            }
//            return block;
//        }

//        /// <summary>
//        /// 加入这个点到地图块对应的位置;
//        /// </summary>
//        void AddNodeToBlock(Block<TopographyNode> block, IntVector2 mapPoint, TopographyNode topographyNode)
//        {
//            ShortVector2 addressPoint;
//            BlockMap.PlanePointToAddress(mapPoint, out addressPoint);
//            block.Add(addressPoint, topographyNode);
//        }

//        /// <summary>
//        /// 初始化这个点的信息;
//        /// </summary>
//        TopographyNode InitializationNode(IntVector2 mapPoint)
//        {
//            WorldNode worldNode;
//            if (ReadOnlyWorldMap.TryGetValue(mapPoint, out worldNode))
//            {
//                int topographyID = worldNode.Topography;
//                Transform topographyPrefab = GetTopographyPrefab(topographyID);
//                Vector2 planePoint = WorldConvert.MapToHex(mapPoint);

//                Transform topographyObject = Instantiate(topographyPrefab, planePoint);
//                TopographyNode topographyNode = new TopographyNode(topographyID, topographyObject);
//                return topographyNode;
//            }
//            return GetDefaultTopographyNode(mapPoint);
//        }

//        TopographyNode GetDefaultTopographyNode(IntVector2 mapPoint)
//        {
//            Transform topographyPrefab = GetTopographyPrefab(0);
//            Vector2 planePoint = WorldConvert.MapToHex(mapPoint);
//            Transform topographyObject = Instantiate(topographyPrefab, planePoint);
//            TopographyNode topographyNode = new TopographyNode(0, topographyObject);
//            return topographyNode;
//        }

//        /// <summary>
//        /// 获取到预制的地貌预制;
//        /// </summary>
//        Transform GetTopographyPrefab(int topographyID)
//        {
//            return TopographyPrefabDictionary[topographyID].prefab;
//        }

//        /// <summary>
//        /// 卸载这个块的所有资源;
//        /// </summary>
//        public void Unload(ShortVector2 address, Block<TopographyNode> block)
//        {
//            foreach (var node in block.Nodes)
//            {
//                Destroy(node.topographyObject);
//            }
//        }


//        public TopographyNode Load(IntVector2 mapPoint)
//        {
//            throw new NotImplementedException();
//        }

//        public void Unload(IntVector2 mapPoint)
//        {
//            throw new NotImplementedException();
//        }

//        Transform Instantiate(Transform topographyPrefab, Vector2 position)
//        {
//            Transform topographyObject = GameObject.Instantiate(topographyPrefab, position, Quaternion.identity) as Transform;
//            return topographyObject;
//        }

//        void Destroy(Transform topographyObject)
//        {
//            GameObject.Destroy(topographyObject.gameObject);
//        }


//    }

//}
