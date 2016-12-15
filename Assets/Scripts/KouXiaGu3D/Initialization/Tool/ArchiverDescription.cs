using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace KouXiaGu.Initialization
{

    /// <summary>
    /// 存档描述文件;
    /// </summary>
    public class ArchiveDescription
    {
        /// <summary>
        /// 存档文件名;
        /// </summary>
        const string FILE_NAME = "Description.xml";





        [XmlType("Description")]
        public struct Description
        {

            /// <summary>
            /// 保存存档的真实时间;
            /// </summary>
            [XmlElement("Time")]
            public long Time;

            /// <summary>
            /// 存档名;
            /// </summary>
            [XmlElement("Name")]
            public string Name;

            /// <summary>
            /// 用户留言;
            /// </summary>
            [XmlElement("Message")]
            public string Message;

        }

    }

}
