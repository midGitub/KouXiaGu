using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KouXiaGu.Grids;

namespace KouXiaGu.WorldEvents
{

    /// <summary>
    /// 城镇信息管理;
    /// </summary>
    public class TownManager
    {


        class TownInfo
        {
            /// <summary>
            /// 城镇信息;
            /// </summary>
            Town town;

            /// <summary>
            /// 城镇坐标;
            /// </summary>
            CubicHexCoord coord;

            /// <summary>
            /// 标识;
            /// </summary>
            public int ID
            {
                get { return town.ID; }
            }



        }

    }

}
