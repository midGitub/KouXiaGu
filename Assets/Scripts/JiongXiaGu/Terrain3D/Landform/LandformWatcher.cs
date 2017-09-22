using System;
using System.Collections.Generic;
using JiongXiaGu.Grids;
using JiongXiaGu.Collections;
using UnityEngine;

namespace JiongXiaGu.Terrain3D
{
    /// <summary>
    /// 根据挂载物体所在位置创建附近地形;
    /// </summary>
    [DisallowMultipleComponent]
    public class LandformWatcher : OChunkWatcher, ILandformWatcher
    {
        LandformWatcher()
        {
        }

        void OnEnable()
        {
            OLandformUpdater.WatcherList.Add(this);
        }

        void OnDisable()
        {
            OLandformUpdater.WatcherList.Remove(this);
        }
    }
}
