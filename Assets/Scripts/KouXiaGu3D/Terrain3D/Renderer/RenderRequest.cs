using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using KouXiaGu.Grids;

namespace KouXiaGu.Terrain3D
{


    /// <summary>
    /// 地形烘焙请求;
    /// 烘焙的为一个正六边形网格内的区域;
    /// </summary>
    public struct RenderRequest
    {

        public RenderRequest(IMap<CubicHexCoord, TerrainNode> map, RectCoord blockCoord) : this()
        {
            this.Map = map;
            this.BlockCoord = blockCoord;
            this.BakingNodes = GetBakingNodes().ToArray();

            if (BakingNodes.Length == 0)
                throw new IndexOutOfRangeException("这个块坐标已经超出了地图的定义;");
        }

        /// <summary>
        /// 地形地图;
        /// </summary>
        public IMap<CubicHexCoord, TerrainNode> Map { get; private set; }

        /// <summary>
        /// 请求烘焙的地图块位置;
        /// </summary>
        public RectCoord BlockCoord { get; private set; }

        /// <summary>
        /// 范围内请求烘焙的节点;
        /// </summary>
        public BakingNode[] BakingNodes { get; set; }


        /// <summary>
        /// 摄像机位置;
        /// </summary>
        public Vector3 CameraPosition
        {
            get { return TerrainData.ChunkGrid.GetCenter(BlockCoord) + new Vector3(0, 5, 0); }
        }

        /// <summary>
        /// 获取到这次需要烘焙的所有节点;
        /// </summary>
        IEnumerable<BakingNode> GetBakingNodes()
        {
            IEnumerable<CubicHexCoord> cover = TerrainData.GetCover(BlockCoord);
            TerrainNode node;

            foreach (var coord in cover)
            {
                Vector3 pixPoint = GridConvert.Grid.GetPixel(coord);
                if (Map.TryGetValue(coord, out node))
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
        public void BasicTextureComplete(Texture2D diffuse, Texture2D height)
        {
            TerrainData.Create(BlockCoord, diffuse, height);
        }

    }

}
