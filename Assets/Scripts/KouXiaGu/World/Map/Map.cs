using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using KouXiaGu.Collections;
using KouXiaGu.Grids;
using KouXiaGu.Rx;
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

        public bool IsActivated { get; private set; }

        public Map()
        {
            Data = new ObservableDictionary<CubicHexCoord, MapNode>();
            Road = new RoadInfo();
            Town = new TownInfo();
            IsActivated = false;
        }

        /// <summary>
        /// 更新地图内容,并且取消启用状态;
        /// </summary>
        public void Update(ArchiveMap archive)
        {
            Disable();

            Data.AddOrUpdate(archive.Data);
            Road = archive.Road;
            Town = archive.Town;
        }

        /// <summary>
        /// 初始化地图,需要手动调用;
        /// </summary>
        public void Enable()
        {
            if (!IsActivated)
            {
                try
                {
                    Town.Subscribe(Data);
                }
                finally
                {
                    IsActivated = true;
                }
            }
        }

        public void Disable()
        {
            if (IsActivated)
            {
                try
                {
                    Town.Unsubscribe();
                }
                finally
                {
                    IsActivated = false;
                }
            }
        }

    }

}
