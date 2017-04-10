using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace KouXiaGu.World.Map
{

    public class MapManager
    {

        public WorldInfo Info { get; private set; }
        public Map Map { get; private set; }
        public ArchiveMap ArchiveMap { get; private set; }

        public IMapReader Reader
        {
            get { return Info.Map; }
        }

        MapManager(WorldInfo info)
        {
            Info = info;
        }

        public static MapManager Create(WorldInfo info)
        {
            var item = new MapManager(info);
            item.Initialize();
            return item;
        }

        public static IAsync<MapManager> CreateAsync(WorldInfo info)
        {
            var item = new MapManager(info);
            return new AsyncInitializer(item);
        }

        /// <summary>
        /// 初始化;
        /// </summary>
        void Initialize()
        {
            ReadMap();
        }

        public void ReadMap()
        {
            Map = Reader.GetMap();
            Map.Enable();

            ArchiveMap = Reader.GetArchiveMap();
            ArchiveMap.Subscribe(Map);
        }

        public void WriteMap()
        {
            Reader.Predefined.WriteMap(Map);
        }

        public void WriteArchived(string archivedDir)
        {
            ArchiveMapFile file = ArchiveMapFile.Create(archivedDir);
            ArchiveMapInfo info = new ArchiveMapInfo(Reader.Predefined);
            file.WriteInfo(info);
            file.WriteMap(ArchiveMap);
        }

        class AsyncInitializer : IAsync<MapManager>
        {
            MapManager manager;
            public bool IsCompleted { get; private set; }
            public bool IsFaulted { get; private set; }
            public MapManager Result { get; private set; }
            public Exception Ex { get; private set; }

            AsyncInitializer()
            {
                IsCompleted = false;
                IsFaulted = false;
                Ex = null;
            }

            public AsyncInitializer(MapManager manager) : this()
            {
                this.manager = manager;
                ThreadPool.QueueUserWorkItem(Initialize);
            }

            void Initialize(object state)
            {
                try
                {
                    manager.Initialize();
                }
                catch (Exception ex)
                {
                    IsFaulted = true;
                    Ex = ex;
                }
                finally
                {
                    Result = manager;
                    IsCompleted = true;
                }
            }

        }

    }


    /// <summary>
    /// 地图存在的元素;
    /// </summary>
    public class MapElement
    {

    }

}
