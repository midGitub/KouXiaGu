//using System;
//using System.Collections;
//using System.Collections.Generic;
//using System.Linq;

//namespace KouXiaGu.World.Map
//{

//    public sealed class MapManager
//    {
//        static MapManager()
//        {
//            MapReader = new MapDataReaderOrEmpty();
//        }

//        internal static IReader<MapData> MapReader { get; set; }


//        public MapManager()
//        {
//            Initialize();
//        }

//        public MapData Map { get; private set; }

//        void Initialize()
//        {
//            Map = MapReader.Read();
//        }


//        public static ThreadOperation<MapManager> CreateAsync()
//        {
//            var item = new AsyncInitializer();
//            item.Start();
//            return item;
//        }

//        class AsyncInitializer : ThreadOperation<MapManager>
//        {
//            protected override MapManager Operate()
//            {
//                MapManager item = new MapManager();
//                item.Initialize();
//                return item;
//            }
//        }

//    }

//}
