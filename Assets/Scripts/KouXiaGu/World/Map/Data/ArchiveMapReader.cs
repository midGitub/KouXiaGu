using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace KouXiaGu.World.Map
{

    public abstract class ArchiveMapReader : IReader<ArchiveMap>
    {
        internal const string MapFileName = "Map/Map";

        public abstract string FileExtension { get; }
        public string ArchiveDir { get; private set; }

        public ArchiveMapReader(string archiveDir)
        {
            ArchiveDir = archiveDir;
        }

        public static ArchiveMapReader Create(string archiveDir)
        {
            return new ArchiveMapProtoReader(archiveDir);
        }

        public abstract ArchiveMap ReadMap(string filePath);
        public abstract void Write(string filePath, ArchiveMap data);

        public ArchiveMap Read()
        {
            string filePath = GetFilePath();
            return ReadMap(filePath);
        }

        public void Write(ArchiveMap data)
        {
            string filePath = GetFilePath();
            Write(filePath, data);
        }

        string GetFilePath()
        {
            string filePath = Path.Combine(ArchiveDir, MapFileName);
            filePath = Path.ChangeExtension(filePath, FileExtension);
            return filePath;
        }

    }

    public class ArchiveMapProtoReader : ArchiveMapReader
    {
        public ArchiveMapProtoReader(string archiveDir) : base(archiveDir)
        {
        }

        public override string FileExtension
        {
            get { return ".save"; }
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
