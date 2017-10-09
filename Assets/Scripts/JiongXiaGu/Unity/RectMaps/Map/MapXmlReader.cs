using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace JiongXiaGu.Unity.RectMaps
{

    /// <summary>
    /// 提供地图读写方法;
    /// </summary>
    public class MapXmlReader
    {
        internal const string MapRootName = "RectMap";
        internal const string MapNameAttributeName = "Name";
        internal const string MapVersionAttributeName = "Version";
        internal const string MapIsArchivedAttributeName = "isArchived";
        internal const string MapNodeElementName = "Item";
        const string MapFilePrefix = "Map_";
        const string MapFileExtension = ".xmap";

        XmlSerializer mapSerializer;

        public MapXmlReader()
        {
            mapSerializer = new XmlSerializer(typeof(Map));
        }

        static string mapFileSearchPattern
        {
            get { return MapFilePrefix + "*" + MapFileExtension; }
        }

        /// <summary>
        /// 读取到地图;
        /// </summary>
        public Map Read(Stream stream)
        {
            return (Map)mapSerializer.Deserialize(stream);
        }

        /// <summary>
        /// 读取到地图;
        /// </summary>
        public Map Read(MapDataXmlFileInfo file)
        {
            using (Stream stream = new FileStream(file.File.FullName, FileMode.Create, FileAccess.Read))
            {
                return Read(stream);
            }
        }

        /// <summary>
        /// 输出地图到目录下,覆盖原有;
        /// </summary>
        public void Write(string dirPath, Map map)
        {
            string fileName = MapFilePrefix + map.Name + MapFileExtension;
            string filePath = Path.Combine(dirPath, fileName);
            using (Stream stream = new FileStream(filePath, FileMode.Create, FileAccess.Write))
            {
                Write(stream, map);
            }
        }

        /// <summary>
        /// 输出地图到;
        /// </summary>
        public void Write(Stream stream, Map map)
        {
            mapSerializer.SerializeXiaGu(stream, map);
        }

        /// <summary>
        /// 输出地图到;
        /// </summary>
        public void Write(MapDataXmlFileInfo file, Map map)
        {
            string tempFilePath = file.File.FullName + ".temp";
            FileInfo tempFileInfo = new FileInfo(tempFilePath);
            try
            {
                using (Stream stream = new FileStream(tempFilePath, FileMode.Create, FileAccess.Write))
                {
                    Write(stream, map);
                }
                tempFileInfo.MoveTo(file.File.FullName);
                file.File = tempFileInfo;
                file.Description = map.Description;
            }
            catch(Exception ex)
            {
                tempFileInfo.Delete();
                throw ex;
            }
        }

        /// <summary>
        /// 迭代获取到所有地图文件信息;
        /// </summary>
        public static IEnumerable<MapDataXmlFileInfo> EnumerateInfos(string directory, SearchOption searchOption)
        {
            foreach (var path in Directory.EnumerateFiles(directory, mapFileSearchPattern, searchOption))
            {
                MapDataXmlFileInfo info;

                try
                {
                    info = ReadInfo(path);
                }
                catch
                {
                    continue;
                }

                yield return info;
                info = null;
            }
        }

        /// <summary>
        /// 读取到地图文件信息;
        /// </summary>
        public static MapDataXmlFileInfo ReadInfo(string filePath)
        {
            FileInfo fileInfo = new FileInfo(filePath);
            using (Stream stream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                MapDescription description = ReadInfo(stream);
                return new MapDataXmlFileInfo(fileInfo, description);
            }
        }

        public static MapDescription ReadInfo(Stream stream)
        {
            using (XmlReader xmlReader = XmlReader.Create(stream))
            {
                xmlReader.MoveToContent();
                if (xmlReader.IsStartElement() && xmlReader.Name == MapRootName)
                {
                    string name = xmlReader.GetAttribute(MapNameAttributeName);
                    int version = Convert.ToInt32(xmlReader.GetAttribute(MapVersionAttributeName));
                    bool isArchived = Convert.ToBoolean(xmlReader.GetAttribute(MapIsArchivedAttributeName));
                    var info = new MapDescription()
                    {
                        Name = name,
                        Version = version,
                        IsArchived = isArchived,
                    };
                    return info;
                }
            }
            throw new XmlException(string.Format("无法获取到详细信息;{0}", stream));
        }
    }
}
