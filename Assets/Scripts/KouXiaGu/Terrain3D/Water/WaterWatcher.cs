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
        static WaterWatcher()
        {
            watchers = new List<WaterWatcher>();
        }

        static List<WaterWatcher> watchers;
        WaterChunk chunk;

        WaterSettings settings
        {
            get { return LandformSettings.Instance.WaterSettings; }
        }

        void Update()
        {
            Vector3 position = transform.position;
            position.y = settings.SeaLevel;
            chunk.transform.position = position;
        }

        void OnEnable()
        {
            if (chunk == null)
            {
                chunk = Instantiate(settings.PrefabChunk);
            }
        }

        void OnDisable()
        {
            
        }
    }

}
