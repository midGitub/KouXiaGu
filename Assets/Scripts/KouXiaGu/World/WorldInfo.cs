using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KouXiaGu.World
{

    public struct WorldTimeInfo
    {
        /// <summary>
        /// 开始时间;
        /// </summary>
        public DateTime StartTime { get; set; }

        /// <summary>
        /// 当前时间;
        /// </summary>
        public DateTime CurrentTime { get; set; }

    }

    public struct WorldInfo
    {

        /// <summary>
        /// 时间;
        /// </summary>
        public WorldTimeInfo Time { get; set; }

        /// <summary>
        /// 人口每日增长比例;
        /// </summary>
        public float ProportionOfDailyGrowth { get; set; }

    }

}
