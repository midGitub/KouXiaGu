using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace KouXiaGu.Terrain3D
{

    /// <summary>
    /// 挂载到需要看见水资源的物体上;
    /// </summary>
    [DisallowMultipleComponent]
    public class WaterWatcher : MonoBehaviour
    {
        static ObservableCollection<WaterWatcher> watchers = new ObservableCollection<WaterWatcher>(new List<WaterWatcher>());



        WaterManager waterManager;

        void Awake()
        {

        }
    }

}
