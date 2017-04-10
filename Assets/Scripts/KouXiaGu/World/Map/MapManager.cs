using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace KouXiaGu.World.Map
{

    public sealed class MapManager
    {
        internal static IReader<Data> DataReader { get; set; }
        internal static IReaderWriter<Dictionary<int, RoadInfo>, RoadInfo[]> RoadReader { get; set; }
        internal static IReaderWriter<Dictionary<int, LandformInfo>, LandformInfo[]> LandformReader { get; set; }

        static MapManager()
        {
            DataReader = new DataReader();
            RoadReader = new RoadInfoXmlSerializer();
            LandformReader = new LandformInfoXmlSerializer();
        }


        public Data Map { get; private set; }
        public Dictionary<int, RoadInfo> RoadInfos { get; private set; }
        public Dictionary<int, LandformInfo> LandformInfos { get; private set; }

        public MapManager()
        {
            Initialize();
        }

        void Initialize()
        {
            Map = DataReader.Read();
            RoadInfos = RoadReader.Read();
            LandformInfos = LandformReader.Read();
        }


        public static AsyncOperation<MapManager> CreateAsync()
        {
            var item = new AsyncInitializer();
            item.Start();
            return item;
        }

        class AsyncInitializer : AsyncOperation<MapManager>
        {
            protected override MapManager Operate()
            {
                MapManager item = new MapManager();
                item.Initialize();
                return item;
            }
        }

    }

}
