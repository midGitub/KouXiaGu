using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KouXiaGu.World.Map
{

    public abstract class ArchiveMapReader
    {
        static readonly ArchiveMapReader defaultReader = new ArchiveMapProtoReader();

        public static ArchiveMapReader DefaultReader
        {
            get { return defaultReader; }
        }

        public abstract string FileExtension { get; }
        public abstract ArchiveMap Read(string filePath);
        public abstract void Write(string filePath, ArchiveMap data);
    }

    public class ArchiveMapProtoReader : ArchiveMapReader
    {
        public override string FileExtension
        {
            get { return ".aMap"; }
        }

        public override ArchiveMap Read(string filePath)
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
