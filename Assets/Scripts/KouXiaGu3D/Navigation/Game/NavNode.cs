using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KouXiaGu.Terrain3D;
using KouXiaGu.Grids;

namespace KouXiaGu.Navigation.Game
{

    /// <summary>
    /// 导航点;
    /// </summary>
    public struct NavNode
    {

        /// <summary>
        /// 位于世界的位置;
        /// </summary>
        public CubicHexCoord Position { get; private set; }

        /// <summary>
        /// 地形信息;
        /// </summary>
        public TerrainNode Terrain { get; private set; }



    }

}
