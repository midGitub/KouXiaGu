using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace KouXiaGu.Navigation
{

    [XmlType("Building")]
    public struct NavigationBuildingInfo
    {
        /// <summary>
        /// 是否可行走?
        /// </summary>
        [XmlElement("IsWalkable")]
        public bool IsWalkable;

        /// <summary>
        /// 经过这个地形的代价值;
        /// </summary>
        [XmlElement("Cost")]
        public int Cost;

        /// <summary>
        /// 移动速度影响;
        /// </summary>
        [XmlElement("MovementRates")]
        public float MovementRates;
    }
}
