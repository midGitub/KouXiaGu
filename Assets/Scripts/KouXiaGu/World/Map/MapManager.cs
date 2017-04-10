using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;


namespace KouXiaGu.World.Map
{

    public sealed class MapManager
    {
        internal static IReader<Data> DataReader { get; set; }
        internal static IReader<Dictionary<int, RoadInfo>> RoadReader { get; set; }
        internal static IReader<Dictionary<int, LandformInfo>> LandformReader { get; set; }

        public Data Map { get; private set; }
        public Dictionary<int, RoadInfo> RoadInfos { get; private set; }
        public Dictionary<int, LandformInfo> LandformInfos { get; private set; }

        static MapManager()
        {
            RoadReader = new RoadInfoXmlReader();
            LandformReader = new LandformInfoXmlSerializer();
        }

        public MapManager()
        {
            Initialize();
        }

        /// <summary>
        /// 初始化;
        /// </summary>
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
