using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using KouXiaGu.Grids;

namespace KouXiaGu.World.Map
{

    /// <summary>
    /// 地图数据;
    /// </summary>
    public sealed class MapResource
    {
        static readonly PredefinedMapReader predefinedMapReader = new PredefinedMapProtoReader();
        static readonly ArchiveMapReader archiveMapReader = new ArchiveMapProtoReader();

        /// <summary>
        /// 从预制初始化地图数据;
        /// </summary>
        public static MapResource Read()
        {
            PredefinedMap predefinedMap = predefinedMapReader.Read();
            var data = new MapResource(predefinedMap);
            return data;
        }

        /// <summary>
        /// 从预制初始化地图数据;
        /// </summary>
        public static IAsyncOperation<MapResource> ReadAsync()
        {
            return new ThreadDelegateOperation<MapResource>(Read);
        }

        /// <summary>
        /// 从存档和预制初始化地图数据;
        /// </summary>
        public static MapResource Read(string archiveDir)
        {
            PredefinedMap predefinedMap = predefinedMapReader.Read();
            ArchiveMap archiveMap = archiveMapReader.Read(archiveDir);
            var data = new MapResource(predefinedMap, archiveMap);
            return data;
        }

        /// <summary>
        /// 从存档和预制初始化地图数据;
        /// </summary>
        public static IAsyncOperation<MapResource> ReadAsync(string archiveDir)
        {
            return new ThreadDelegateOperation<MapResource>(() => Read(archiveDir));
        }


        public static MapResource ReadOrCreate()
        {
            MapResource data;
            try
            {
                data = Read();
            }
            catch
            {
                data = new MapResource();
            }
            return data;
        }

        public static IAsyncOperation<MapResource> ReadOrCreateAsync()
        {
            return new ThreadDelegateOperation<MapResource>(ReadOrCreate);
        }



        /// <summary>
        /// 空白地图;
        /// </summary>
        public MapResource()
            : this(new PredefinedMap())
        {
        }

        /// <summary>
        /// 仅从预制地图初始化地图数据;
        /// </summary>
        /// <param name="map"></param>
        public MapResource(PredefinedMap map)
        {
            if (map == null)
                throw new ArgumentNullException();

            PredefinedMap = map;
            ArchiveMap = new ArchiveMap();
            ArchiveMap.Subscribe(PredefinedMap);
        }

        /// <summary>
        /// 初始化地图数据;
        /// </summary>
        /// <param name="map">不包含存档内容的地图数据;</param>
        /// <param name="archive">变化内容,存档内容</param>
        public MapResource(PredefinedMap map, ArchiveMap archive)
        {
            if (map == null || archive == null)
                throw new ArgumentNullException();

            PredefinedMap = map;
            ArchiveMap = archive;
            PredefinedMap.Update(ArchiveMap);
            ArchiveMap.Subscribe(PredefinedMap);
        }


        /// <summary>
        /// 地图数据,包括修改内容;
        /// </summary>
        internal PredefinedMap PredefinedMap { get; private set; }

        /// <summary>
        /// 存档地图数据;
        /// </summary>
        internal ArchiveMap ArchiveMap { get; private set; }

        /// <summary>
        /// 当前游戏地图数据;
        /// </summary>
        public IDictionary<CubicHexCoord, MapNode> Data
        {
            get { return PredefinedMap.Data; }
        }

        /// <summary>
        /// 输出地图资源(保存修改后的地图);
        /// </summary>
        public void WriteMap()
        {
            predefinedMapReader.Write(PredefinedMap);
        }

        /// <summary>
        /// 输出地图资源到路径;
        /// </summary>
        public void WriteMap(string filePath)
        {
            predefinedMapReader.Write(PredefinedMap, filePath);
        }

        /// <summary>
        /// 输出存档;
        /// </summary>
        public void WriteArchived(string archivedDir)
        {
            archiveMapReader.Write(ArchiveMap, archivedDir);
        }

        /// <summary>
        /// 设置新的存档信息;
        /// </summary>
        public void SetArchiveMap(ArchiveMap archive)
        {
            ArchiveMap.Unsubscribe();
            PredefinedMap.Update(archive);
            ArchiveMap = archive;
            archive.Subscribe(PredefinedMap);
        }
    }

}
