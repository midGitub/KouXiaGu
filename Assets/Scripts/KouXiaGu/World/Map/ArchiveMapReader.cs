using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace KouXiaGu.World.Map
{

    class ArchiveMapFilePath : ArchiveFilePath
    {
        public ArchiveMapFilePath(string fileExtension) : base(fileExtension)
        {
        }

        public override string FileName
        {
            get { return "World/Map"; }
        }
    }

    public abstract class ArchiveMapReader
    {
        public ArchiveMapReader(string fileExtension)
        {
            File = new ArchiveMapFilePath(fileExtension);
        }

        public ArchiveFilePath File { get; private set; }
        public abstract ArchiveMap ReadMap(string filePath);
        public abstract void Write(string filePath, ArchiveMap data);

        public ArchiveMap Read(string archiveDir)
        {
            string filePath = File.GetFilePath(archiveDir);
            return ReadMap(filePath);
        }

        public void Write(ArchiveMap data, string archiveDir)
        {
            string filePath = File.GetFilePath(archiveDir);
            Write(filePath, data);
        }
    }

    public class ArchiveMapProtoReader : ArchiveMapReader
    {
        public const string fileExtension = ".save";

        public ArchiveMapProtoReader() 
            : base(fileExtension)
        {
        }

        public override ArchiveMap ReadMap(string filePath)
        {
            ArchiveMap data = ProtoBufExtensions.Deserialize<ArchiveMap>(filePath);
            return data;
        }

        public override void Write(string filePath, ArchiveMap data)
        {
            ProtoBufExtensions.Serialize(filePath, data);
        }
    }

}
