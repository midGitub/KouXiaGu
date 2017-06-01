using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using KouXiaGu.World.Map;
using System.Threading;
using KouXiaGu.Resources;
using System.IO;

namespace KouXiaGu.World
{


    [DisallowMultipleComponent]
    public class WorldInfoEditor : MonoBehaviour
    {

        public bool UseRandomMap;
        public WorldTimeInfo Time;
        public ArchiveFile Archive;

        //void Awake()
        //{
        //    WorldInitializer.WorldInfoReader = new WorldInfoReader(this);
        //}

        //class WorldInfoReader : AsyncOperation<WorldInfo>
        //{
        //    public WorldInfoReader(WorldInfoEditor info)
        //    {
        //        this.info = info;
        //        Start();
        //    }

        //    WorldInfoEditor info;
        //    WorldInfo worldInfo;

        //    void Start()
        //    {
        //        worldInfo = new WorldInfo()
        //        {
        //            Archive = info.Archive,
        //            Time = info.Time,
        //            MapReader = new RandomGameMapCreater(50),
        //        };
        //        OnCompleted(worldInfo);
        //    }

        //}
    }
}
