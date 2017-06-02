using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KouXiaGu.Terrain3D;

namespace KouXiaGu.World
{


    public class WorldComponentInitialization : IWorldComponents
    {
        public WorldComponentInitialization(IBasicData basicData, IWorldData worldData)
        {
            if (basicData == null)
                throw new ArgumentNullException("basicData");
            if (worldData == null)
                throw new ArgumentNullException("worldData");

            Initialize(basicData, worldData);
        }

        public Landform Landform { get; private set; }

        void Initialize(IBasicData basicData, IWorldData worldData)
        {
            Landform = new Landform();
        }
    }
}
