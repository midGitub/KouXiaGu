using System;
using System.Collections.Generic;
using System.Linq;
using KouXiaGu.Collections;
using KouXiaGu.Grids;
using ProtoBuf;

namespace KouXiaGu.World.Map
{

    public abstract class MapReader
    {
        public virtual string SearchPattern
        {
            get { return "*" + FileExtension; }
        }

        public abstract string FileExtension { get; }
        public abstract Map Read(string filePath);
        public abstract void Write(string filePath, Map data);
    }

    public class ProtoMapReader : MapReader
    {
        public override string FileExtension
        {
            get { return ".map"; }
        }

        public override Map Read(string filePath)
        {
            Map data = ProtoBufExtensions.Deserialize<Map>(filePath);
            return data;
        }

        public override void Write(string filePath, Map data)
        {
            ProtoBufExtensions.Serialize(filePath, data);
        }
    }

    [ProtoContract]
    public class Map
    {
        [ProtoMember(1)]
        public ObservableDictionary<CubicHexCoord, MapNode> Data { get; private set; }

        [ProtoMember(2)]
        public RoadInfo Road { get; set; }

        public Map()
        {
            Data = new ObservableDictionary<CubicHexCoord, MapNode>();
            Road = new RoadInfo();
        }

    }

}
