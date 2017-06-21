using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KouXiaGu.World.HumanGeography
{


    public class Town
    {
        public Town(int id, IWorld world)
        {
            ID = id;
            Range = new TownRange(id, world.WorldData.MapData.Data);
        }

        public int ID { get; private set; }
        public TownRange Range { get; private set; }
    }
}
