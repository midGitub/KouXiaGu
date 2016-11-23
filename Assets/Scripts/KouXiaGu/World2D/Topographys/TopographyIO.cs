using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KouXiaGu.World2D.Map;
using UnityEngine;

namespace KouXiaGu.World2D
{

    /// <summary>
    /// 地貌预制构建方法;
    /// </summary>
    public class TopographyIO : IMapBlockIO<Block<TopographyNode>>
    {
        public TopographyIO() { }

        public TopographyIO(
            BlockMap<Block<TopographyNode>> blockMap, 
            IMap<IntVector2, WorldNode> readOnlyWorldMap,
            Dictionary<int, TopographyPrefab> topographyPrefabDictionary)
        {
            this.BlockMap = blockMap;
            this.ReadOnlyWorldMap = readOnlyWorldMap;
            this.TopographyPrefabDictionary = topographyPrefabDictionary;
        }

        /// <summary>
        /// 保存块信息的地图;
        /// </summary>
        public BlockMap<Block<TopographyNode>> BlockMap { get; set; }
        /// <summary>
        /// 只读的世界地图信息;
        /// </summary>
        public IMap<IntVector2, WorldNode> ReadOnlyWorldMap { get; set; }
        /// <summary>
        /// 在其上构建的预制物体信息;
        /// </summary>
        public Dictionary<int, TopographyPrefab> TopographyPrefabDictionary { get; set; }


        public Block<TopographyNode> Load(ShortVector2 address)
        {
            IEnumerable<IntVector2> points = BlockMap.GetBlockRange(address);
            Block<TopographyNode> block = InitializationBlock(points);
            return block;
        }

        Block<TopographyNode> InitializationBlock(IEnumerable<IntVector2> mapPoints)
        {
            Block<TopographyNode> block = new Block<TopographyNode>();
            TopographyNode topographyNode;
            foreach (var mapPoint in mapPoints)
            {
                topographyNode = InitializationNode(mapPoint);
                AddNodeToBlock(block, mapPoint, topographyNode);
            }
            return block;
        }

        /// <summary>
        /// 加入这个点到地图块对应的位置;
        /// </summary>
        void AddNodeToBlock(Block<TopographyNode> block, IntVector2 mapPoint, TopographyNode topographyNode)
        {
            ShortVector2 addressPoint;
            BlockMap.PlanePointToAddress(mapPoint, out addressPoint);
            block.Add(addressPoint, topographyNode);
        }

        /// <summary>
        /// 初始化这个点的信息;
        /// </summary>
        TopographyNode InitializationNode(IntVector2 mapPoint)
        {
            WorldNode worldNode = ReadOnlyWorldMap[mapPoint];

            int topographyID = worldNode.Topography;
            Transform topographyPrefab = GetTopographyPrefab(topographyID);
            Vector2 planePoint = WorldConvert.MapToHex(mapPoint);

            Transform topographyObject = Instantiate(topographyPrefab, planePoint);
            TopographyNode topographyNode = new TopographyNode(topographyID, topographyObject);
            return topographyNode;
        }

        /// <summary>
        /// 获取到预制的地貌预制;
        /// </summary>
        Transform GetTopographyPrefab(int topographyID)
        {
            return TopographyPrefabDictionary[topographyID].prefab;
        }

        /// <summary>
        /// 卸载这个块的所有资源;
        /// </summary>
        public void Unload(ShortVector2 address, Block<TopographyNode> block)
        {
            foreach (var node in block.Nodes)
            {
                Destroy(node.topographyObject);
            }
        }


        Transform Instantiate(Transform topographyPrefab, Vector2 position)
        {
            Transform topographyObject = GameObject.Instantiate(topographyPrefab, position, Quaternion.identity) as Transform;
            return topographyObject;
        }

        void Destroy(Transform topographyObject)
        {
            GameObject.Destroy(topographyObject.gameObject);
        }

    }

}
