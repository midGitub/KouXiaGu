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
            MapReader = new MapDataReader();
        }

        internal static IReader<MapData> MapReader { get; set; }


        public MapManager()
        {
            Initialize();
        }

        public MapData Map { get; private set; }

        void Initialize()
        {
            Map = MapReader.Read();
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
