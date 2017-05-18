using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KouXiaGu.Terrain3D;
using KouXiaGu.Grids;

namespace KouXiaGu.World.Map
{

    /// <summary>
    /// 地图读取基类;
    /// </summary>
    [Serializable]
    public class MapResourceReader
    {
        static readonly PredefinedMapReader predefinedMapReader = new PredefinedMapProtoReader();
        static readonly ArchiveMapReader archiveMapReader = new ArchiveMapProtoReader();

        public MapResourceReader()
        {
        }

        public MapResourceReader(ArchiveFile archive)
        {
            Archive = archive;
        }

        public ArchiveFile Archive { get; private set; }

        /// <summary>
        /// 读取游戏地图;
        /// </summary>
        public MapResource Read(IGameData gameData)
        {
            if (Archive == null)
            {
                PredefinedMap predefinedMap = ReadPredefinedMap(gameData);
                var data = new MapResource(predefinedMap);
                return data;
            }
            else
            {
                PredefinedMap predefinedMap = ReadPredefinedMap(gameData);
                ArchiveMap archiveMap = ReadArchiveMap(gameData, Archive.ArchiveDirectory);
                var data = new MapResource(predefinedMap, archiveMap);
                return data;
            }
        }

        public IAsyncOperation<MapResource> ReadAsync(IGameData gameData)
        {
            return new ThreadDelegateOperation<MapResource>(() => Read(gameData));
        }

        protected virtual PredefinedMap ReadPredefinedMap(IGameData gameData)
        {
            PredefinedMap predefinedMap = predefinedMapReader.Read();
            return predefinedMap;
        }

        protected virtual ArchiveMap ReadArchiveMap(IGameData gameData, string archiveDir)
        {
            ArchiveMap archiveMap = archiveMapReader.Read(archiveDir);
            return archiveMap;
        }
    }


    [Serializable]
    public class RandomMapReader : MapResourceReader
    {
        public RandomMapReader(int mapSize)
        {
            MapSize = mapSize;
        }

        public RandomMapReader(int mapSize, ArchiveFile archive)
            : base(archive)
        {
            MapSize = mapSize;
        }

        public int MapSize { get; set; }

        protected override PredefinedMap ReadPredefinedMap(IGameData gameData)
        {
            int[] landformArray = gameData.Terrain.LandformInfos.Keys.ToArray();
            int[] buildArray = gameData.Terrain.BuildingInfos.Keys.ToArray();
            PredefinedMap map = new PredefinedMap();
            var points = CubicHexCoord.Range(CubicHexCoord.Self, MapSize);

            foreach (var point in points)
            {
                MapNode node = new MapNode()
                {
                    Landform = new LandformNode()
                    {
                        Type = Random(landformArray),
                        Angle = RandomAngle(),
                    },

                    Building = new BuildingNode()
                    {
                        Type = Random(buildArray),
                        Angle = RandomAngle(),
                    },
                };
                map.Data.Add(point, node);
            }
            return map;
        }

        static readonly System.Random random = new System.Random();

        T Random<T>(T[] array)
        {
            int index = random.Next(0, array.Length);
            return array[index];
        }

        int RandomAngle()
        {
            return random.Next(0, 360);
        }

        protected override ArchiveMap ReadArchiveMap(IGameData gameData, string archiveDir)
        {
            return new ArchiveMap();
        }
    }

}
