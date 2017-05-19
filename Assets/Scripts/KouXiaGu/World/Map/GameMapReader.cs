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

        public GameMap Read(IGameData info)
        {
            MapData data = mapDataReader.Read(info);
            return new GameMap(data);
        }
    }
}
