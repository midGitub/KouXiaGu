using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KouXiaGu.World
{


    public class WorldElementManager
    {
        static WorldElementManager()
        {
            RoadReader = new RoadInfoXmlSerializer();
            LandformReader = new LandformInfoXmlSerializer();
            BuildingReader = new BuildingInfosXmlSerializer();
        }

        internal static DataReader<Dictionary<int, RoadInfo>, IEnumerable<RoadInfo>> RoadReader { get; set; }
        internal static DataReader<Dictionary<int, LandformInfo>, IEnumerable<LandformInfo>> LandformReader { get; set; }
        internal static DataReader<Dictionary<int, BuildingInfo>, IEnumerable<BuildingInfo>> BuildingReader { get; set; }

        /// <summary>
        /// 同步读取所有信息;
        /// </summary>
        public static WorldElementManager Read()
        {
            var item = new WorldElementManager(false);
            item.Initialize();
            return item;
        }

        /// <summary>
        /// 异步读取所有信息;
        /// </summary>
        public static void ReadAsync()
        {
            throw new NotImplementedException();
        }


        public WorldElementManager()
        {
            RoadInfos = new Dictionary<int, RoadInfo>();
            LandformInfos = new Dictionary<int, LandformInfo>();
            BuildingInfos = new Dictionary<int, BuildingInfo>();
        }

        /// <summary>
        /// 不进行任何方法的构造函数;
        /// </summary>
        internal WorldElementManager(bool none)
        {
        }

        public Dictionary<int, RoadInfo> RoadInfos { get; protected set; }
        public Dictionary<int, LandformInfo> LandformInfos { get; protected set; }
        public Dictionary<int, BuildingInfo> BuildingInfos { get; protected set; }

        /// <summary>
        /// 同步的读取所有信息;
        /// </summary>
        void Initialize()
        {
            RoadInfos = RoadReader.Read();
            LandformInfos = LandformReader.Read();
            BuildingInfos = BuildingReader.Read();
        }

        /// <summary>
        /// 输出所有信息到文件夹内;
        /// </summary>
        /// <param name="overlay">输出到的文件夹;</param>
        /// <param name="overlay">是否覆盖已经存在的文件?</param>
        public void WriteToDirectory(string dirPath, bool overlay)
        {
            WriteToDirectory(RoadReader, RoadInfos, dirPath, overlay);
            WriteToDirectory(LandformReader, LandformInfos, dirPath, overlay);
            WriteToDirectory(BuildingReader, BuildingInfos, dirPath, overlay);
        }

        void WriteToDirectory<T>(
            DataReader<Dictionary<int, T>, IEnumerable<T>> reader,
            Dictionary<int, T> dictionary,
            string dirPath,
            bool overlay)
        {
            if (!overlay && reader.File.Exists(dirPath))
                return;

            IEnumerable<T> infos = dictionary.Values;
            reader.WriteToDirectory(infos, dirPath);
        }

    }

}
