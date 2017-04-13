using System;
using System.Collections.Generic;


namespace KouXiaGu.World
{


    public class WorldElementResource
    {

        internal static DataReader<Dictionary<int, RoadInfo>, IEnumerable<RoadInfo>> RoadReader = new RoadInfoXmlSerializer();
        internal static DataReader<Dictionary<int, LandformInfo>, IEnumerable<LandformInfo>> LandformReader = new LandformInfoXmlSerializer();
        internal static DataReader<Dictionary<int, BuildingInfo>, IEnumerable<BuildingInfo>> BuildingReader = new BuildingInfosXmlSerializer();
        internal static DataReader<Dictionary<int, ProductElementInfo>, IEnumerable<ProductElementInfo>> ProductReader = new ProductInfosXmlSerializer();

        /// <summary>
        /// 同步读取所有信息;
        /// </summary>
        public static WorldElementResource Read()
        {
            var item = new WorldElementResource(false);
            item.Initialize();
            return item;
        }

        /// <summary>
        /// 异步读取所有信息;
        /// </summary>
        public static AsyncOperation<WorldElementResource> ReadAsync()
        {
            return new AsyncReader();
        }


        /// <summary>
        /// 初始化为空;
        /// </summary>
        public WorldElementResource()
        {
            RoadInfos = new Dictionary<int, RoadInfo>();
            LandformInfos = new Dictionary<int, LandformInfo>();
            BuildingInfos = new Dictionary<int, BuildingInfo>();
            ProductInfos = new Dictionary<int, ProductElementInfo>();
        }

        /// <summary>
        /// 不进行任何方法的构造函数;
        /// </summary>
        internal WorldElementResource(bool none)
        {
        }

        public Dictionary<int, RoadInfo> RoadInfos { get; protected set; }
        public Dictionary<int, LandformInfo> LandformInfos { get; protected set; }
        public Dictionary<int, BuildingInfo> BuildingInfos { get; protected set; }
        public Dictionary<int, ProductElementInfo> ProductInfos { get; protected set; }

        /// <summary>
        /// 同步的读取所有信息;
        /// </summary>
        void Initialize()
        {
            RoadInfos = RoadReader.Read();
            LandformInfos = LandformReader.Read();
            BuildingInfos = BuildingReader.Read();
            ProductInfos = ProductReader.Read();
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
            WriteToDirectory(ProductReader, ProductInfos, dirPath, overlay);
        }

        protected virtual void WriteToDirectory<T>(
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


        /// <summary>
        /// 多线程读取到;
        /// </summary>
        class AsyncReader : AsyncOperation<WorldElementResource>
        {
            protected override WorldElementResource Operate()
            {
                return WorldElementResource.Read();
            }
        }

    }

}
