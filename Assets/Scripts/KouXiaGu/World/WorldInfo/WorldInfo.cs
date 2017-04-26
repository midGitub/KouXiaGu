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

        public WorldInfoReader(ArchiveFile archive)
        {

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
        public MapResourceReader MapReader;

        /// <summary>
        /// 人口每日增长比例;
        /// </summary>
        [Obsolete]
        public float ProportionOfDailyGrowth { get; set; }

    }

}
