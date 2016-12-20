using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KouXiaGu.Grids;
using UnityEngine;

namespace KouXiaGu.Terrain3D
{

    public interface IBakeRequest<T>
    {
        Vector3 CameraPosition { get; }
        IEnumerable<T> BakingNodes { get; }
        void TextureComplete(Texture2D diffuse, Texture2D height);
    }

    /// <summary>
    /// 地形烘焙请求;
    /// </summary>
    public struct BakeRequest : IBakeRequest<BakingNode>
    {


        #region 静态

        /// <summary>
        /// 地形地图;
        /// </summary>
        public static IDictionary<CubicHexCoord, TerrainNode> terrainMap
        {
            get { return TerrainController.CurrentMap.Map; }
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
        /// 是否为空的地形块;
        /// </summary>
        static bool IsEmtpyChunk(RectCoord chunkCoord)
        {
            IEnumerable<CubicHexCoord> cover = TerrainData.GetChunkCover(chunkCoord);
            try
            {
                cover.First();
                return false;
            }
            catch (InvalidOperationException)
            {
                return true;
            }
        }


        /// <summary>
        /// 创建地形到场景;
        /// </summary>
        static void Create(RectCoord chunkCoord, Texture2D diffuse, Texture2D height)
        {
            if (!onQueueChunk.Contains(chunkCoord))
            {
                GameObject.Destroy(diffuse);
                GameObject.Destroy(height);
                return;
            }

            TerrainData.Create(chunkCoord, diffuse, height);
        }

        #endregion


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

        public BakeRequest(IDictionary<CubicHexCoord, TerrainNode> map, RectCoord chunkCoord) : this()
        {
            this.ChunkCoord = chunkCoord;
            this.cameraPosition = TerrainData.ChunkGrid.GetCenter(chunkCoord);
            this.bakingNodes = GetBakingNodes(map, chunkCoord).ToArray();

            if (bakingNodes.Length == 0)
                throw new IndexOutOfRangeException("请求渲染地图块:" +chunkCoord + " ;超出了地图的定义;");
        }

        /// <summary>
        /// 获取到这次需要烘焙的所有节点;
        /// </summary>
        IEnumerable<BakingNode> GetBakingNodes(IDictionary<CubicHexCoord, TerrainNode> map, RectCoord blockCoord)
        {
            IEnumerable<CubicHexCoord> cover = TerrainData.GetChunkCover(blockCoord);
            TerrainNode node;
            Vector3 pixPoint;

            foreach (var coord in cover)
            {
                pixPoint = GridConvert.Grid.GetPixel(coord);
                if (map.TryGetValue(coord, out node))
                {
                    yield return new BakingNode(pixPoint, node);
                }
            }
        }

        /// <summary>
        /// 基本的贴图烘焙完毕时调用;
        /// </summary>
        void IBakeRequest<BakingNode>.TextureComplete(Texture2D diffuse, Texture2D height)
        {
            TerrainData.Create(ChunkCoord, diffuse, height);
        }


    }

}
