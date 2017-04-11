using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KouXiaGu.World
{


    public class WorldElement
    {
        static WorldElement()
        {
            RoadReader = new RoadInfoXmlSerializer();
            LandformReader = new LandformInfoXmlSerializer();
        }

        internal static DataReader<Dictionary<int, RoadInfo>, RoadInfo[]> RoadReader { get; set; }
        internal static DataReader<Dictionary<int, LandformInfo>, LandformInfo[]> LandformReader { get; set; }


        public WorldElement()
        {
            Initialize();
        }

        public Dictionary<int, RoadInfo> RoadInfos { get; private set; }
        public Dictionary<int, LandformInfo> LandformInfos { get; private set; }

        void Initialize()
        {
            RoadInfos = RoadReader.Read();
            LandformInfos = LandformReader.Read();
        }

        public void WriteToMainDirectory()
        {
            WriteToDirectory(GameFile.MainDirectory);
        }

        public void WriteToDirectory(string dirPath)
        {
            var roadInfos = RoadInfos.Values.ToArray();
            RoadReader.WriteToDirectory(roadInfos, dirPath);

            var landformInfos = LandformInfos.Values.ToArray();
            LandformReader.WriteToDirectory(landformInfos, dirPath);
        }

    }

}
