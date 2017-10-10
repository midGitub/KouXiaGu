using JiongXiaGu.Unity.Resources;
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace JiongXiaGu.Unity.RectMaps
{

    /// <summary>
    /// 提供地图读写方法;
    /// </summary>
    public class MapReader
    {
        /// <summary>
        /// 游戏地图存放目录;
        /// </summary>
        [PathDefinition(ResourceTypes.DataDirectory, "游戏地图存放目录;")]
        internal const string MapsDirectoryName = "Maps";

        internal const string MapRootName = "RectMap";
        internal const string MapNameAttributeName = "Name";
        internal const string MapVersionAttributeName = "Version";
        internal const string MapIsArchivedAttributeName = "isArchived";
        internal const string MapNodeElementName = "Item";
        const string MapFilePrefix = "Map_";
        const string MapFileExtension = ".xmap";

        XmlSerializer mapSerializer;

        public MapReader()
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
        public Map Read(MapFileInfo mapFileInfo)
        {
            return Read(mapFileInfo.FileInfo);
        }

        /// <summary>
        /// 读取到地图;
        /// </summary>
        public Map Read(FileInfo fileInfo)
        {
            return Read(fileInfo.FullName);
        }

        /// <summary>
        /// 读取到地图;
        /// </summary>
        /// <param name="fullPath">完整的文件路径</param>
        public Map Read(string fullPath)
        {
            using (Stream stream = new FileStream(fullPath, FileMode.Open, FileAccess.Read))
            {
                return Read(stream);
            }
        }

        /// <summary>
        /// 读取到地图;
        /// </summary>
        public Map Read(Stream stream)
        {
            return (Map)mapSerializer.Deserialize(stream);
        }

        internal const bool DefaultIsKeepBackup = false;

        /// <summary>
        /// 输出地图到文件路径,覆盖原有;
        /// </summary>
        public MapFileInfo WriteToFile(string filePath, Map map, bool isKeepBackup = DefaultIsKeepBackup)
        {
            filePath = Path.ChangeExtension(filePath, MapFileExtension);
            FileInfo fileInfo = new FileInfo(filePath);
            Write(fileInfo, map, isKeepBackup);
            return new MapFileInfo(fileInfo, map.Description);
        }

        /// <summary>
        /// 输出地图到目录下,覆盖原有;
        /// </summary>
        public MapFileInfo WriteToDirectory(string directory, Map map, bool isKeepBackup = DefaultIsKeepBackup)
        {
            string fileName = GetStandardMapFileName(map);
            string filePath = Path.Combine(directory, fileName);
            FileInfo fileInfo = new FileInfo(filePath);
            Write(fileInfo, map, isKeepBackup);
            return new MapFileInfo(fileInfo, map.Description);
        }

        /// <summary>
        /// 获取到地图名;
        /// </summary>
        string GetStandardMapFileName(Map map)
        {
            string fileName = MapFilePrefix + map.Name + MapFileExtension;
            return fileName;
        }

        /// <summary>
        /// 输出地图到,若路径已经存在地图,则覆盖;
        /// </summary>
        /// <param name="mapFileInfo">地图文件信息</param>
        /// <param name="map">需要输出的地图</param>
        /// <param name="isKeepBackup">是否保留备份文件?</param>
        public void Write(MapFileInfo mapFileInfo, Map map, bool isKeepBackup = DefaultIsKeepBackup)
        {
            Write(mapFileInfo.FileInfo, map, isKeepBackup);
            mapFileInfo.Description = map.Description;
        }

        /// <summary>
        ///  输出地图到,若路径已经存在地图,则覆盖;
        /// </summary>
        /// <param name="file">地图文件信息</param>
        /// <param name="map">需要输出的地图</param>
        /// <param name="isKeepBackup">是否保留备份文件?</param>
        public void Write(FileInfo file, Map map, bool isKeepBackup = DefaultIsKeepBackup)
        {
            FileInfo backupFileInfo = null;
            if (file.Exists)
            {
                backupFileInfo = new FileInfo(file.FullName);
                string backupPath = file.FullName + ".backup";
                backupFileInfo.MoveTo(backupPath);
            }
            try
            {
                using (Stream stream = new FileStream(file.FullName, FileMode.Create, FileAccess.Write))
                {
                    Write(stream, map);
                }

                if (!isKeepBackup)
                    backupFileInfo?.Delete();
            }
            catch(Exception ex)
            {
                backupFileInfo?.MoveTo(file.FullName);
                throw ex;
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
        /// 迭代获取到所有地图文件信息;
        /// </summary>
        public IEnumerable<MapFileInfo> EnumerateMapInfos(SearchOption searchOption = SearchOption.TopDirectoryOnly)
        {
            string directory = GetMapsDirectory();
            return EnumerateMapInfos(directory, searchOption);
        }

        /// <summary>
        /// 获取到默认地图存储目录;
        /// </summary>
        string GetMapsDirectory()
        {
            string directory = Path.Combine(Resource.DataDirectoryPath, MapsDirectoryName);
            return directory;
        }

        /// <summary>
        /// 迭代获取到所有地图文件信息;
        /// </summary>
        public IEnumerable<MapFileInfo> EnumerateMapInfos(string directory, SearchOption searchOption = SearchOption.TopDirectoryOnly)
        {
            foreach (var path in Directory.EnumerateFiles(directory, mapFileSearchPattern, searchOption))
            {
                MapFileInfo mapFileInfo;
                try
                {
                    MapDescription description = ReadInfo(path);
                    FileInfo fileInfo = new FileInfo(path);
                    mapFileInfo = new MapFileInfo(fileInfo, description);
                }
                catch
                {
                    continue;
                }
                yield return mapFileInfo;
                mapFileInfo = null;
            }
        }

        /// <summary>
        /// 读取到地图文件信息;
        /// </summary>
        public MapDescription ReadInfo(string filePath)
        {
            using (Stream stream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                MapDescription description = ReadInfo(stream);
                return description;
            }
        }

        public MapDescription ReadInfo(Stream stream)
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
