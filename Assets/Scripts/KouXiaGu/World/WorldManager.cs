using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KouXiaGu.World.Map;
using KouXiaGu.World.Navigation;
using KouXiaGu.Rx;
using UnityEngine;

namespace KouXiaGu.World
{


    public class WorldManager : IObserver<IWorld>
    {

        /// <summary>
        /// 世界信息;
        /// </summary>
        [Obsolete]
        public WorldInfo Info { get; private set; }

        /// <summary>
        /// 资源\产品;
        /// </summary>
        [Obsolete]
        public ProductManager Product { get; private set; }

        /// <summary>
        /// 建筑物;
        /// </summary>
        [Obsolete]
        public BuildingManager Building { get; private set; }



        public WorldManager(WorldInfo info, WorldElementResource elementInfo)
        {
            Time = new TimeManager(info.Time);
            Map = new MapManager();
            Navigation = new NavigationManager(this);
        }

        /// <summary>
        /// 时间;
        /// </summary>
        public TimeManager Time { get; private set; }

        /// <summary>
        /// 地图信息;
        /// </summary>
        public MapManager Map { get; private set; }

        /// <summary>
        /// 路径导航;
        /// </summary>
        public NavigationManager Navigation { get; private set; }

        public void StartWorld()
        {
            Time.StartTimeUpdating();
        }

        void IObserver<IWorld>.OnNext(IWorld item)
        {
            return;
        }

        void IObserver<IWorld>.OnError(Exception error)
        {
            return;
        }

        void IObserver<IWorld>.OnCompleted()
        {
            StartWorld();
        }

    }

}
