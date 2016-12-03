using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KouXiaGu.HexTerrain
{

    /// <summary>
    /// 保存当前游戏地形数据;
    /// </summary>
    public sealed class TerrainData
    {

        Map2D<TerrainNode> terrainMap;


        /// <summary>
        /// 获取到烘焙地图节点;
        /// </summary>
        IEnumerable<BakingNode> GetBakingNeighbors(ShortVector2 mapPoint)
        {
            throw new NotImplementedException();
        }

    }

}
