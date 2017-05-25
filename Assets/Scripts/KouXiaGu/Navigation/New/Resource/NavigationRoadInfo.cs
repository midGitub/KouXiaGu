using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace KouXiaGu.Navigation
{

    [XmlType("Road")]
    public struct NavigationRoadInfo
    {
        /// <summary>
        /// 代价值加成;
        /// </summary>
        [XmlElement("RoadCost")]
        public int Cost;

        /// <summary>
        /// 移动速度影响;
        /// </summary>
        [XmlElement("MovementRates")]
        public float MovementRates;
    }
}
