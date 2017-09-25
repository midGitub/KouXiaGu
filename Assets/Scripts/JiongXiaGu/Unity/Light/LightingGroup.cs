//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using UnityEngine;

//namespace JiongXiaGu.World
//{

//    [DisallowMultipleComponent]
//    public class LightingGroup : SceneSington<LightingGroup>
//    {

//        [SerializeField]
//        Light[] lights;
//        [SerializeField]
//        float intensity;

//        public float Intensity
//        {
//            get { return intensity; }
//        }

//        void OnValidate()
//        {
//            SetIntensity(intensity);
//        }

//        public void SetIntensity(float intensity)
//        {
//            foreach (var light in lights)
//            {
//                light.intensity = intensity;
//            }
//        }

//        [ContextMenu("获取所有灯组件")]
//        void FindChildLights()
//        {
//            lights = GetComponentsInChildren<Light>();
//            if (lights != null && lights.Length > 0)
//            {
//                intensity = lights[0].intensity;
//            }
//        }
//    }
//}
