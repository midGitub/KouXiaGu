﻿using KouXiaGu.Terrain3D;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KouXiaGu.World.Resources
{

    public class WorldResources
    {
        public IDictionary<int, LandformInfo> Landform { get; internal set; }
        public IDictionary<int, BuildingInfo> Building { get; internal set; }
        public IDictionary<int, RoadInfo> Road { get; internal set; }
        public IDictionary<int, ProductInfo> Products { get; internal set; }
        public IDictionary<int, TownInfo> Towns { get; internal set; }
        public LandformTagConverter Tags { get; internal set; }
    }
}