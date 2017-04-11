using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using KouXiaGu.Terrain3D;

namespace KouXiaGu.World.Map
{


    public class MapDataEditor
    {

        public MapDataEditor()
        {
            Manager = new MapManager();
        }

        public MapDataEditor(MapManager manager)
        {
            Manager = manager;
        }

        public MapManager Manager { get; private set; }
    }

}
