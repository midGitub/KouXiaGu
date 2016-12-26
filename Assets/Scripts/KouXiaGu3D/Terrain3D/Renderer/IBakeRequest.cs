using System;
using System.Collections.Generic;
using KouXiaGu.Grids;
using UnityEngine;

namespace KouXiaGu.Terrain3D
{

    /// <summary>
    /// 地形烘焙请求;
    /// </summary>
    public interface IBakeRequest
    {
        /// <summary>
        /// 地图块的坐标
        /// </summary>
        RectCoord ChunkCoord { get; }

        /// <summary>
        /// 地形使用的地图;
        /// </summary>
        IDictionary<CubicHexCoord, TerrainNode> Map { get; }

        /// <summary>
        /// 贴图烘焙完成;
        /// </summary>
        void OnComplete(Texture2D diffuse, Texture2D height, Texture2D normal);

        /// <summary>
        /// 出现错误时调用;
        /// </summary>
        void OnError(Exception ex);
    }

}
