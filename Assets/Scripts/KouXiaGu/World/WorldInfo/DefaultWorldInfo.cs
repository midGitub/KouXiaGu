using KouXiaGu.World.Map;
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
                //MapReader = new RandomGameMapCreater(50),
                MapReader = new MapDataReader(),
                TimeInfo = new WorldTimeInfo()
                {
                    CurrentTime = new DateTime(),
                },
            };
            OnCompleted(worldInfo);
        }
    }
}
