using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using KouXiaGu.Collections;
using KouXiaGu.Grids;
using ProtoBuf;

namespace KouXiaGu.World.Map
{

    /// <summary>
    /// 游戏地图数据;
    /// </summary>
    [ProtoContract]
    public class Map
    {

        [ProtoMember(1)]
        public ObservableDictionary<CubicHexCoord, MapNode> Data { get; private set; }

        [ProtoMember(2)]
        public RoadInfo Road { get; private set; }

        [ProtoMember(3)]
        public TownInfo Town { get; private set; }

        public Map()
        {
            Data = new ObservableDictionary<CubicHexCoord, MapNode>();
            Road = new RoadInfo();
            Town = new TownInfo();
        }

        public void Update(ArchiveMap archive)
        {
            Data.AddOrUpdate(archive.Data);
            Road = archive.Road;
        }

    }

}
