using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace KouXiaGu.World.Map
{

    class MapDataFilePath : FilePath
    {
        public MapDataFilePath(string fileExtension) : base(fileExtension)
        {
        }

        public override string FileName
        {
            get { return "World/Map"; }
        }
    }

    /// <summary>
    /// 读取游戏地图方法类;
    /// </summary>
    public class MapDataReader : IReader<MapData>
    {
        internal static readonly MapDataReader instance = new MapDataReader();

        public MapDataReader()
        {
            File = new MapDataFilePath(FileExtension);
        }

        internal MapDataFilePath File { get; private set; }

        public string FileExtension
        {
            get { return ".map"; }
        }

        public MapData Read()
        {
            string filePath = GetFilePath();
            return Read(filePath);
        }

        public string GetFilePath()
        {
            string filePath = Path.ChangeExtension(File.MainFilePath, FileExtension);
            return filePath;
        }

        public MapData Read(string filePath)
        {
            MapData data = ProtoBufExtensions.Deserialize<MapData>(filePath);
            return data;
        }


        public void Write(MapData data)
        {
            string filePath = GetFilePath();
            Write(filePath, data);
        }

        public void Write(string filePath, MapData data)
        {
            ProtoBufExtensions.Serialize(filePath, data);
        }
    }

}
