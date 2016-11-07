using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


namespace XGame.Running.Capitals
{

    /// <summary>
    /// 游戏资金;
    /// </summary>
    [DisallowMultipleComponent]
    public class CapitalController : Controller<CapitalController>, IGameArchive
    {

        [SerializeField]
        private CallOrder callOrder = CallOrder.Static;

        /// <summary>
        /// 场景内物体的价值;
        /// </summary>
        [SerializeField]
        private int totalValue = 0;

        /// <summary>
        /// 现有资金;
        /// </summary>
        [SerializeField]
        private int capital = 0;

        /// <summary>
        /// 每月维护费用;
        /// </summary>
        [SerializeField]
        private int maintenanceCosts = 0;

        /// <summary>
        /// 加入的维护费合集;
        /// </summary>
        private HashSet<ICapital> m_CapitalSet;

        private GameTimer gameTimer;

        protected override CapitalController This { get { return this; } }
        CallOrder ICallOrder.CallOrder{ get { return callOrder; } }
        protected GameTimer GetGameTimer { get { return ControllerHelper.GameController.GetComponentInChildren<GameTimer>(); } }

        /// <summary>
        /// 现持有资金;对设置做了限制;
        /// </summary>
        public int Capital{ get { return capital; } }

        /// <summary>
        /// 每月维护费用;
        /// </summary>
        public int MaintenanceCosts{ get { return maintenanceCosts; } }


        protected override void Awake()
        {
            base.Awake();
            m_CapitalSet = new HashSet<ICapital>();
            gameTimer = GetGameTimer;

            gameTimer.OnEvevyMonthEvent.AddListener(EveryMonthUpdate);
        }

        /// <summary>
        /// 加入花费项目;
        /// </summary>
        /// <param name="capitals"></param>
        public virtual void Add(ICapital capitals)
        {
            if (m_CapitalSet.Add(capitals))
            {
                this.capital += capitals.ConstructionCosts;
                this.maintenanceCosts += capitals.MaintenanceCosts;
                this.totalValue += capitals.ObjectValue;
            }
#if UNITY_EDITOR
            else
            {
                Debug.LogError("重复加入相同的 ICapital ;");
            }
#endif
        }

        /// <summary>
        /// 移除花费项目;
        /// </summary>
        /// <param name="capitals"></param>
        public virtual void Remove(ICapital capitals)
        {
            if (m_CapitalSet.Remove(capitals))
            {
                this.capital += capitals.DemolitionCosts;
                this.maintenanceCosts -= capitals.MaintenanceCosts;
                this.totalValue -= capitals.ObjectValue;
            }
#if UNITY_EDITOR
            else
            {
                Debug.LogError("尝试移除未加入的 ICapital ;");
            }
#endif
        }

        /// <summary>
        /// 每月更新;
        /// </summary>
        private void EveryMonthUpdate()
        {
            capital -= maintenanceCosts;        //扣除维护费;
        }

        /// <summary>
        /// 重置所有数据;
        /// </summary>
        private void ResetAllData()
        {
            totalValue = 0;
            capital = 0;
            maintenanceCosts = 0;

            m_CapitalSet.Clear();
        }

        IEnumerator IGameLoad.OnStart()
        {
            yield break;
        }

        IEnumerator IGameArchive.OnLoad(GameSaveInfo info, GameSaveData data)
        {
            capital = info.Capital;
            yield break;
        }

        IEnumerator IGameArchive.OnSave(GameSaveInfo info, GameSaveData data)
        {
            info.Capital = Capital;
            yield break;
        }

        IEnumerator IGameLoad.OnClear()
        {
            ResetAllData();
            yield break;
        }

    }

}
