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

        class Initializer : IAsync
        {
            MapManager manager;
            WorldInfo info;

            public bool IsCompleted { get; private set; }
            public bool IsFaulted { get; private set; }
            public Exception Ex { get; private set; }

            public Map Map
            {
                get { return manager.Map; }
                private set { manager.Map = value; }
            }
            public ArchiveMap ArchiveMap
            {
                get { return manager.ArchiveMap; }
                private set { manager.ArchiveMap = value; }
            }

            Initializer()
            {
                IsCompleted = false;
                IsFaulted = false;
                Ex = null;
            }

            public Initializer(MapManager manager, WorldInfo info) : this()
            {
                this.manager = manager;
                this.info = info;
                ReadMap();
            }

            void ReadMap()
            {
                IMapReader reader = info.Map;
                try
                {
                    Map = reader.GetMap();
                    Map.Enable();

                    ArchiveMap = reader.GetArchiveMap();
                    ArchiveMap.Subscribe(Map);
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
