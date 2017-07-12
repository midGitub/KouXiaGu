using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KouXiaGu.World;
using KouXiaGu.World.Map;

namespace KouXiaGu.Terrain3D.MapEditor
{


    public class Editor
    {
        public Editor(IGameResource basicResource, WorldMap map)
        {
            BasicResource = basicResource;
            Map = map;
        }

        public IGameResource BasicResource { get; private set; }
        public WorldMap Map { get; private set; }


    }
}
