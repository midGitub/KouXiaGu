using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KouXiaGu.World.Map;

namespace KouXiaGu.World
{

    /// <summary>
    /// 提供初始化的世界信息;
    /// </summary>
    public class WorldInfo
    {
        /// <summary>
        /// 时间;
        /// </summary>
        public WorldTimeInfo Time { get; set; }


        public ArchiveWorldInfo ArchiveInfo { get; set; }

        public bool IsInitializeFromArchive
        {
            get { return ArchiveInfo != null; }
        }


        /// <summary>
        /// 人口每日增长比例;
        /// </summary>
        public float ProportionOfDailyGrowth { get; set; }

    }

}
