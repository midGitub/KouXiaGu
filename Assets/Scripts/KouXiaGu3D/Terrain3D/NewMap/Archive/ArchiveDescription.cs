using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace KouXiaGu.Terrain3D
{


    [XmlType("Terrain")]
    public struct ArchiveDescription
    {
        static readonly XmlSerializer serializer = new XmlSerializer(typeof(ArchiveDescription));

        public static XmlSerializer Serializer
        {
            get { return serializer; }
        }


        /// <summary>
        /// 使用的地形地图ID;
        /// </summary>
        [XmlElement("UseMapID")]
        public int UseMapID { get; set; }


    }

}
