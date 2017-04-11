using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace KouXiaGu.World.Map
{


    public class MapEditor
    {

        static IReaderWriter<Dictionary<int, RoadInfo>, RoadInfo[]> RoadReader
        {
            get { return MapManager.RoadReader; }
        }

        static IReaderWriter<Dictionary<int, LandformInfo>, LandformInfo[]> LandformReader
        {
            get { return MapManager.LandformReader; }
        }

        public MapManager Manager { get; private set; }

        public MapEditor()
        {
            Manager = new MapManager();
        }

        public MapEditor(MapManager manager)
        {
            Manager = manager;
        }

    }

}
