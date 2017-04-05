using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KouXiaGu.World.Map;

namespace KouXiaGu.World
{

    public struct WorldInfo
    {

        /// <summary>
        /// 时间;
        /// </summary>
        public WorldTimeInfo Time { get; set; }

        /// <summary>
        /// 使用的地图信息;
        /// </summary>
        public MapInfo Map { get; set; }

        /// <summary>
        /// 人口每日增长比例;
        /// </summary>
        public float ProportionOfDailyGrowth { get; set; }

    }

}
