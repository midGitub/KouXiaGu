using KouXiaGu.Resources;
using KouXiaGu.World.TimeSystem;
using KouXiaGu.World.Map;

namespace KouXiaGu.World
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
