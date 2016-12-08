using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace KouXiaGu.HexTerrain
{

    /// <summary>
    /// 地形烘焙请求;
    /// 烘焙的为一个正六边形网格内的区域;
    /// </summary>
    public struct BakingRequest
    {

        public BakingRequest(IReadOnlyMap2D<CubicHexCoord, LandformNode> map, ShortVector2 blockCoord)
        {
            this.Map = map;
            this.BlockCoord = blockCoord;
        }

        public IReadOnlyMap2D<CubicHexCoord, LandformNode> Map { get; set; }

        /// <summary>
        /// 请求烘焙的地图块位置;
        /// </summary>
        public ShortVector2 BlockCoord { get; set; }

        /// <summary>
        /// 摄像机位置;
        /// </summary>
        public Vector3 CameraPosition
        {
            get { return TerrainBlock.BlockCoordToBlockCenter(BlockCoord) + new Vector3(0, 5, 0); }
        }

        /// <summary>
        /// 获取到这次需要烘焙的所有节点;
        /// </summary>
        public IEnumerable<BakingNode> GetBakingNodes()
        {
            IEnumerable<CubicHexCoord> cover = TerrainBlock.GetBlockCover(BlockCoord);
            LandformNode node;

            float index = -2;

            foreach (var coord in cover)
            {
                Vector3 pixPoint = HexGrids.HexToPixel(coord, index--);
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
        /// 当完成后调用;
        /// </summary>
        public void OnComplete(Texture2D diffuse, Texture2D height)
        {
            TerrainBlock.Create(BlockCoord, diffuse, height);
        }

    }

}
