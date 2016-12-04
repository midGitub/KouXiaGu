using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KouXiaGu.HexTerrain
{

    /// <summary>
    /// 保存当前游戏地形数据;
    /// </summary>
    public sealed class TerrainData : UnitySingleton<TerrainData>
    {

        Map2D<LandformNode> terrainMap;

        /// <summary>
        /// 获取到烘焙地图节点;
        /// </summary>
        IEnumerable<BakingRequest> GetBakingNeighbors(ShortVector2 mapPoint)
        {
            throw new NotImplementedException();
        }

        void Awake()
        {
            terrainMap = new Map2D<LandformNode>();
        }

        public void BakingLandform(ShortVector2 mapPoint)
        {

        }

    }

}
