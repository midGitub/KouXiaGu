using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using KouXiaGu.Grids;

namespace KouXiaGu.Terrain3D
{

    /// <summary>
    /// 地形创建到场景;
    /// </summary>
    [DisallowMultipleComponent]
    public sealed class TerrainCreater : MonoBehaviour
    {
        TerrainCreater() { }


        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                Destroy(TerrainData.ChunkGrid.GetCoord(MouseConvert.MouseToPixel()));
            }
        }


        /// <summary>
        /// 地形地图;
        /// </summary>
        public static IMap<CubicHexCoord, TerrainNode> terrainMap
        {
            get { return TerrainMap.Map; }
        }

        /// <summary>
        /// 正在队列等待烘焙的地形块;
        /// </summary>
        static readonly HashSet<RectCoord> onQueueChunk = new HashSet<RectCoord>();

        /// <summary>
        /// 创建地形到场景,若已经在场景则返回false;
        /// </summary>
        public static bool Create(RectCoord chunkCoord)
        {
            if (IsCreated(chunkCoord))
                return false;

            onQueueChunk.Add(chunkCoord);
            BasicRenderer.Enqueue(new BakeRequest(terrainMap, chunkCoord));
            return true;
        }

        /// <summary>
        /// 从场景移除地形,若不存在这个地形则返回false;
        /// </summary>
        public static bool Destroy(RectCoord chunkCoord)
        {
            if (!IsCreated(chunkCoord))
                return false;

            onQueueChunk.Remove(chunkCoord);
            RemoveOnScene(chunkCoord);
            return true;
        }

        /// <summary>
        /// 从场景移除地形;
        /// </summary>
        static void RemoveOnScene(RectCoord chunkCoord)
        {
            TerrainData.Destroy(chunkCoord);
        }

        /// <summary>
        /// 是否已经创建到场景?
        /// </summary>
        static bool IsCreated(RectCoord chunkCoord)
        {
            return TerrainData.Contains(chunkCoord) || onQueueChunk.Contains(chunkCoord);
        }

        /// <summary>
        /// 创建地形到场景;
        /// </summary>
        static void Create(RectCoord chunkCoord, Texture2D diffuse, Texture2D height)
        {
            if (!onQueueChunk.Contains(chunkCoord))
            {
                Destroy(diffuse);
                Destroy(height);
                return;
            }

            TerrainData.Create(chunkCoord, diffuse, height);
        }


        struct BakeRequest : IBakeRequest<BakingNode>
        {

            BakingNode[] bakingNodes;
            Vector3 cameraPosition;

            public RectCoord ChunkCoord { get; private set; }

            /// <summary>
            /// 范围内请求烘焙的节点;
            /// </summary>
            IEnumerable<BakingNode> IBakeRequest<BakingNode>.BakingNodes
            {
                get { return bakingNodes; }
            }

            /// <summary>
            /// 摄像机位置;
            /// </summary>
            Vector3 IBakeRequest<BakingNode>.CameraPosition
            {
                get { return cameraPosition; }
            }

            public BakeRequest(IMap<CubicHexCoord, TerrainNode> map, RectCoord chunkCoord) : this()
            {
                this.ChunkCoord = chunkCoord;
                this.cameraPosition = TerrainData.ChunkGrid.GetCenter(chunkCoord);
                this.bakingNodes = GetBakingNodes(map, chunkCoord).ToArray();

                if (bakingNodes.Length == 0)
                    throw new IndexOutOfRangeException("这个块坐标已经超出了地图的定义;");
            }

            /// <summary>
            /// 获取到这次需要烘焙的所有节点;
            /// </summary>
            IEnumerable<BakingNode> GetBakingNodes(IMap<CubicHexCoord, TerrainNode> map, RectCoord blockCoord)
            {
                IEnumerable<CubicHexCoord> cover = TerrainData.GetCover(blockCoord);
                TerrainNode node;
                Vector3 pixPoint;

                foreach (var coord in cover)
                {
                    pixPoint = GridConvert.Grid.GetPixel(coord);
                    if (map.TryGetValue(coord, out node))
                    {
                        yield return new BakingNode(pixPoint, node);
                    }
                    else
                    {
                        yield return default(BakingNode);
                    }
                }
            }

            /// <summary>
            /// 基本的贴图烘焙完毕时调用;
            /// </summary>
            void IBakeRequest<BakingNode>.TextureComplete(Texture2D diffuse, Texture2D height)
            {
                TerrainCreater.Create(ChunkCoord, diffuse, height);
            }

        }

    }

}
