using System;
using System.Collections;
using UnityEngine;

namespace XGame.Running
{

    /// <summary>
    /// 游戏时间控制;通过游戏进行时间 + 存档运行到的时间,获取到当前时间;
    /// </summary>
    [DisallowMultipleComponent]
    public sealed class GameTime : Controller<GameTime>, IGameArchive
    {

        [SerializeField]
        private CallOrder callOrder = CallOrder.Static;

        /// <summary>
        /// 游戏时间缩放;
        /// </summary>
        [SerializeField]
        private float timeScale = 1;

        /// <summary>
        /// 时间更新速度;
        /// </summary>
        [SerializeField]
        private float updateSpeed = 1;

        [Header("初始时间设置:")]

        public int Year;
        public int Month;
        public int Day;
        public int Hour;
        public int Minute;
        public int Second;


        /// <summary>
        /// 事件更新协程指令;
        /// </summary>
        private WaitForSeconds m_timeUpdateYieldInstruction;

        /// <summary>
        /// 游戏时间更新协程;
        /// </summary>
        private Coroutine m_TimeUpdateCoroutine;

        /// <summary>
        /// 游戏开始时的Unity时间;
        /// </summary>
        private float m_GameStartTime;

        /// <summary>
        /// 存档创建时的时间;
        /// </summary>
        private DateTime m_SaveFileStartTime;

        /// <summary>
        /// 存档载入时运行到的时间;
        /// </summary>
        private DateTime m_SaveFileTime;

        /// <summary>
        /// 现在的存档游戏时间;
        /// </summary>
        private DateTime m_NowTime;

        /// <summary>
        /// 获取到游戏设定的开始时间(用于初始化存档);
        /// </summary>
        private DateTime NewStartTime{ get { return new DateTime(Year, Month, Day, Hour, Minute, Second); } }

        /// <summary>
        /// 获取到游戏运行状态的Unity时间(秒);
        /// </summary>
        public float RunningTime{ get { return (Time.time - m_GameStartTime) * timeScale; } }

        /// <summary>
        /// 存档创建时的时间;
        /// </summary>
        public DateTime StartTime{ get { return m_SaveFileStartTime; } }

        /// <summary>
        /// 现在当前存档游戏进行到的时间;
        /// </summary>
        public DateTime NowTime{ get { return m_NowTime; } }

        protected override GameTime This { get { return this; } }
        CallOrder ICallOrder.CallOrder { get { return callOrder; } }


        protected override void Awake()
        {
            base.Awake();
            GetComponentInParent<GameLoader>().OnGameStartEvent.AddListener(OnStartGame);
            m_timeUpdateYieldInstruction = new WaitForSeconds(updateSpeed);
        }

        /// <summary>
        /// 当游戏开始时需要初始化的信息;
        /// </summary>
        private void OnStartGame()
        {
#if UNITY_EDITOR
            if (m_TimeUpdateCoroutine != null)
                Debug.LogError("重复更新时间!");
#endif
            m_GameStartTime = Time.time;
            m_TimeUpdateCoroutine = StartCoroutine(TimeUpdate());
        }

        /// <summary>
        /// 事件更新协程;
        /// </summary>
        /// <returns></returns>
        public IEnumerator TimeUpdate()
        {
            while (true)
            {
                m_NowTime = m_SaveFileTime.AddSeconds(RunningTime);
                yield return m_timeUpdateYieldInstruction;
            }
        }

        IEnumerator IGameLoad.OnStart()
        {
            DateTime newTime = NewStartTime;
            m_SaveFileStartTime = newTime;
            m_SaveFileTime = newTime;
            m_NowTime = newTime;
            yield return AsyncHelper.WaitForFixedUpdate;
        }

        IEnumerator IGameArchive.OnLoad(GameSaveInfo info, GameSaveData data)
        {
            m_SaveFileStartTime = new DateTime(info.StartTime_Ticks);
            m_SaveFileTime = new DateTime(info.Time_Ticks);
            m_NowTime = m_SaveFileTime;
            yield return AsyncHelper.WaitForFixedUpdate;
        }

        IEnumerator IGameLoad.OnClear()
        {
            StopCoroutine(m_TimeUpdateCoroutine);
            yield return AsyncHelper.WaitForFixedUpdate;
        }

        IEnumerator IGameArchive.OnSave(GameSaveInfo info, GameSaveData data)
        {
            info.StartTime_Ticks = m_SaveFileStartTime.Ticks;
            info.Time_Ticks = NowTime.Ticks;
            yield return AsyncHelper.WaitForFixedUpdate;
        }



    }

}

