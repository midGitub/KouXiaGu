using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KouXiaGu.World.Map
{

    public interface IGameMapReader
    {
        GameMap Read(IGameData info);
    }

    class RandomGameMapCreater : IGameMapReader
    {
        public RandomGameMapCreater(int mapSize)
        {
            mapDataReader = new RandomMapDataCreater(mapSize);
        }

        RandomMapDataCreater mapDataReader;

        public GameMap Read(IGameData info)
        {
            MapData data = mapDataReader.Read(info);
            return new GameMap(data);
        }
    }

    /// <summary>
    /// 游戏地图读取;
    /// </summary>
    class GameMapReader : IGameMapReader
    {
        public GameMapReader()
        {
        }

        public GameMap Read(IGameData info)
        {
            throw new NotImplementedException();
        }

        public IAsyncOperation<GameMap> ReadAsync(IGameData info)
        {
            throw new NotImplementedException();
        }
    }
}
