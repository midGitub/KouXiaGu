using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KouXiaGu.World.Map
{

    public class MapManager
    {
        public bool IsInitialized { get; private set; }
        public Map Map { get; private set; }
        public ArchiveMap ArchiveMap { get; private set; }

        public MapManager()
        {
            IsInitialized = false;
        }

        public IAsync Initialize(WorldInfo info)
        {
            if (IsInitialized)
                throw new ArgumentException();

            IsInitialized = true;
            return new Initializer(this, info);
        }

        void UpdateMap(Map map)
        {
            Map = map;
            ArchiveMap = new ArchiveMap(Map);
        }

        void UpdateMap(Map map, ArchiveMap archiveMap)
        {
            Map = map;
            ArchiveMap = archiveMap;
            Map.Update(archiveMap);
            ArchiveMap.Subscribe(map);
        }

        class Initializer : IAsync
        {
            MapManager manager;
            WorldInfo info;

            public bool IsCompleted { get; private set; }
            public bool IsFaulted { get; private set; }
            public Exception Ex { get; private set; }

            Initializer(MapManager manager)
            {
                IsCompleted = false;
                IsFaulted = false;
                Ex = null;
                this.manager = manager;
            }

            public Initializer(MapManager manager, WorldInfo info) : this(manager)
            {
                this.info = info;

                if(info.IsInitializeFromArchive)
                    ReadMapFromArchive();
                else
                    ReadMapOnly();
            }

            void ReadMapOnly()
            {
                try
                {
                    Map map = info.Map.ReadMap();
                    manager.UpdateMap(map);
                }
                catch (Exception ex)
                {
                    IsFaulted = true;
                    Ex = ex;
                }
                finally
                {
                    IsCompleted = true;
                }
            }

            void ReadMapFromArchive()
            {
                try
                {
                    Map map = info.Map.ReadMap();
                    ArchiveMap archiveMap = info.ArchiveInfo.Map.Read();
                    manager.UpdateMap(map, archiveMap);
                }
                catch (Exception ex)
                {
                    IsFaulted = true;
                    Ex = ex;
                }
                finally
                {
                    IsCompleted = true;
                }
            }

        }

    }

}
