using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace KouXiaGu.World.Map
{

    public sealed class MapManager
    {
        static MapManager()
        {
            DataReader = new MapDataReader();
            RoadReader = new RoadInfoXmlSerializer();
            LandformReader = new LandformInfoXmlSerializer();
        }

        internal static IReader<MapData> DataReader { get; set; }
        internal static DataReader<Dictionary<int, RoadInfo>, RoadInfo[]> RoadReader { get; set; }
        internal static DataReader<Dictionary<int, LandformInfo>, LandformInfo[]> LandformReader { get; set; }


        public MapManager()
        {
            Initialize();
        }

        public MapData Map { get; private set; }
        public Dictionary<int, RoadInfo> RoadInfos { get; private set; }
        public Dictionary<int, LandformInfo> LandformInfos { get; private set; }

        void Initialize()
        {
            Map = DataReader.Read();
            RoadInfos = RoadReader.Read();
            LandformInfos = LandformReader.Read();
        }


        public void WriteInfosToMainDirectory()
        {
            WriteInfosToDirectory(GameFile.MainDirectory);
        }

        public void WriteInfosToDirectory(string dirPath)
        {
            var roadInfos = RoadInfos.Values.ToArray();
            RoadReader.WriteToDirectory(roadInfos, dirPath);

            var landformInfos = LandformInfos.Values.ToArray();
            LandformReader.WriteToDirectory(landformInfos, dirPath);
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
