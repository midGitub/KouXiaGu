using KouXiaGu.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace KouXiaGu.World.Map
{

    public interface IGameMapReader : IReader<GameMap, IGameResource>
    {
    }

    /// <summary>
    /// 游戏地图读取和保存;
    /// </summary>
    sealed class MapDataReader : IGameMapReader
    {
        public MapDataReader() : this(null)
        {
        }

        public MapDataReader(ISingleFilePath archivedFile) : this(new MapFile(), archivedFile, ProtoFileSerializer<MapData>.Default)
        {
        }

        public MapDataReader(ISingleFilePath dataFile, ISingleFilePath archivedFile, IFileSerializer<MapData> serializer)
        {
            DataFile = dataFile;
            ArchivedFile = archivedFile;
            Serializer = serializer;
        }

        public ISingleFilePath DataFile { get; private set; }
        public ISingleFilePath ArchivedFile { get; private set; }
        public IFileSerializer<MapData> Serializer { get; private set; }

        public GameMap Read(IGameResource item)
        {
            MapData mapData = Serializer.Read(DataFile.GetFullPath());
            if (ArchivedFile != null)
            {
                MapData archiveData = Serializer.Read(ArchivedFile.GetFullPath());
                return new GameMap(mapData, archiveData);
            }
            return new GameMap(mapData);
        }
    }

    sealed class MapDataWriter : IWriter<MapData>
    {
        public MapDataWriter(ISingleFilePath file) : this(file, ProtoFileSerializer<MapData>.Default)
        {
        }

        public MapDataWriter(ISingleFilePath file, IFileSerializer<MapData> serializer)
        {
            File = file;
            Serializer = serializer;
        }

        public ISingleFilePath File { get; private set; }
        public IFileSerializer<MapData> Serializer { get; private set; }

        public void Write(MapData item)
        {
            Serializer.Write(item, File.GetFullPath(), FileMode.Create);
        }
    }

    class MapFile : SingleFilePath
    {
        public override string FileName
        {
            get { return "World/Map.data"; }
        }
    }

    class MapArchiveFile : SingleFilePath
    {
        public MapArchiveFile(string archiveDirectory) : base(archiveDirectory)
        {
        }

        public override string FileName
        {
            get { return "World/Map.data"; }
        }
    }



    /// <summary>
    /// 随机生成的地图获取;
    /// </summary>
    class RandomGameMapCreater : IGameMapReader
    {
        public RandomGameMapCreater(int mapSize)
        {
            mapDataReader = new RandomMapDataCreater(mapSize);
        }

        RandomMapDataCreater mapDataReader;

        public GameMap Read(IGameResource info)
        {
            MapData data = mapDataReader.Read(info);
            return new GameMap(data);
        }
    }
}
