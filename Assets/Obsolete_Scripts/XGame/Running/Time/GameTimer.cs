using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace XGame.Running
{

    [RequireComponent(typeof(GameTime))]
    [DisallowMultipleComponent]
    public class GameTimer : Controller<GameTimer>, IGameArchive
    {

        [SerializeField]
        private CallOrder callOrder = CallOrder.Instance;

        [Header("更新事件:")]

        /// <summary>
        /// 事件更新速度(秒);
        /// </summary>
        [SerializeField]
        private float updateSpeed = 1;

        [SerializeField]
        private UnityEvent onEvevyMonthEvent;
        [SerializeField]
        private UnityEvent onEvevyYearEvent;

        public event Action<DateTime> OnEvevyMonthAction;
        public event Action<DateTime> OnEvevyYearAction;

        //事件更新协程指令;
        private WaitForSeconds m_EventUpdateYieldInstruction;
        private Coroutine m_EventUpdateCoroutine;

        /// <summary>
        /// 游戏时间组件;
        /// </summary>
        private GameTime gameTime;

        //记录上次更新的时间;
        public int LastYear { get; private set; }
        public int LastMonth { get; private set; }

        public UnityEvent OnEvevyMonthEvent{ get { return onEvevyMonthEvent; } }
        public UnityEvent OnEvevyYearEvent{ get { return onEvevyYearEvent; } }

        protected override GameTimer This { get { return this; } }
        CallOrder ICallOrder.CallOrder{ get { return callOrder; } }

        protected override void Awake()
        {
            base.Awake();
            GetComponentInParent<GameLoader>().OnGameStartEvent.AddListener(OnStartGame);
            gameTime = GetComponent<GameTime>();
            m_EventUpdateYieldInstruction = new WaitForSeconds(updateSpeed);
        }

        /// <summary>
        /// 当游戏开始时需要初始化的信息;
        /// </summary>
        public void OnStartGame()
        {
            m_EventUpdateCoroutine = StartCoroutine(EventUpdate());
        }

        private void OnMonth(ref DateTime nowTime)
        {
            OnEvevyMonthEvent.Invoke();
            OnEvevyMonthAction.Invoke(nowTime);
        }

        private void OnYear(ref DateTime nowTime)
        {
            OnEvevyYearEvent.Invoke();
            OnEvevyYearAction.Invoke(nowTime);
        }

        /// <summary>
        /// 事件更新协程;
        /// </summary>
        /// <returns></returns>
        private IEnumerator EventUpdate()
        {
            DateTime nowTime;
            int Year;
            int Month;

            while (true)
            {
                nowTime = gameTime.NowTime;
                Year = nowTime.Year;
                Month = nowTime.Month;

                if (Month != LastMonth)
                {
                    LastMonth = Month;
                    OnMonth(ref nowTime);

                    if (Year != LastYear)
                    {
                        LastYear = Year;
                        OnYear(ref nowTime);
                    }
                }
                yield return m_EventUpdateYieldInstruction;
            }
        }


        IEnumerator IGameLoad.OnStart()
        {
            DateTime time = gameTime.StartTime;
            LastYear = time.Year;
            LastMonth = time.Month;
            yield return AsyncHelper.WaitForFixedUpdate;
        }

        IEnumerator IGameArchive.OnLoad(GameSaveInfo info, GameSaveData data)
        {
            DateTime time = new DateTime(data.LastTime_Ticks);
            LastYear = time.Year;
            LastMonth = time.Month;
            yield return AsyncHelper.WaitForFixedUpdate;
        }

        IEnumerator IGameLoad.OnClear()
        {
            StopCoroutine(m_EventUpdateCoroutine);
            yield return AsyncHelper.WaitForFixedUpdate;
        }

        IEnumerator IGameArchive.OnSave(GameSaveInfo info, GameSaveData data)
        {
            DateTime time = new DateTime(LastYear, LastMonth, 0);
            data.LastTime_Ticks = time.Ticks;
            yield return AsyncHelper.WaitForFixedUpdate;
        }

    }


}
