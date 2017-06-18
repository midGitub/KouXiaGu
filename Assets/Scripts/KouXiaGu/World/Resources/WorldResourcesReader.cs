using KouXiaGu.Concurrent;
using KouXiaGu.Terrain3D;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KouXiaGu.World.Resources
{


    public class WorldResourcesReader
    {
        public WorldResourcesReader()
        {
            dispatcher = GameResourceUnityDispatcher.Instance;
        }

        GameResourceUnityDispatcher dispatcher;
        internal LandformInfoXmlSerializer LandformFileSerializer = new LandformInfoXmlSerializer();
        internal BuildingInfoXmlSerializer BuildingFileSerializer = new BuildingInfoXmlSerializer();
        internal RoadInfoXmlSerializer RoadFileSerializer = new RoadInfoXmlSerializer();
        internal TerrainResourcesReader TerrainResourcesReader = new TerrainResourcesReader();

        public WorldResources Read(IOperationState state)
        {
            var landformInfos = LandformFileSerializer.Read();
            var buildingInfos = BuildingFileSerializer.Read();
            var roadInfos = RoadFileSerializer.Read();

            WorldResources result = new WorldResources()
            {
                Landform = landformInfos,
                Building = buildingInfos,
                Road = roadInfos,
            };

            TerrainResourcesReader.Read(dispatcher, result, state);
            return result;
        }
    }
}
