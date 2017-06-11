using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace KouXiaGu.World
{

    [DisallowMultipleComponent]
    public class LightingGroup : SceneSington<LightingGroup>
    {
        [SerializeField]
        Light[] lights;

        public void SetIntensity(float intensity)
        {
            foreach (var light in lights)
            {
                light.intensity = intensity;
            }
        }

        [ContextMenu("获取所有灯组件")]
        void FindChildLights()
        {
            lights = GetComponentsInChildren<Light>();
        }
    }
}
