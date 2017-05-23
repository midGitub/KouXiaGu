using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace KouXiaGu.Navigation
{

    [XmlType("Road")]
    public class NavigationRoadInfo
    {
        /// <summary>
        /// 代价值加成;
        /// </summary>
        [XmlElement("RoadCost")]
        public int RoadCost;
    }
}
