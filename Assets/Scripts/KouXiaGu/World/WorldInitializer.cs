using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace KouXiaGu.World
{

    /// <summary>
    /// 负责初始化游戏场景;
    /// </summary>
    [DisallowMultipleComponent]
    public class WorldInitializer : OperateWaiter
    {

        public WorldInfo Info { get; private set; }
        public static WorldManager World { get; private set; }


        void Start()
        {
            World = new WorldManager(new BasicInformation(), default(WorldInfo));
        }

        void OnDestroy()
        {
            World = null;
        }

    }


}
