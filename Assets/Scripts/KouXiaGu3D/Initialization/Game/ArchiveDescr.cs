using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace KouXiaGu.Initialization
{

    [XmlType("ArchiveDescr")]
    public struct ArchiveDescr
    {

        static readonly XmlSerializer serializer = new XmlSerializer(typeof(ArchiveDescr));

        public static XmlSerializer Serializer
        {
            get { return serializer; }
        }

        /// <summary>
        /// 存档名;
        /// </summary>
        [XmlAttribute("name")]
        public string Name { get; set; }

        /// <summary>
        /// 是否允许编辑?
        /// </summary>
        [XmlElement("AllowEdit")]
        public bool AllowEdit { get; set; }

        /// <summary>
        /// 存档版本;
        /// </summary>
        [XmlElement("Version")]
        public string Version { get; set; }

        /// <summary>
        /// 保存存档的真实时间;
        /// </summary>
        [XmlElement("Time")]
        public long Time { get; set; }

        /// <summary>
        /// 用户留言;
        /// </summary>
        [XmlElement("Message")]
        public string Message { get; set; }

    }

}
