using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace KouXiaGu.World.HumanGeography
{

    /// <summary>
    /// 城镇预定义信息;
    /// </summary>
    [XmlType("TownInfo")]
    public class TownPredefinedInfo
    {
        [XmlAttribute("id")]
        public int TownID { get; set; }

        [XmlAttribute("name")]
        public string Name { get; set; }

        [XmlElement("Description")]
        public string Description { get; set; }
    }
}
