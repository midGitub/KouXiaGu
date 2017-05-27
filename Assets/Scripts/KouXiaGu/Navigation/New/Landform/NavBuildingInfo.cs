using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace KouXiaGu.Navigation
{

    [XmlType("Building")]
    public struct NavBuildingInfo
    {
        /// <summary>
        /// 是否影响导航?
        /// </summary>
        [XmlElement("IsInfluenceNavigation")]
        public bool IsInfluenceNavigation;

        /// <summary>
        /// 导航信息;
        /// </summary>
        [XmlElement("Navigation")]
        public NavLandformInfo Navigation;
    }
}
