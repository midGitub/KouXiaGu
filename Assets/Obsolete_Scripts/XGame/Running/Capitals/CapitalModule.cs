using UnityEngine;

namespace XGame.Running.Capitals
{

    /// <summary>
    /// 花费组件;挂载在物体上;
    /// </summary>
    public class CapitalModule : MonoBehaviour, ICapital, IXBehaviour
    {

        [Header("建造花费;")]

        /// <summary>
        /// 设施所属的类型;
        /// </summary>
        [SerializeField]
        private FacilityType facility;

        /// <summary>
        /// 物体价值;
        /// </summary>
        [SerializeField]
        [Tooltip("物体价值;")]
        private int objectValue = 1;

        /// <summary>
        /// 建造花费;负数为减钱,正数位获取到钱;
        /// </summary>
        [SerializeField]
        [Tooltip("建造花费")]
        private int constructionCosts = 1;

        /// <summary>
        /// 每月维护费用;负数为每月加钱,正数为每月减钱;
        /// </summary>
        [SerializeField]
        [Tooltip("维护费用")]
        private int maintenanceCosts = 1;

        /// <summary>
        /// 拆毁获取;负数为减钱,正数位获取到钱;
        /// </summary>
        [SerializeField]
        [Tooltip("拆毁花费")]
        private int demolitionCosts = 1;

        /// <summary>
        /// 设施所属的类型;
        /// </summary>
        public FacilityType Facility
        {
            get { return facility; }
            set { facility = value; }
        }

        /// <summary>
        /// 物体价值;
        /// </summary>
        public int ObjectValue
        {
            get { return objectValue; }
            set { objectValue = value; }
        }

        /// <summary>
        /// 建造花费;负数为减钱,正数位获取到钱;
        /// </summary>
        public int ConstructionCosts
        {
            get { return constructionCosts; }
            set { constructionCosts = value; }
        }

        /// <summary>
        /// 每月维护费用;负数为每月加钱,正数为每月减钱;
        /// </summary>
        public int MaintenanceCosts
        {
            get { return maintenanceCosts; }
            set { maintenanceCosts = value; }
        }

        /// <summary>
        /// 拆毁获取;负数为减钱,正数位获取到钱;
        /// </summary>
        public int DemolitionCosts
        {
            get { return demolitionCosts; }
            set { demolitionCosts = value; }
        }

        FacilityType ICapital.facility { get { return facility; } }
        int ICapital.ObjectValue { get { return objectValue; } }
        int ICapital.ConstructionCosts { get { return constructionCosts; } }
        int ICapital.MaintenanceCosts { get { return maintenanceCosts; } }
        int ICapital.DemolitionCosts { get { return demolitionCosts; } }

        void IXBehaviour.XOnEnable()
        {
            CapitalController.GetInstance.Add(this);
        }

        void IXBehaviour.XOnDisable()
        {
            CapitalController.GetInstance.Remove(this);
        }

    }

}
