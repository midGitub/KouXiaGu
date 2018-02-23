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
    [Obsolete]
    public class OMapReader
    {
        /// <summary>
        /// 游戏地图存放目录;
        /// </summary>
        [PathDefinition(PathDefinitionType.DataDirectory, "游戏地图存放目录;")]
        internal const string MapsDirectoryName = "Maps";

        internal const string MapRootName = "RectMap";
        internal const string MapNameAttributeName = "Name";
        internal const string MapVersionAttributeName = "Version";
        internal const string MapIsArchivedAttributeName = "isArchived";
        internal const string MapNodeElementName = "Item";
        const string MapFilePrefix = "Map_";
        const string MapFileExtension = ".xmap";

        readonly System.Xml.Serialization.XmlSerializer mapSerializer;
        readonly string mapsDirectory;

        public OMapReader()
        {
            mapSerializer = new System.Xml.Serialization.XmlSerializer(typeof(OMap));
            mapsDirectory = GetMapsDirectory();
        }

        static string mapFileSearchPattern
        {
            get { return MapFilePrefix + "*" + MapFileExtension; }
        }

        /// <summary>
        /// 获取到默认地图存储目录;
        /// </summary>
        string GetMapsDirectory()
        {
            string directory = Path.Combine(Resource.ConfigDirectory, MapsDirectoryName);
            return directory;
        }

        /// <summary>
        /// 读取到地图;
        /// </summary>
        public OMap Read(MapFileInfo mapFileInfo)
        {
            //return Read(mapFileInfo.FileInfo);
            throw new NotImplementedException();
        }

        /// <summary>
        /// 读取到地图;
        /// </summary>
        public OMap Read(FileInfo fileInfo)
        {
            return Read(fileInfo.FullName);
        }

        /// <summary>
        /// 读取到地图;
        /// </summary>
        /// <param name="fullPath">完整的文件路径</param>
        public OMap Read(string fullPath)
        {
            using (Stream stream = new FileStream(fullPath, FileMode.Open, FileAccess.Read))
            {
                return Read(stream);
            }
        }

        /// <summary>
        /// 读取到地图;
        /// </summary>
        public OMap Read(Stream stream)
        {
            return (OMap)mapSerializer.Deserialize(stream);
        }

        internal const bool DefaultIsKeepBackup = false;

        /// <summary>
        /// 输出地图到文件路径,覆盖原有;
        /// </summary>
        public MapFileInfo WriteToFile(string filePath, OMap map, bool isKeepBackup = DefaultIsKeepBackup)
        {
            filePath = Path.ChangeExtension(filePath, MapFileExtension);
            FileInfo fileInfo = new FileInfo(filePath);
            Write(fileInfo, map, isKeepBackup);
            //return new MapFileInfo(fileInfo, map.Description);
            throw new NotImplementedException();
        }

        /// <summary>
        /// 输出地图到目录下,覆盖原有;
        /// </summary>
        public MapFileInfo WriteToDirectory(string directory, OMap map, bool isKeepBackup = DefaultIsKeepBackup)
        {
            string fileName = GetStandardMapFileName(map);
            string filePath = Path.Combine(directory, fileName);
            FileInfo fileInfo = new FileInfo(filePath);
            Write(fileInfo, map, isKeepBackup);
            //return new MapFileInfo(fileInfo, map.Description);
            throw new NotImplementedException();
        }

        /// <summary>
        /// 获取到地图名;
        /// </summary>
        string GetStandardMapFileName(OMap map)
        {
            string fileName = MapFilePrefix + map.Description.Name + MapFileExtension;
            return fileName;
        }

        /// <summary>
        /// 输出地图到,若路径已经存在地图,则覆盖;
        /// </summary>
        /// <param name="mapFileInfo">地图文件信息</param>
        /// <param name="map">需要输出的地图</param>
        /// <param name="isKeepBackup">是否保留备份文件?</param>
        public void Write(MapFileInfo mapFileInfo, OMap map, bool isKeepBackup = DefaultIsKeepBackup)
        {
            //Write(mapFileInfo.FileInfo, map, isKeepBackup);
            //mapFileInfo.Description = map.Description;
            throw new NotImplementedException();
        }

        /// <summary>
        ///  输出地图到,若路径已经存在地图,则覆盖;
        /// </summary>
        /// <param name="file">地图文件信息</param>
        /// <param name="map">需要输出的地图</param>
        /// <param name="isKeepBackup">是否保留备份文件?</param>
        public void Write(FileInfo file, OMap map, bool isKeepBackup = DefaultIsKeepBackup)
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
            catch (Exception ex)
            {
                backupFileInfo?.MoveTo(file.FullName);
                throw ex;
            }
        }

        /// <summary>
        /// 输出地图到;
        /// </summary>
        public void Write(Stream stream, OMap map)
        {
            mapSerializer.SerializeXiaGu(stream, map);
        }


        internal const SearchOption DefaultMapsSearchOption = SearchOption.AllDirectories;

        /// <summary>
        /// 根据地图名获取到对应地图;
        /// </summary>
        public MapFileInfo FindByName(string mapName, SearchOption searchOption = DefaultMapsSearchOption)
        {
            return FindByName(mapName, mapsDirectory, searchOption);
        }

        /// <summary>
        /// 根据地图名获取到对应地图;
        /// </summary>
        public MapFileInfo FindByName(string mapName, string directory, SearchOption searchOption = DefaultMapsSearchOption)
        {
            IEnumerable<MapFileInfo> mapFileInfos = EnumerateMapInfos(directory, searchOption);
            foreach (var mapFileInfo in mapFileInfos)
            {
                if (mapFileInfo.Description.Name == mapName)
                {
                    return mapFileInfo;
                }
            }
            throw new FileNotFoundException(string.Format("在目录[{0}]未找到对应地图:[{1}]", directory, mapName));
        }

        /// <summary>
        /// 迭代获取到所有地图文件信息;
        /// </summary>
        public IEnumerable<MapFileInfo> EnumerateMapInfos(SearchOption searchOption = DefaultMapsSearchOption)
        {
            string directory = GetMapsDirectory();
            return EnumerateMapInfos(directory, searchOption);
        }

        /// <summary>
        /// 迭代获取到所有地图文件信息;
        /// </summary>
        public IEnumerable<MapFileInfo> EnumerateMapInfos(string directory, SearchOption searchOption = DefaultMapsSearchOption)
        {
            foreach (var path in Directory.EnumerateFiles(directory, mapFileSearchPattern, searchOption))
            {
                MapFileInfo mapFileInfo;
                try
                {
                    MapDescription description = ReadInfo(path);
                    FileInfo fileInfo = new FileInfo(path);
                    //mapFileInfo = new MapFileInfo(fileInfo, description);
                    throw new NotImplementedException();
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
                    string version = Convert.ToString(xmlReader.GetAttribute(MapVersionAttributeName));
                    bool isArchived = Convert.ToBoolean(xmlReader.GetAttribute(MapIsArchivedAttributeName));
                    var info = new MapDescription()
                    {
                        Name = name,
                        Version = version,
                    };
                    return info;
                }
            }
            throw new XmlException(string.Format("无法获取到详细信息;{0}", stream));
        }
    }
}
