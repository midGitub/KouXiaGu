using System;
using System.Collections.Generic;


namespace KouXiaGu.Resources
{

    public class BasicTerrainResource
    {
        public Dictionary<int, RoadInfo> Road { get; internal set; }
        public Dictionary<int, LandformInfo> Landform { get; internal set; }
        public Dictionary<int, BuildingInfo> Building { get; internal set; }
    }
}
