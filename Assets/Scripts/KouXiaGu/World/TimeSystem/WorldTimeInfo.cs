using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KouXiaGu.Resources;
using System.Globalization;

namespace KouXiaGu.World.TimeSystem
{

    public class WorldTimeInfo : IReader<WorldTime>
    {
        public WorldDateTime Start { get; set; }
        public WorldDateTime Current { get; set; }

        WorldTime IReader<WorldTime>.Read()
        {
            return new WorldTime()
            {
                StartTime = Start,
                CurrentTime = Current,
            };
        }
    }
}
