using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace KouXiaGu.Initialization
{

    [XmlType("Archive")]
    public struct ArchiveDescription
    {

        /// <summary>
        /// 存储文件后缀名;
        /// </summary>
        public const string FILE_EXTENSION = ".xml";

        static readonly XmlSerializer serializer = new XmlSerializer(typeof(ArchiveDescription));

        public static XmlSerializer Serializer
        {
            get { return serializer; }
        }

        /// <summary>
        /// 输出到文件;
        /// </summary>
        public static void Write(string filePath, ArchiveDescription item)
        {
            filePath = Path.ChangeExtension(filePath, FILE_EXTENSION);
            serializer.SerializeXiaGu(item, filePath);
        }

        /// <summary>
        /// 从文件读取到;
        /// </summary>
        public static ArchiveDescription Read(string filePath)
        {
            filePath = Path.ChangeExtension(filePath, FILE_EXTENSION);
            return (ArchiveDescription)serializer.DeserializeXiaGu(filePath);
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
