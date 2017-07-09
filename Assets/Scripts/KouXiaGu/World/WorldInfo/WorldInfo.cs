using KouXiaGu.World.Map;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KouXiaGu.Resources;
using KouXiaGu.World.TimeSystem;

namespace KouXiaGu.World
{
    /// <summary>
    /// 提供初始化的世界信息;
    /// </summary>
    public class WorldInfo
    {
        public IGameMapReader MapReader { get; set; }
        public IReader<WorldTime> TimeInfo { get; set; }
    }
}
