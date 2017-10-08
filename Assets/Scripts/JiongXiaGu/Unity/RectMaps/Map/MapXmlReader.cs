using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace JiongXiaGu.Unity.RectMaps
{


    /// <summary>
    /// 提供地图读写方法;
    /// </summary>
    public class MapXmlReader
    {

        internal const string MapRootName = "RectMap";
        internal const string MapDescriptionElementName = "Description";
        internal const string MapDataElementName = "Data";

        XmlSerializer mapSerializer;

        public MapXmlReader()
        {
            mapSerializer = new XmlSerializer(typeof(Map));
        }

        /// <summary>
        /// 读取到地图;
        /// </summary>
        public Map Read(Stream stream)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 读取到地图;
        /// </summary>
        public Map Read(MapDataXmlFileInfo file)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 输出地图到;
        /// </summary>
        public void Write(Stream stream, Map map)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 输出地图到;
        /// </summary>
        public void Write(MapDataXmlFileInfo file, Map map)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 迭代获取到所有地图文件信息;
        /// </summary>
        public static MapDataXmlFileInfo EnumerateInfos(string directory, SearchOption searchOption)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 读取到地图描述信息;
        /// </summary>
        public static MapDescription ReadDescription(Stream stream)
        {
            throw new NotImplementedException();
        }
    }
}
