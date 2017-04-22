﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KouXiaGu.Grids;
using UnityEngine;
using KouXiaGu.World;
using KouXiaGu.World.Map;

namespace KouXiaGu.Terrain3D
{

    /// <summary>
    /// 地形烘培;
    /// </summary>
    [Serializable]
    class LandformBaker
    {
        LandformBaker()
        {
        }

        public BakeLandform _landform;
        public IWorldData WorldData { get; private set; }

        public void Initialise(IWorldData worldData)
        {
            WorldData = worldData;
        }

        public IEnumerator GetBakeCoroutine(RectCoord chunkCoord)
        {
            throw new NotImplementedException();
        }

    }

}
