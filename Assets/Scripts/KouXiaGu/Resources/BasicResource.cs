using KouXiaGu.Navigation;
using KouXiaGu.Terrain3D;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KouXiaGu.Resources
{

    /// <summary>
    /// 游戏基本资源;
    /// </summary>
    public class BasicResource
    {
        public BasicTerrainResource BasicTerrain { get; internal set; }
        public TerrainResource Terrain { get; internal set; }
        public NavigationResource Navigation { get; internal set; }
    }
}
