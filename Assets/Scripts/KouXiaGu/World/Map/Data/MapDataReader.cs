using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace KouXiaGu.World.Map
{

    /// <summary>
    /// 读取游戏地图方法类;
    /// </summary>
    public abstract class MapDataReader : IReader<MapData>
    {
        internal const string MapFileName = "Map/Map";

        public abstract string FileExtension { get; }
        public abstract MapData Read(string filePath);
        public abstract void Write(string filePath, MapData data);

        public static MapDataReader Create()
        {
            return new ProtoMapReader();
        }

        public MapData Read()
        {
            string filePath = GetFilePath();
            return Read(filePath);
        }

        public void Write(MapData data)
        {
            string filePath = GetFilePath();
            Write(filePath, data);
        }

        public string GetFilePath()
        {
            string filePath = Path.Combine(ResourcePath.ConfigDirectoryPath, MapFileName);
            filePath = Path.ChangeExtension(filePath, FileExtension);
            return filePath;
        }
    }

    /// <summary>
    /// 使用 ProtoBuf 的方式读取游戏地图;
    /// </summary>
    public class ProtoMapReader : MapDataReader
    {
        public override string FileExtension
        {
            get { return ".map"; }
        }

        public override MapData Read(string filePath)
        {
            MapData data = ProtoBufExtensions.Deserialize<MapData>(filePath);
            return data;
        }

        public override void Write(string filePath, MapData data)
        {
            ProtoBufExtensions.Serialize(filePath, data);
        }
    }

}
