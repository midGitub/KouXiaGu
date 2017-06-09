using KouXiaGu.World.Map;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KouXiaGu.World
{
    /// <summary>
    /// 提供初始化的世界信息;
    /// </summary>
    [Serializable]
    public class WorldInfo
    {

        public ArchiveFile Archive;
        public IGameMapReader MapReader;

        /// <summary>
        /// 人口每日增长比例;
        /// </summary>
        [Obsolete]
        public float ProportionOfDailyGrowth { get; set; }

    }

}
