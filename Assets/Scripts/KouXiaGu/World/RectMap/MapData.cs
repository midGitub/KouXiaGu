using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KouXiaGu.Grids;
using ProtoBuf;

namespace KouXiaGu.World.RectMap
{

    /// <summary>
    /// 游戏地图数据;
    /// </summary>
    [ProtoContract]
    public class MapData 
    {
        public MapData()
        {
            Data = new Dictionary<RectCoord, MapNode>();
        }

        [ProtoMember(1)]
        public Dictionary<RectCoord, MapNode> Data { get; set; }

        /// <summary>
        /// 整合其它地图数据;
        /// </summary>
        public void Add(MapData other)
        {
            if (other != this)
            {
                Data.AddOrUpdate(other.Data);
            }
        }
    }
}
