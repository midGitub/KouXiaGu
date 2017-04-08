using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace KouXiaGu.World.Map
{

    /// <summary>
    /// 获取地图文件;
    /// </summary>
    public class MapFileManager
    {

        MapInfoReader defaultInfoReader
        {
            get { return MapInfoReader.DefaultReader; }
        }

        public virtual string DefaultMapsDirectory
        {
            get { return Path.Combine(ResourcePath.ConfigDirectoryPath, "Maps"); }
        }

        public virtual IEnumerable<MapFile> SearchAll(string dirPath, SearchOption searchOption = SearchOption.TopDirectoryOnly)
        {
            var filePaths = Directory.GetFiles(dirPath, defaultInfoReader.FileSearchPattern, searchOption);

            foreach (var filePath in filePaths)
            {
                MapInfo info;
                if (TryReadInfo(filePath, out info))
                {
                    MapFile file = new MapFile(filePath, info);
                    yield return file;
                }
            }
        }

        bool TryReadInfo(string filePath, out MapInfo info)
        {
            try
            {
                info = defaultInfoReader.Read(filePath);
                return true;
            }
            catch
            {
                info = default(MapInfo);
                return false;
            }
        }

        public MapFile Create(MapInfo Info)
        {
            string dirPath = Path.Combine(DefaultMapsDirectory, Info.ID.ToString());

            if (Directory.Exists(dirPath) && Directory.GetFiles(dirPath).Length != 0)
                throw new ArgumentException("已经存在相同ID的地图;ID:" + Info.ID);

            return Create(dirPath, Info);
        }

        public virtual MapFile Create(string dirPath, MapInfo Info)
        {
            Directory.CreateDirectory(dirPath);
            string infoPath = Path.Combine(dirPath, Info.Name.ToString());
            Path.ChangeExtension(infoPath, defaultInfoReader.FileExtension);

            var file = new MapFile(infoPath, Info);
            file.WriteInfo();
            return file;
        }

    }

}
