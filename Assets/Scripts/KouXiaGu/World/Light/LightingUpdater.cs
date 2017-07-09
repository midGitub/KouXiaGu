using KouXiaGu.World.TimeSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace KouXiaGu.World
{

    [DisallowMultipleComponent]
    public class LightingUpdater : MonoBehaviour, IStateObserver<IWorldComplete>
    {
        [SerializeField]
        LightingGroup globalLights;
        [SerializeField]
        Light sunLight;
        [SerializeField]
        LightOfTime[] lightStyles;
        WorldTime timeManager;
        int currentHour;

        public LightOfTime[] LightStyles
        {
            get { return lightStyles; }
            set { lightStyles = value; }
        }

        void Start()
        {
            enabled = false;
            WorldSceneManager.OnWorldInitializeComplted.Subscribe(this);
        }

        void Update()
        {
            WorldDateTime time = timeManager.CurrentTime;
            int hour = time.Hour;
            int minute = time.Minute;
            float t = minute / 59f;
            LightOfTime current = LightStyles[hour];
            LightOfTime next = LightStyles[hour + 1 >= 24 ? 0 : hour + 1];

            sunLight.intensity = Mathf.Lerp(current.SunIntensity, next.SunIntensity, t);
            sunLight.transform.rotation = Quaternion.Lerp(Quaternion.Euler(current.SunRotation), Quaternion.Euler(next.SunRotation), t);
            globalLights.SetIntensity(Mathf.Lerp(current.GlobalIntensity, next.GlobalIntensity, t));

            if (currentHour != hour)
            {
                currentHour = hour;
                if (current.Skybox != null)
                {
                    RenderSettings.skybox = current.Skybox;
                }
            }
        }

        /// <summary>
        /// 获取到当前的状态;
        /// </summary>
        public LightOfTime GetCurrentState()
        {
            LightOfTime item = new LightOfTime()
            {
                SunIntensity = sunLight.intensity,
                SunRotation = sunLight.transform.rotation.eulerAngles,
                GlobalIntensity = globalLights.Intensity,
                Skybox = RenderSettings.skybox,
            };
            return item;
        }

        [ContextMenu("设置当前状态到所有时间")]
        void SetCurrentStateToAll()
        {
            LightOfTime item = GetCurrentState();
            lightStyles = new LightOfTime[24];

            for (int i = 0; i < 24; i++)
            {
                lightStyles[i] = item;
            }
        }

        void IStateObserver<IWorldComplete>.OnCompleted(IWorldComplete item)
        {
            timeManager = item.WorldData.Time;
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
