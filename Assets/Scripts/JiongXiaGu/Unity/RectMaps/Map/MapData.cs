using JiongXiaGu.Collections;
using JiongXiaGu.Grids;
using ProtoBuf;
using System.Collections.Generic;

namespace JiongXiaGu.Unity.RectMaps
{

    /// <summary>
    /// 游戏地图数据;
    /// </summary>
    [ProtoContract]
    public class MapData 
    {
        /// <summary>
        /// 不对变量进行初始化;
        /// </summary>
        public MapData()
        {
        }

        /// <summary>
        /// 不对变量进行初始化;
        /// </summary>
        /// <param name="isArchive">是否用于存档?</param>
        public MapData(bool isArchive)
        {
            IsArchive = isArchive;
        }

        /// <summary>
        /// 是否为存档?
        /// </summary>
        [ProtoMember(1)]
        public bool IsArchive { get; private set; }

        /// <summary>
        /// 地图节点字典;
        /// </summary>
        [ProtoMember(10)]
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
