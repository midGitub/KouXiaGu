using JiongXiaGu.Resources;
using JiongXiaGu.World.TimeSystem;
using JiongXiaGu.World.Map;

namespace JiongXiaGu.World
{

    /// <summary>
    /// 提供初始化的世界信息;
    /// </summary>
    public class WorldInfo
    {
        public IReader<WorldMap, IGameResource> MapReader { get; set; }
        public IReader<WorldTime> TimeReader { get; set; }
    }
}
