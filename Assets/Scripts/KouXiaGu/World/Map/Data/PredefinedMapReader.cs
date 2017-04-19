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
    /// 读取游戏预制地图;
    /// </summary>
    public abstract class PredefinedMapReader : IReader<PredefinedMap>
    {
        public PredefinedMapReader(string fileExtension)
        {
            File = new PredefinedMapFilePath(fileExtension);
        }

        public FilePath File { get; private set; }
        public abstract PredefinedMap ReadMap(string filePath);
        public abstract void Write(PredefinedMap data, string filePath);

        public PredefinedMap Read()
        {
            string filePath = File.MainFilePath;
            return ReadMap(filePath);
        }

        public void Write(PredefinedMap data)
        {
            string filePath = File.MainFilePath;
            Write(data, filePath);
        }
    }


    public class PredefinedMapProtoReader : PredefinedMapReader
    {
        public const string fileExtension = ".map";

        public PredefinedMapProtoReader() 
            : base(fileExtension)
        {
        }

        public override PredefinedMap ReadMap(string filePath)
        {
            PredefinedMap data = ProtoBufExtensions.Deserialize<PredefinedMap>(filePath);
            return data;
        }

        public override void Write(PredefinedMap data, string filePath)
        {
            ProtoBufExtensions.Serialize(filePath, data);
        }
    }

}
