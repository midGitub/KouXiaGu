using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace KouXiaGu.World
{

    [DisallowMultipleComponent]
    public class LightUpdater : MonoBehaviour, IStateObserver<IWorldComplete>
    {
        [SerializeField]
        LightingGroup globalLights;
        [SerializeField]
        Light sunLight;
        [SerializeField]
        LightOfTime[] lightStyles;
        WorldTime timeManager;

        public LightOfTime[] LightStyles
        {
            get { return lightStyles; }
        }

        void Start()
        {
            enabled = false;
        }

        void Update()
        {
            DateTime time = timeManager.CurrentTime;
            int hour = time.Hour;
            int minute = time.Minute;
            float f = minute / 59;
            LightOfTime current = LightStyles[hour];
            LightOfTime next = LightStyles[++hour >= 24 ? 0 : hour];

            //暂缓;
        }

        void IStateObserver<IWorldComplete>.OnCompleted(IWorldComplete item)
        {
            timeManager = item.Components.Time;
            enabled = true;
        }

        void IStateObserver<IWorldComplete>.OnFailed(Exception ex)
        {
            return;
        }
    }

    [Serializable]
    public class LightOfTime
    {
        /// <summary>
        /// 太阳光强度;
        /// </summary>
        public float SunIntensity;

        /// <summary>
        /// 太阳光旋转变量;
        /// </summary>
        public Vector3 SunRotation;

        /// <summary>
        /// 全局光强度;
        /// </summary>
        public float GlobalIntensity;

        /// <summary>
        /// 天空盒材质,若不替换则为Null;
        /// </summary>
        public Material Skybox;
    }
}
