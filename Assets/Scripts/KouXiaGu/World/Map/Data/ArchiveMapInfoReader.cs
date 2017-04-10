using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace KouXiaGu.World.Map
{

    public abstract class ArchiveMapInfoReader
    {
        public static readonly ArchiveMapInfoReader defaultReader = new XmlArchiveMapInfoReader();

        public static ArchiveMapInfoReader DefaultReader
        {
            get { return defaultReader; }
        }

        public abstract string FileExtension { get; }
        public abstract ArchiveMapInfo Read(string filePath);
        public abstract void Write(string filePath, ArchiveMapInfo data);
    }

    public class XmlArchiveMapInfoReader : ArchiveMapInfoReader
    {
        static readonly XmlSerializer serializer = new XmlSerializer(typeof(ArchiveMapInfo));

        public override string FileExtension
        {
            get { return ".xml"; }
        }

        public override ArchiveMapInfo Read(string filePath)
        {
            var item = (ArchiveMapInfo)serializer.DeserializeXiaGu(filePath);
            return item;
        }

        public override void Write(string filePath, ArchiveMapInfo data)
        {
            serializer.SerializeXiaGu(filePath, data);
        }
    }

}
