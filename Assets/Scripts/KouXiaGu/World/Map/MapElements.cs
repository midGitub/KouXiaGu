using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using KouXiaGu.Terrain3D;
using UnityEngine;

namespace KouXiaGu.World.Map
{

    public class MapElements
    {
        public static IReader<Dictionary<int, RoadInfo>> RoadReader { get; set; }
        public static IReader<Dictionary<int, LandformInfo>> LandformReader { get; set; }

        public Dictionary<int, RoadInfo> RoadInfos { get; private set; }
        public Dictionary<int, LandformInfo> LandformInfos { get; private set; }

        static MapElements()
        {
            RoadReader = new RoadInfoXmlReader();
            LandformReader = new LandformInfoXmlSerializer();
        }

        public MapElements()
        {
            Initialize();
        }

        void Initialize()
        {
            RoadInfos = RoadReader.Read();
            LandformInfos = LandformReader.Read();
        }

    }

}
