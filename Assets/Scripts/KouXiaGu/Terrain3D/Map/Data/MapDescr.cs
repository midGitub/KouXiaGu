using System.IO;
using System.Xml.Serialization;
using ProtoBuf;

namespace KouXiaGu.Terrain3D
{


    [ProtoContract, XmlType("Terrain")]
    public struct MapDescr
    {

        /// <summary>
        /// 存储文件后缀名;
        /// </summary>
        public const string XML_FILE_EXTENSION = ".xml";

        static readonly XmlSerializer serializer = new XmlSerializer(typeof(MapDescr));

        /// <summary>
        /// 输出到文件;
        /// </summary>
        public static void XmlWrite(string filePath, MapDescr map)
        {
            filePath = Path.ChangeExtension(filePath, XML_FILE_EXTENSION);
            serializer.SerializeXiaGu(filePath, map);
        }

        /// <summary>
        /// 从文件读取到;
        /// </summary>
        public static MapDescr XmlRead(string filePath)
        {
            filePath = Path.ChangeExtension(filePath, XML_FILE_EXTENSION);
            return (MapDescr)serializer.DeserializeXiaGu(filePath);
        }


        /// <summary>
        /// 存储文件后缀名;
        /// </summary>
        public const string FILE_EXTENSION = ".data";

        /// <summary>
        /// 输出到文件;
        /// </summary>
        public static void Write(string filePath, MapDescr map)
        {
            filePath = Path.ChangeExtension(filePath, FILE_EXTENSION);
            ProtoBufExtensions.SerializeXiaGu(filePath, map);
        }

        /// <summary>
        /// 从文件读取到;
        /// </summary>
        public static MapDescr Read(string filePath)
        {
            filePath = Path.ChangeExtension(filePath, FILE_EXTENSION);
            return ProtoBufExtensions.DeserializeXiaGu<MapDescr>(filePath);
        }


        /// <summary>
        /// 地图道路信息;
        /// </summary>
        [ProtoMember(10), XmlElement("RoadInfo")]
        public RoadDescription RoadInfo;

    }

}
