using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace KouXiaGu.World
{


    [Obsolete]
    public class WorldManager
    {

        /// <summary>
        /// 世界信息;
        /// </summary>
        public WorldInfo Info { get; private set; }

        /// <summary>
        /// 资源\产品;
        /// </summary>
        public ProductManager Product { get; private set; }

        /// <summary>
        /// 建筑物;
        /// </summary>
        public BuildingManager Building { get; private set; }

        /// <summary>
        /// 时间;
        /// </summary>
        public TimeManager Time { get; private set; }

        /// <summary>
        /// 基础信息;
        /// </summary>
        public WorldElementManager ElementInfo { get; private set; }

        public WorldManager(WorldInfo info)
        {
            Info = info;

            Time = SceneObject.GetObject<TimeManager>();
            Time.Initialize(info.Time);
        }


    }

}
