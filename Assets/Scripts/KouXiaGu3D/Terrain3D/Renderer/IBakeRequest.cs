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
