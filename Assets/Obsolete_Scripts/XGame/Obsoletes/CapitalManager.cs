//using System;
//using System.Collections;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using UnityEngine;


//namespace XGame.GameRunning
//{

//    /// <summary>
//    /// 资本控制;
//    /// </summary>
//    [DisallowMultipleComponent]
//    public class CapitalManager : Manager<CapitalManager>, IGameSave
//    {

//        [SerializeField]
//        private CallOrder moduleType = CallOrder.Static;

//        /// <summary>
//        /// 场景内物体的价值;
//        /// </summary>
//        [SerializeField]
//        private int totalValue = 0;

//        /// <summary>
//        /// 现有资金;
//        /// </summary>
//        [SerializeField]
//        private int capital = 0;

//        /// <summary>
//        /// 每月维护费用;
//        /// </summary>
//        [SerializeField]
//        private int maintenanceCosts = 0;

//        /// <summary>
//        /// 加入的维护费合集;
//        /// </summary>
//        private HashSet<ICapital> m_CapitalSet;

//        /// <summary>
//        /// 返回单例;
//        /// </summary>
//        protected override CapitalManager This
//        {
//            get { return this; }
//        }

//        /// <summary>
//        /// 初始化次序;
//        /// </summary>
//        CallOrder ICallOrder.CallOrder
//        {
//            get { return moduleType; }
//        }

//        /// <summary>
//        /// 现持有资金;对设置做了限制;
//        /// </summary>
//        public int Capital
//        {
//            get { return capital; }
//            private set { capital = CapitalLock ? value : capital; }
//        }

//        /// <summary>
//        /// 每月维护费用;
//        /// </summary>
//        public int MaintenanceCosts
//        {
//            get { return maintenanceCosts; }
//            private set { maintenanceCosts = value; }
//        }

//        /// <summary>
//        /// 金钱锁,仅在游戏状态会变换金钱数值;true为允许变化,false为不允许变化;
//        /// </summary>
//        public bool CapitalLock
//        {
//            get { return GameController.State == GameState.Game; }
//        }


//        void IGameO.GameStart()
//        {
//            return;
//        }

//        IEnumerator IGameSave.OnLoad(GameSaveInfo info, GameSaveData data)
//        {
//            capital = info.Capital;
//            m_CapitalSet = new HashSet<ICapital>();
//            yield break;
//        }

//        IEnumerator IGameSave.OnSave(GameSaveInfo info, GameSaveData data)
//        {
//            info.Capital = Capital;
//            yield break;
//        }

//        IEnumerator IGameO.OnStart()
//        {
//            capital = CapitalController.GetInstance.InitialCapital;
//            m_CapitalSet = new HashSet<ICapital>();
//            //TimeManager.GetInstance.OnMonthEvent += MonthUpdate;
//            yield break;
//        }

//        /// <summary>
//        /// 加入花费项目;
//        /// </summary>
//        /// <param name="capitals"></param>
//        public void Add(ICapital capitals)
//        {
//            m_CapitalSet.Add(capitals);
//            this.Capital += capitals.ConstructionCosts;
//            this.MaintenanceCosts += capitals.MaintenanceCosts;
//            this.totalValue += capitals.ObjectValue;
//        }

//        /// <summary>
//        /// 移除花费项目;
//        /// </summary>
//        /// <param name="capitals"></param>
//        public void Remove(ICapital capitals)
//        {
//            m_CapitalSet.Remove(capitals);
//            this.Capital += capitals.DemolitionCosts;
//            this.MaintenanceCosts -= capitals.MaintenanceCosts;
//            this.totalValue -= capitals.ObjectValue;
//        }

//        /// <summary>
//        /// 每月更新;
//        /// </summary>
//        private void MonthUpdate(DateTime dateTime)
//        {
//            capital -= maintenanceCosts;        //扣除维护费;
//        }

//    }

//    /// <summary>
//    /// 花费项目;
//    /// </summary>
//    public interface ICapital
//    {

//        /// <summary>
//        /// 设施所属的类型;
//        /// </summary>
//        FacilityType facility { get; }

//        /// <summary>
//        /// 物体价值;
//        /// </summary>
//        int ObjectValue { get; }

//        /// <summary>
//        /// 建造花费;
//        /// </summary>
//        int ConstructionCosts { get; }

//        /// <summary>
//        /// 每月维护费用;
//        /// </summary>
//        int MaintenanceCosts { get; }

//        /// <summary>
//        /// 拆毁花费;
//        /// </summary>
//        int DemolitionCosts { get; }

//    }


//    /// <summary>
//    /// 设施类型;
//    /// </summary>
//    public enum FacilityType
//    {
//        Communal
//    }


//    ///// <summary>
//    ///// 物体价值,物体的效果;
//    ///// </summary>
//    //[Serializable]
//    //public class Capital : ICapital, IXBehaviour
//    //{

//    //    /// <summary>
//    //    /// 设施所属的类型;
//    //    /// </summary>
//    //    [SerializeField]
//    //    private FacilityType facility;

//    //    /// <summary>
//    //    /// 物体价值;
//    //    /// </summary>
//    //    [SerializeField]
//    //    [Tooltip("物体价值;")]
//    //    private int objectValue = 1;

//    //    /// <summary>
//    //    /// 建造花费;负数为减钱,正数位获取到钱;
//    //    /// </summary>
//    //    [SerializeField]
//    //    [Tooltip("建造花费")]
//    //    private int constructionCosts = 1;

//    //    /// <summary>
//    //    /// 每月维护费用;负数为每月加钱,正数为每月减钱;
//    //    /// </summary>
//    //    [SerializeField]
//    //    [Tooltip("维护费用")]
//    //    private int maintenanceCosts = 1;

//    //    /// <summary>
//    //    /// 拆毁获取;负数为减钱,正数位获取到钱;
//    //    /// </summary>
//    //    [SerializeField]
//    //    [Tooltip("拆毁花费")]
//    //    private int demolitionCosts = 1;

//    //    /// <summary>
//    //    /// 设施所属的类型;
//    //    /// </summary>
//    //    public FacilityType Facility
//    //    {
//    //        get { return facility; }
//    //        set { facility = value; }
//    //    }

//    //    /// <summary>
//    //    /// 物体价值;
//    //    /// </summary>
//    //    public int ObjectValue
//    //    {
//    //        get { return objectValue; }
//    //        set { objectValue = value; }
//    //    }

//    //    /// <summary>
//    //    /// 建造花费;负数为减钱,正数位获取到钱;
//    //    /// </summary>
//    //    public int ConstructionCosts
//    //    {
//    //        get { return constructionCosts; }
//    //        set { constructionCosts = value; }
//    //    }

//    //    /// <summary>
//    //    /// 每月维护费用;负数为每月加钱,正数为每月减钱;
//    //    /// </summary>
//    //    public int MaintenanceCosts
//    //    {
//    //        get { return maintenanceCosts; }
//    //        set { maintenanceCosts = value; }
//    //    }

//    //    /// <summary>
//    //    /// 拆毁获取;负数为减钱,正数位获取到钱;
//    //    /// </summary>
//    //    public int DemolitionCosts
//    //    {
//    //        get { return demolitionCosts; }
//    //        set { demolitionCosts = value; }
//    //    }

//    //    FacilityType ICapital.facility { get { return facility; } }
//    //    int ICapital.ObjectValue { get { return objectValue; } }
//    //    int ICapital.ConstructionCosts { get { return constructionCosts; } }
//    //    int ICapital.MaintenanceCosts { get { return maintenanceCosts; } }
//    //    int ICapital.DemolitionCosts { get { return demolitionCosts; } }

//    //    void IXBehaviour.XStart()
//    //    {
//    //        CapitalManager.GetInstance.Add(this);
//    //    }

//    //    void IXBehaviour.XDestroy()
//    //    {
//    //        CapitalManager.GetInstance.Remove(this);
//    //    }

//    //}



//}
