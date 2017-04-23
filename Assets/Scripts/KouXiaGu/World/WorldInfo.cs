using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KouXiaGu.World.Map;

namespace KouXiaGu.World
{

    public class WorldInfoReader : AsyncOperation<WorldInfo>
    {
        public WorldInfoReader(WorldInfo info)
        {
            OnCompleted(info);
        }
    }

    /// <summary>
    /// 提供初始化的世界信息;
    /// </summary>
    [Serializable]
    public class WorldInfo
    {

        public WorldTimeInfo Time;
        public ArchiveFile Archive;




        /// <summary>
        /// 人口每日增长比例;
        /// </summary>
        [Obsolete]
        public float ProportionOfDailyGrowth { get; set; }

    }

}
