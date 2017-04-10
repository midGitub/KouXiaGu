using System;
using System.Collections.Generic;
using System.Linq;
using ProtoBuf;
using KouXiaGu.Grids;
using KouXiaGu.Rx;
using UnityEngine;

namespace KouXiaGu.World.Map
{

    /// <summary>
    /// 节点城镇信息;
    /// </summary>
    [ProtoContract]
    public struct TownNode
    {
        /// <summary>
        /// 所属城镇编号;
        /// </summary>
        [ProtoMember(1)]
        public int TownID;
    }

    [ProtoContract]
    public class MapTown : DictionaryObserver<CubicHexCoord, MapNode>
    {

        internal const int EmptyTownID = 0;

        /// <summary>
        /// 映射城镇坐标;
        /// </summary>
        [ProtoMember(1)]
        Dictionary<int, CubicHexCoord> townMap;

        public MapTown()
        {
            townMap = new Dictionary<int, CubicHexCoord>();
        }

        public MapTown(MapData map) : this()
        {
            Subscribe(map.Data);
        }

        void AddTown(CubicHexCoord pos, TownNode node)
        {
            int townID = node.TownID;
            try
            {
                townMap.Add(townID, pos);
            }
            catch (ArgumentException)
            {
                Debug.LogWarning("请求添加一个已存在的城镇,检查代码;TownID:" + townID);
            }
        }

        void RemoveTown(CubicHexCoord pos, TownNode node)
        {
            int townID = node.TownID;
            if (!townMap.Remove(townID))
            {
                Debug.LogWarning("移除不存在的城镇;");
            }
        }

        public override void OnAdded(CubicHexCoord key, MapNode newValue)
        {
            if (newValue.ExistTown())
            {
                AddTown(key, newValue.Town);
            }
        }

        public override void OnRemoved(CubicHexCoord key, MapNode originalValue)
        {
            if (originalValue.ExistTown())
            {
                RemoveTown(key, originalValue.Town); 
            }
        }

        public override void OnUpdated(CubicHexCoord key, MapNode originalValue, MapNode newValue)
        {
            if (originalValue.Town.TownID != newValue.Town.TownID)
            {
                if (originalValue.ExistTown())
                {
                    RemoveTown(key, originalValue.Town);
                }
                if (newValue.ExistTown())
                {
                    AddTown(key, newValue.Town);
                }
            }
        }

    }

    public static class TownExtensions
    {

        public static bool ExistTown(this MapNode node)
        {
            return node.Town.ExistTown();
        }

        public static bool ExistTown(this TownNode node)
        {
            return node.TownID != MapTown.EmptyTownID;
        }

    }

}
