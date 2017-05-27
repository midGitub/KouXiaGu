using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KouXiaGu.Resources
{


    public class BasicTerrainResourceSerializer : IReader<BasicTerrainResource>
    {
        internal RoadInfoXmlSerializer RoadReader = new RoadInfoXmlSerializer();
        internal LandformInfoXmlSerializer LandformReader = new LandformInfoXmlSerializer();
        internal IReader<Dictionary<int, BuildingInfo>> BuildingReader = new BuildingXmlSerializer();

        public BasicTerrainResource Read()
        {
            var item = new BasicTerrainResource()
            {
                Landform = LandformReader.Read(),
                Road = RoadReader.Read(),
                Building = BuildingReader.Read(),
            };
            return item;
        }
    }
}
