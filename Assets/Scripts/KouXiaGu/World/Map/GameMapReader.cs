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
    class GameMapSerializer : FileReaderWriter<MapData>, IGameMapReader, IWriter<GameMap>
    {
        public GameMapSerializer() : base(new GameMapFile(), ProtoFileSerializer<MapData>.Default)
        {
        }

        public GameMapSerializer(ISingleFilePath file, IFileSerializer<MapData> serializer) : base(file, serializer)
        {
        }

        public GameMap Read(IGameResource item)
        {
            MapData mapData = Read();
            GameMap gameMap = new GameMap(mapData);
            return gameMap;
        }

        public void Write(GameMap item, FileMode fileMode)
        {
            MapData mapData = item.Data;
            Write(mapData, fileMode);
        }
    }

    class GameMapFile : SingleFilePath
    {
        public override string FileName
        {
            get { return "World/Map.data"; }
        }
    }

    class GameMapArchiveSerializer
    {

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
