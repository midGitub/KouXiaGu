using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JiongXiaGu.World;
using JiongXiaGu.World.Map;

namespace JiongXiaGu.Terrain3D.MapEditor
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
