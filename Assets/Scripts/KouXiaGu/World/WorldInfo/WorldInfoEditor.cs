using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using KouXiaGu.World.Map;

namespace KouXiaGu.World
{


    [DisallowMultipleComponent]
    [RequireComponent(typeof(WorldInitializer))]
    public class WorldInfoEditor : MonoBehaviour
    {

        public bool UseRandomMap;
        public WorldTimeInfo Time;
        public ArchiveFile Archive;

        void Awake()
        {
            WorldInitializer.WorldInfoReader = new WorldInfoReader(this);
        }

        class WorldInfoReader : AsyncOperation<WorldInfo>
        {
            public WorldInfoReader(WorldInfoEditor info)
            {
                this.info = info;
                Start();
            }

            WorldInfoEditor info;
            WorldInfo worldInfo;

            void Start()
            {
                worldInfo = new WorldInfo()
                {
                    Archive = info.Archive,
                    Time = info.Time,
                    MapReader = info.UseRandomMap ? new RandomMapReadr(50) : new MapResourceReader(),
                };
                OnCompleted(worldInfo);
            }

        }
    }

}
