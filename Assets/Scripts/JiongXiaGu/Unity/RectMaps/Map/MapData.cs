using System;
using JiongXiaGu.Collections;
using JiongXiaGu.Grids;
using ProtoBuf;
using System.Collections.Generic;

namespace JiongXiaGu.Unity.RectMaps
{

    /// <summary>
    /// 地图存档数据;
    /// </summary>
    [ProtoContract]
    public class ArchiveData
    {
        /// <summary>
        /// 地图节点字典;
        /// </summary>
        [ProtoMember(1)]
        public Dictionary<RectCoord, MapNode> Data { get; internal set; }
    }

    /// <summary>
    /// 游戏地图数据;
    /// </summary>
    [ProtoContract]
    public class MapData 
    {
        /// <summary>
        /// 地图节点字典;
        /// </summary>
        [ProtoMember(1)]
        public Dictionary<RectCoord, MapNode> Data { get; internal set; }

        /// <summary>
        /// 添加存档数据;
        /// </summary>
        public void AddArchive(ArchiveData archiveData)
        {
            if (archiveData == null)
                throw new ArgumentNullException("archiveData");

            Data.AddOrUpdate(archiveData.Data);
        }
    }
}
