using KouXiaGu.World.Map;
using KouXiaGu.World.TimeSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KouXiaGu.World
{


    public class DefaultWorldInfo : AsyncOperation<WorldInfo>
    {
        public DefaultWorldInfo()
        {
            Start();
        }

        void Start()
        {
            WorldInfo worldInfo = new WorldInfo()
            {
                MapReader = new WorldMapReader(),
                TimeReader = new WorldTimeReader(),
            };
            OnCompleted(worldInfo);
        }
    }
}
