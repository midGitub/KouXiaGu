using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace KouXiaGu.Terrain3D
{

    [DisallowMultipleComponent]
    public class BuildingWatcher : OChunkWatcher, IBuildingWatcher
    {
        BuildingWatcher()
        {
        }

        void OnEnable()
        {
            BuildingUpdater.WatcherList.Add(this);
        }

        void OnDisable()
        {
            BuildingUpdater.WatcherList.Remove(this);
        }
    }

}
