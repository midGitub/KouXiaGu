﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KouXiaGu.World
{


    public class WorldManager
    {
        public WorldManager(BasicInformation basicInfo, WorldInfo info)
        {
            BasicInfo = basicInfo;
            Info = info;
            Time = SceneObject.GetObject<TimeManager>();
        }

        /// <summary>
        /// 基础信息;
        /// </summary>
        public BasicInformation BasicInfo { get; private set; }

        /// <summary>
        /// 世界信息;
        /// </summary>
        public WorldInfo Info { get; private set; }

        /// <summary>
        /// 时间;
        /// </summary>
        public TimeManager Time { get; private set; }

    }

}
