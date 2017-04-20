//using KouXiaGu.Collections;
//using KouXiaGu.Grids;
//using ProtoBuf;
//using System.IO;
//using System.Collections.Generic;
//using System;
//using System.Collections;

//namespace KouXiaGu.Terrain3D
//{

//    /// <summary>
//    /// 地图文件;
//    /// </summary>
//    public class MapDataFile
//    {
//        public MapDataFile(string name, string filePath)
//        {
//            Name = name;
//            FilePath = filePath;
//        }

//        public string Name { get; private set; }
//        public string FilePath { get; private set; }
//    }

//    public abstract class MapFiler
//    {
//        public abstract string FileExtension { get; }
//        public abstract MapData Read(string filePath);
//        public abstract ArchiveMapFile ReadArchived(string filePath);
//        public abstract void Write(string filePath, MapData data);
//        public abstract void Write(string filePath, ArchiveMapFile data);
//    }

//    public class ProtoMapFiler : MapFiler
//    {
//        const string fileExtension = ".data";

//        public override string FileExtension
//        {
//            get { return fileExtension; }
//        }

//        public override MapData Read(string filePath)
//        {
//            MapData data = ProtoBufExtensions.Deserialize<MapData>(filePath);
//            return data;
//        }

//        public override ArchiveMapFile ReadArchived(string filePath)
//        {
//            ArchiveMapFile data = ProtoBufExtensions.Deserialize<ArchiveMapFile>(filePath);
//            return data;
//        }

//        public override void Write(string filePath, ArchiveMapFile data)
//        {
//            ProtoBufExtensions.Serialize(filePath, data);
//        }

//        public override void Write(string filePath, MapData data)
//        {
//            ProtoBufExtensions.Serialize(filePath, data);
//        }
//    }


//    /// <summary>
//    /// 地形地图;
//    /// </summary>
//    [ProtoContract]
//    public class MapData
//    {
        
//        /// <summary>
//        /// 地图数据;
//        /// </summary>
//        [ProtoMember(1)]
//        public OObservableDictionary<CubicHexCoord, MapNode> Data { get; private set; }

//        [ProtoMember(2)]
//        public RoadData Road { get; private set; }
//        public LandformData Landform { get; private set; }
//        public BuildingData Building { get; private set; }

//        /// <summary>
//        /// 用于监视地图变化;
//        /// </summary>
//        public DictionaryArchiver<CubicHexCoord, MapNode> ArchiveData { get; private set; }


//        MapData()
//        {
//        }

//        public MapData(bool isArchive)
//        {
//            Data = new OObservableDictionary<CubicHexCoord, MapNode>();
//            Road = new RoadData(Data);
//            Landform = new LandformData(Data);
//            Building = new BuildingData(Data);

//            if (isArchive)
//            {
//                ArchiveData = new DictionaryArchiver<CubicHexCoord, MapNode>();
//                ArchiveData.Subscribe(Data);
//            }
//        }

//        [ProtoAfterDeserialization]
//        void AfterDeserialization()
//        {
//            Landform = new LandformData(Data);
//            Building = new BuildingData(Data);

//            ArchiveData = new DictionaryArchiver<CubicHexCoord, MapNode>();
//            ArchiveData.Subscribe(Data);
//        }

//        /// <summary>
//        /// 从文件读取后检查是否完整;
//        /// </summary>
//        public bool IsIntegrated()
//        {
//            return
//                Data != null &&
//                Road != null &&
//                Landform != null &&
//                Building != null;
//        }

//        /// <summary>
//        /// 清除记录;
//        /// </summary>
//        public void ClearArchiveData()
//        {
//            this.ArchiveData.Clear();
//        }


//        /// <summary>
//        /// 获取到变化部分的实例;
//        /// </summary>
//        public ArchiveMapFile GetArchiveMap()
//        {
//            ArchiveMapFile archive = new ArchiveMapFile()
//            {
//                Data = ArchiveData,
//                Road = Road,
//            };
//            return archive;
//        }

//        /// <summary>
//        /// 添加新的变化内容到地图;
//        /// </summary>
//        public void AddArchiveFile(ArchiveMapFile file)
//        {
//            AddArchiveData(file.Data);
//            AddRoadDate(file.Road);
//            file.Clear();
//        }

//        void AddArchiveData(DictionaryArchiver<CubicHexCoord, MapNode> archiveData)
//        {
//            if (ArchiveData.Count == 0)
//            {
//                ArchiveData.Unsubscribe();
//                ArchiveData = null;
//            }

//            if (ArchiveData != null)
//            {
//                ArchiveData.Unsubscribe();
//                ArchiveData.AddOrUpdate(archiveData);
//            }
//            else
//            {
//                ArchiveData = archiveData;
//            }

//            Data.AddOrUpdate(archiveData);
//            ArchiveData.Subscribe(Data);
//        }

//        void AddRoadDate(RoadData road)
//        {
//            Road = road;
//        }

//    }

//    /// <summary>
//    /// 记录地图变化内容;
//    /// </summary>
//    [ProtoContract]
//    public class ArchiveMapFile
//    {

//        /// <summary>
//        /// 地图变化信息;
//        /// </summary>
//        [ProtoMember(1)]
//        public DictionaryArchiver<CubicHexCoord, MapNode> Data { get; set; }

//        /// <summary>
//        /// 地形道路信息;
//        /// </summary>
//        [ProtoMember(2)]
//        public RoadData Road { get; set; }

//        public void Clear()
//        {
//            Data = null;
//            Road = null;
//        }
//    }

//}
