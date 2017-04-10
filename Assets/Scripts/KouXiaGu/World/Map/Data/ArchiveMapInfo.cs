using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using ProtoBuf;

namespace KouXiaGu.World.Map
{

    [XmlType("MapInfo")]
    public class ArchiveMapInfo
    {
        /// <summary>
        /// 使用的地图ID;
        /// </summary>
        [XmlAttribute("id")]
        public int ID { get; set; }


        public ArchiveMapInfo(MapFile file)
        {
            MapInfo info = file.ReadInfo();
            Initialize(info);
        }

        public ArchiveMapInfo(MapInfo info)
        {
            Initialize(info);
        }

        void Initialize(MapInfo info)
        {
            ID = info.ID;
        }

    }




}
