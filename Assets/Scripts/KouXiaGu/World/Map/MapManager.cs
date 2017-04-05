using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KouXiaGu.World.Map
{

    public interface IMapReader
    {
        MapInfo Info { get; }

        void Read();
        void Write();
    }


    public class MapManager
    {

        /// <summary>
        /// 世界所使用的地图;
        /// </summary>
        public Map Map { get; private set; }

        public MapManager()
        {
        }

        public IAsync Initialize(WorldInfo info)
        {
            return new Initializer(this, info);
        }

        class Initializer : IAsync
        {
            MapManager manager;
            WorldInfo info;
            ArchiveWorldInfo archiveInfo;

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

            }

            void ReadMapFromArchive()
            {

            }

        }

    }

}
