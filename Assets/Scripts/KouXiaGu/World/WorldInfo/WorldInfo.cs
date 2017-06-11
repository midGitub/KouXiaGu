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
    public class WorldInfo
    {
        public IGameMapReader MapReader { get; set; }
        public WorldTimeInfo TimeInfo { get; set; }
    }
}
