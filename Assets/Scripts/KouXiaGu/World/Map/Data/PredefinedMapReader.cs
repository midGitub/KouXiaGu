using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace KouXiaGu.World.Map
{

    class PredefinedMapFilePath : FilePath
    {
        public PredefinedMapFilePath(string fileExtension) : base(fileExtension)
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
    public class PredefinedMapReader : IReader<PredefinedMap>
    {
        internal static readonly PredefinedMapReader instance = new PredefinedMapReader();

        public PredefinedMapReader()
        {
            File = new PredefinedMapFilePath(FileExtension);
        }

        internal PredefinedMapFilePath File { get; private set; }

        public string FileExtension
        {
            get { return ".map"; }
        }

        public PredefinedMap Read()
        {
            string filePath = GetFilePath();
            return Read(filePath);
        }

        public string GetFilePath()
        {
            string filePath = Path.ChangeExtension(File.MainFilePath, FileExtension);
            return filePath;
        }

        public PredefinedMap Read(string filePath)
        {
            PredefinedMap data = ProtoBufExtensions.Deserialize<PredefinedMap>(filePath);
            return data;
        }


        public void Write(PredefinedMap data)
        {
            string filePath = GetFilePath();
            Write(filePath, data);
        }

        public void Write(string filePath, PredefinedMap data)
        {
            ProtoBufExtensions.Serialize(filePath, data);
        }
    }

}
