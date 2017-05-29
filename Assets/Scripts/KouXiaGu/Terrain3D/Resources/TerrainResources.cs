using System;
using System.Collections.Generic;

namespace KouXiaGu.Terrain3D
{

    public class TerrainResources
    {
        public IDictionary<int, LandformInfo> Landform { get; internal set; }
        public IDictionary<int, BuildingInfo> Building { get; internal set; }
        public IDictionary<int, RoadInfo> Road { get; internal set; }
        public LandformTag Tags { get; internal set; }
    }
}
