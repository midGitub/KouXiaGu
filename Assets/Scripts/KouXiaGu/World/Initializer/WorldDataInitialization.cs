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

            Initialize(basicData);
        }

        public GameMap MapData { get; private set; }

        void Initialize(IBasicData basicData)
        {
            WorldInfo info = basicData.WorldInfo;
            MapData = info.MapReader.Read(basicData.BasicResource);
        }
    }
}
