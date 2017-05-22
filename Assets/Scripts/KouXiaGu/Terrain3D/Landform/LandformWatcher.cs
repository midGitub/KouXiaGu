﻿using System;
using System.Collections.Generic;
using KouXiaGu.Grids;
using KouXiaGu.Collections;
using UnityEngine;

namespace KouXiaGu.Terrain3D
{

    /// <summary>
    /// 根据挂载物体所在位置创建附近地形;
    /// </summary>
    [DisallowMultipleComponent]
    public class LandformWatcher : ChunkWatcher, ILandformWatcher
    {
        LandformWatcher()
        {
        }

        void OnEnable()
        {
            LandformManager.WatcherList.Add(this);
        }

        void OnDisable()
        {
            LandformManager.WatcherList.Remove(this);
        }
    }

}
