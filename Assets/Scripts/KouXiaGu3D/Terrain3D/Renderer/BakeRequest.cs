using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KouXiaGu.Grids;
using UnityEngine;

namespace KouXiaGu.Terrain3D
{

    /// <summary>
    /// 地形烘焙请求;
    /// </summary>
    public class BakeRequest : IEquatable<BakeRequest>, IBakeRequest
    {

        /// <summary>
        /// 地图块坐标;
        /// </summary>
        public RectCoord ChunkCoord { get; private set; }

        public IDictionary<CubicHexCoord, TerrainNode> Map { get; private set; }

        public BakeRequest(IDictionary<CubicHexCoord, TerrainNode> map, RectCoord chunkCoord)
        {
            this.ChunkCoord = chunkCoord;
            this.Map = map;
        }

        public bool Equals(BakeRequest other)
        {
            return ChunkCoord == other.ChunkCoord;
        }

        /// <summary>
        /// 基本的贴图烘焙完毕时调用;
        /// </summary>
        public void OnComplete(Texture2D diffuse, Texture2D height, Texture2D normal)
        {
            TerrainChunk.Create(ChunkCoord, diffuse, height, normal);
        }

        /// <summary>
        /// 当烘焙时出现出错调用;
        /// </summary>
        public void OnError(Exception ex)
        {

        }

    }

}
