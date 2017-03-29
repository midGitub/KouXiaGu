using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KouXiaGu.World
{

    public struct WorldInfo
    {

        /// <summary>
        /// 开始时间;
        /// </summary>
        public DateTime StartTime { get; set; }

        /// <summary>
        /// 人口每日增长比例;
        /// </summary>
        public float ProportionOfDailyGrowth { get; set; }

    }

}
