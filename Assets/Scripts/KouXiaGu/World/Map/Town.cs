using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ProtoBuf;
using KouXiaGu.Collections;
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
    public class TownInfo : DictionaryObserver<CubicHexCoord, MapNode>
    {

        public const int EmptyTownID = 0;

        /// <summary>
        /// 映射城镇坐标;
        /// </summary>
        [ProtoMember(2)]
        Dictionary<int, CubicHexCoord> townMap;

        public IEnumerable<int> Towns
        {
            get { return townMap.Keys; }
        }

        public TownInfo()
        {
            townMap = new Dictionary<int, CubicHexCoord>();
        }

        public TownInfo(Map map) : this()
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

        protected override void Add(CubicHexCoord key, MapNode newValue)
        {
            if (newValue.ExistTown())
            {
                AddTown(key, newValue.Town);
            }
        }

        protected override void Remove(CubicHexCoord key, MapNode originalValue)
        {
            if (originalValue.ExistTown())
            {
                RemoveTown(key, originalValue.Town); 
            }
        }

        protected override void Update(CubicHexCoord key, MapNode originalValue, MapNode newValue)
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

        public static bool ExistTown(this TownNode node)
        {
            return node.TownID != TownInfo.EmptyTownID;
        }

        public static bool ExistTown(this MapNode node)
        {
            return node.Town.TownID != TownInfo.EmptyTownID;
        }


    }

}
