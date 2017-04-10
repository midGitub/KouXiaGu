using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace KouXiaGu.World.Map
{

    public abstract class MapInfoReader
    {
        static readonly MapInfoReader defaultReader = new XmlMapInfoReader();

        public static MapInfoReader DefaultReader
        {
            get { return defaultReader; }
        }

        public virtual string FileSearchPattern
        {
            get { return "*" + FileExtension; }
        }

        public abstract string FileExtension { get; }
        public abstract MapInfo Read(string filePath);
        public abstract void Write(string filePath, MapInfo data);
    }

    public class XmlMapInfoReader : MapInfoReader
    {
        static readonly XmlSerializer serializer = new XmlSerializer(typeof(MapInfo));

        public override string FileExtension
        {
            get { return ".xml"; }
        }

        public override MapInfo Read(string filePath)
        {
            return (MapInfo)serializer.DeserializeXiaGu(filePath);
        }

        public override void Write(string filePath, MapInfo data)
        {
            serializer.SerializeXiaGu(filePath, data);
        }
    }

}
