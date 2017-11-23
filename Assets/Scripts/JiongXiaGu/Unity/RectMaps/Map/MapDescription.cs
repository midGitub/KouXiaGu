using System.Xml.Serialization;

namespace JiongXiaGu.Unity.RectMaps
{

    /// <summary>
    /// 地图描述;
    /// </summary>
    [XmlRoot("MapDescription")]
    public struct MapDescription
    {
        /// <summary>
        /// 唯一标识;
        /// </summary>
        [XmlElement]
        public string ID { get; set; }

        /// <summary>
        /// 名称;
        /// </summary>
        [XmlElement]
        public string Name { get; set; }

        /// <summary>
        /// 作者;
        /// </summary>
        [XmlElement]
        public string Author { get; set; }

        /// <summary>
        /// 版本;
        /// </summary>
        [XmlElement]
        public string Version { get; set; }

        /// <summary>
        /// 预留消息;
        /// </summary>
        [XmlElement]
        public string Message { get; set; }

        public MapDescription(string id) : this()
        {
            ID = id;
        }
    }
}
