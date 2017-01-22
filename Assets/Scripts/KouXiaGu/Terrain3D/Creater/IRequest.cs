using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KouXiaGu.Grids;

namespace KouXiaGu.Terrain3D
{


    public interface IRequest
    {

        /// <summary>
        /// 地图块的坐标
        /// </summary>
        RectCoord ChunkCoord { get; }

        /// <summary>
        /// 地形使用的地图;
        /// </summary>
        MapData Data { get; }

    }

}
