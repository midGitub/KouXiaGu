using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XGame.Running.Capitals
{

    /// <summary>
    /// 花费项目;
    /// </summary>
    public interface ICapital
    {

        /// <summary>
        /// 设施所属的类型;
        /// </summary>
        FacilityType facility { get; }

        /// <summary>
        /// 物体价值;
        /// </summary>
        int ObjectValue { get; }

        /// <summary>
        /// 建造花费;
        /// </summary>
        int ConstructionCosts { get; }

        /// <summary>
        /// 每月维护费用;
        /// </summary>
        int MaintenanceCosts { get; }

        /// <summary>
        /// 拆毁花费;
        /// </summary>
        int DemolitionCosts { get; }

    }

}
