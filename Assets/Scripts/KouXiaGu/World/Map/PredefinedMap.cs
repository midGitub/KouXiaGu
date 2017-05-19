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
    public sealed class PredefinedMap
    {

        [ProtoMember(1)]
        public ObservableDictionary<CubicHexCoord, MapNode> Data { get; private set; }

        [ProtoMember(2)]
        public MapRoad Road { get; private set; }

        [ProtoMember(3)]
        public MapTown Town { get; private set; }

        [ProtoMember(4)]
        public IdentifierGenerator BuildingIdentifierGenerator { get; private set; }

        public bool IsReadOnly
        {
            get { throw new NotImplementedException(); }
            private set { throw new NotImplementedException(); }
        }

        public PredefinedMap()
        {
            //Data = new ObservableDictionary<CubicHexCoord, MapNode>();
            Road = new MapRoad();
            Town = new MapTown();
            Enable();
        }

        /// <summary>
        /// 更新地图内容,并允许编辑地图;
        /// </summary>
        public void Update(ArchiveMap archive)
        {
            Disable();

            Data.AddOrUpdate(archive.Data);
            Road = archive.Road;
            Town = archive.Town;

            Enable();
        }

        void Enable()
        {
            if (IsReadOnly)
            {
                try
                {
                    Town.Subscribe(Data);
                }
                finally
                {
                    IsReadOnly = false;
                }
            }
        }

        void Disable()
        {
            if (!IsReadOnly)
            {
                try
                {
                    Town.Unsubscribe();
                }
                finally
                {
                    IsReadOnly = true;
                }
            }
        }

    }

}
