//using System;
//using System.Collections.Generic;
//using System.IO;
//using System.Linq;
//using System.Text;

//namespace KouXiaGu.Resources
//{

//    public class BasicTerrainResourceSerializer : ISerializer<BasicTerrainResource>
//    {
//        internal ISerializer<Dictionary<int, LandformInfo>> LandformReader = new LandformXmlSerializer();
//        internal ISerializer<Dictionary<int, RoadInfo>> RoadReader = new RoadXmlSerializer();
//        internal ISerializer<Dictionary<int, BuildingInfo>> BuildingReader = new BuildingXmlSerializer();

//        public BasicTerrainResource Read()
//        {
//            var item = new BasicTerrainResource()
//            {
//                Landform = LandformReader.Read(),
//                Road = RoadReader.Read(),
//                Building = BuildingReader.Read(),
//            };
//            return item;
//        }

//        public void Write(BasicTerrainResource item, FileMode fileMode)
//        {
//            LandformReader.Write(item.Landform, fileMode);
//            RoadReader.Write(item.Road, fileMode);
//            BuildingReader.Write(item.Building, fileMode);
//        }
//    }
//}
