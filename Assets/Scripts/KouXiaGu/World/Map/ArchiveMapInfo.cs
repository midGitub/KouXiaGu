using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using ProtoBuf;

namespace KouXiaGu.World.Map
{

    [XmlType("MapInfo")]
    public struct ArchiveMapInfo
    {
        /// <summary>
        /// 使用的地图ID;
        /// </summary>
        [XmlAttribute("id")]
        public int ID { get; set; }
    }

}
