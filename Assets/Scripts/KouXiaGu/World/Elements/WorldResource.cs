using System;
using System.Collections.Generic;


namespace KouXiaGu.World
{


    /// <summary>
    /// 基本资源;
    /// </summary>
    public class WorldResource
    {

        internal static DataReader<Dictionary<int, RoadInfo>, IEnumerable<RoadInfo>> RoadReader = new RoadInfoXmlSerializer();
        internal static DataReader<Dictionary<int, LandformInfo>, IEnumerable<LandformInfo>> LandformReader = new LandformInfoXmlSerializer();
        internal static DataReader<Dictionary<int, BuildingInfo>, IEnumerable<BuildingInfo>> BuildingReader = new BuildingInfosXmlSerializer();
        internal static DataReader<Dictionary<int, ProductElementInfo>, IEnumerable<ProductElementInfo>> ProductReader = new ProductInfosXmlSerializer();

        /// <summary>
        /// 同步读取所有信息;
        /// </summary>
        public static WorldResource Read()
        {
            var item = new WorldResource(false);
            item.Initialize();
            return item;
        }

        /// <summary>
        /// 异步读取所有信息;
        /// </summary>
        public static ThreadOperation<WorldResource> ReadAsync()
        {
            return new AsyncReader();
        }


        /// <summary>
        /// 初始化为空;
        /// </summary>
        public WorldResource()
        {
            Road = new Dictionary<int, RoadInfo>();
            Landform = new Dictionary<int, LandformInfo>();
            Building = new Dictionary<int, BuildingInfo>();
            ProductInfos = new Dictionary<int, ProductElementInfo>();
        }

        /// <summary>
        /// 不进行任何方法的构造函数;
        /// </summary>
        internal WorldResource(bool none)
        {
        }

        public Dictionary<int, RoadInfo> Road { get; protected set; }
        public Dictionary<int, LandformInfo> Landform { get; protected set; }
        public Dictionary<int, BuildingInfo> Building { get; protected set; }
        public Dictionary<int, ProductElementInfo> ProductInfos { get; protected set; }

        /// <summary>
        /// 同步的初始化;
        /// </summary>
        void Initialize()
        {
            Road = RoadReader.Read();
            Landform = LandformReader.Read();
            Building = BuildingReader.Read();
            ProductInfos = ProductReader.Read();
        }

        /// <summary>
        /// 输出所有信息到文件夹内;
        /// </summary>
        /// <param name="overlay">输出到的文件夹;</param>
        /// <param name="overlay">是否覆盖已经存在的文件?</param>
        public void WriteToDirectory(string dirPath, bool overlay)
        {
            WriteToDirectory(RoadReader, Road, dirPath, overlay);
            WriteToDirectory(LandformReader, Landform, dirPath, overlay);
            WriteToDirectory(BuildingReader, Building, dirPath, overlay);
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
        class AsyncReader : ThreadOperation<WorldResource>
        {
            public AsyncReader()
            {
                Start();
            }

            protected override WorldResource Operate()
            {
                return WorldResource.Read();
            }
        }

    }

}
