using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JiongXiaGu.Unity.Resources;

namespace JiongXiaGu.Unity.RectTerrain
{

    /// <summary>
    /// 地形资源描述;
    /// </summary>
    public class RectTerrainDescription
    {
        public IReadOnlyDictionary<string, Description<LandformDescription>> Landform { get; private set; }
        public IReadOnlyDictionary<string, Description<BuildingDescription>> Building { get; private set; }
        public IReadOnlyDictionary<string, Description<RoadDescription>> Road { get; private set; }
    }
}
