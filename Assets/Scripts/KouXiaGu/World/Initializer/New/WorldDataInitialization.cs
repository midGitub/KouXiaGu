using KouXiaGu.World.Map;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KouXiaGu.World
{

    public class WorldDataInitialization : IWorldData
    {
        public WorldDataInitialization(IBasicData basicData)
        {
            if (basicData == null)
                throw new ArgumentNullException("basicData");

            BasicData = basicData;
        }

        public IBasicData BasicData { get; private set; }
        public GameMap MapData { get; private set; }
        public TimeManager Time { get; private set; }

        void Initialize(IBasicData basicData)
        {
            MapData = basicData.WorldInfo.MapReader.Read(BasicData.BasicResource);
        }
    }
}
