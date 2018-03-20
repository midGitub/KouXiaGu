using JiongXiaGu.Grids;
using JiongXiaGu.Unity.Maps;
using JiongXiaGu.Unity.Scenarios;
using System;

namespace JiongXiaGu.Unity.RunTime
{

    /// <summary>
    /// 游戏世界资源;
    /// </summary>
    public class WorldResource
    {
        /// <summary>
        /// 游戏剧情信息;
        /// </summary>
        public static ScenarioDescription ScenarioDescription { get; private set; }

        /// <summary>
        /// 游戏地图;
        /// </summary>
        public WorldMap<RectCoord> Map { get; set; }

        public WorldResource(ScenarioResource resource)
        {
            ScenarioDescription = resource.Description;
            Map = new WorldMap<RectCoord>(resource.Map.Dictionary);
        }

        public WorldResource(ScenarioResource resource, ArchivedResource archived)
        {
            ScenarioDescription = resource.Description;
            Map = new WorldMap<RectCoord>(resource.Map.Dictionary, archived.Map.Dictionary);
        }

        /// <summary>
        /// 获取到存档;
        /// </summary>
        public ArchivedResource GetArchivedResource()
        {
            throw new NotImplementedException();
        }
    }
}
