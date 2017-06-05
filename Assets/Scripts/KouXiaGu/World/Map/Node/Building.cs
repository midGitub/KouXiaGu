using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KouXiaGu.World.Map
{

    /// <summary>
    /// 节点建筑信息;
    /// </summary>
    [ProtoContract]
    public struct BuildingNode : IEquatable<BuildingNode>
    {
        /// <summary>
        /// 不存在建筑时放置的标志;
        /// </summary>
        public const int EmptyMark = 0;

        /// <summary>
        /// 编号,不存在建筑则为0;
        /// </summary>
        [ProtoMember(0)]
        public uint ID { get; internal set; }

        [ProtoMember(1)]
        List<BuildingItem> items;

        /// <summary>
        /// 存在的建筑;
        /// </summary>
        internal List<BuildingItem> Items
        {
            get { return items; }
            set { items = value; }
        }

        /// <summary>
        /// 存在的建筑;
        /// </summary>
        public IEnumerable<BuildingItem> BuildingItems
        {
            get { return Items; }
        }

        /// <summary>
        /// 添加建筑物;
        /// </summary>
        public BuildingNode Add(MapData data, BuildingItem item)
        {
            return Add(data.Building, item);
        }

        /// <summary>
        /// 添加建筑物;
        /// </summary>
        public BuildingNode Add(IdentifierGenerator buildingInfo, BuildingItem item)
        {
            if (Items == null)
            {
                ID = buildingInfo.GetNewEffectiveID();
                items = new List<BuildingItem>();
            }
            items.Add(item);
            return this;
        }

        /// <summary>
        /// 移除建筑物;
        /// </summary>
        public BuildingNode Remove(int buildingType)
        {
            items.Remove(item => item.BuildingType == buildingType);
            return this;
        }

        /// <summary>
        /// 是否存在该建筑物?
        /// </summary>
        public bool Exist(int buildingType)
        {
            return items.Contains(item => item.BuildingType == buildingType);
        }

        /// <summary>
        /// 是否存在建筑物?
        /// </summary>
        public bool Exist()
        {
            return ID != EmptyMark;
        }

        /// <summary>
        /// 清除建筑信息;
        /// </summary>
        public BuildingNode Destroy()
        {
            ID = 0;
            items = null;
            return default(BuildingNode);
        }

        public bool Equals(BuildingNode other)
        {
            return ID == other.ID
                && Equals(items, other.items);
        }

        bool Equals(List<BuildingItem> items, List<BuildingItem> other)
        {
            foreach (var item in items)
            {
                if (!other.Contains(item))
                {
                    return false;
                }
            }
            return true;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is BuildingNode))
                return false;
            return Equals((BuildingNode)obj);
        }

        public override int GetHashCode()
        {
            return ID.GetHashCode();
        }

        public static bool operator ==(BuildingNode a, BuildingNode b)
        {
            return a.Equals(b);
        }

        public static bool operator !=(BuildingNode a, BuildingNode b)
        {
            return !a.Equals(b);
        }
    }

    public struct BuildingItem : IEquatable<BuildingItem>
    {
        public BuildingItem(int buildingType, float angle)
        {
            BuildingType = buildingType;
            Angle = angle;
        }

        public int BuildingType { get; internal set; }
        public float Angle { get; internal set; }

        public bool Equals(BuildingItem other)
        {
            return BuildingType == other.BuildingType
            && Angle == other.Angle;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is BuildingItem))
            {
                return false;
            }
            return Equals((BuildingItem)obj);
        }

        public override int GetHashCode()
        {
            return BuildingType.GetHashCode();
        }

        public override string ToString()
        {
            return "[BuildingType:" + BuildingType + ", Angle:" + Angle + "]";
        }
    }
}
