using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace KouXiaGu.Terrain3D
{

    [DisallowMultipleComponent]
    public class BuildingWatcher : ChunkWatcher, IBuildingWatcher
    {
        BuildingWatcher()
        {
        }

        void OnEnable()
        {
            BuildingManager.WatcherList.Add(this);
        }

        void OnDisable()
        {
            BuildingManager.WatcherList.Remove(this);
        }
    }

}
