using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace KouXiaGu.Terrain3D.Navigation
{

    /// <summary>
    /// 地形导航信息;
    /// </summary>
    [XmlType("Navigation")]
    public struct NavigationDescr
    {

        static readonly XmlSerializer arraySerializer = new XmlSerializer(typeof(NavigationDescr[]));

        public static XmlSerializer ArraySerializer
        {
            get { return arraySerializer; }
        }

        /// <summary>
        /// 关联的地貌ID;
        /// </summary>
        [XmlAttribute("landform")]
        public int Landform { get; set; }

        /// <summary>
        /// 可行走的;
        /// </summary>
        [XmlElement("Walkable")]
        public bool Walkable { get; set; }

        /// <summary>
        /// 行走速度(百分比);
        /// </summary>
        [XmlElement("SpeedOfTravel")]
        public float SpeedOfTravel { get; set; }

        /// <summary>
        /// 寻路时的代价值;
        /// </summary>
        [XmlElement("NavigationCost")]
        public float NavigationCost { get; set; }

    }

}
