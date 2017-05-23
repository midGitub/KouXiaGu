using KouXiaGu.Collections;
using KouXiaGu.Grids;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KouXiaGu.Navigation
{

    /// <summary>
    /// 寻路;
    /// </summary>
    public class Pathfinder<T>
    {
        public Pathfinder()
        {
            openPointsSet = new OpenDictionary();
            closePointsSet = new HashSet<T>();
        }

        /// <summary>
        /// 需要进行搜索的点;
        /// </summary>
        OpenDictionary openPointsSet;

        /// <summary>
        /// 丢弃点合集;
        /// </summary>
        HashSet<T> closePointsSet;

        /// <summary>
        /// 代价值获取;
        /// </summary>
        public INavigationMap<T> Map { get; private set; }

        /// <summary>
        /// 起点;
        /// </summary>
        public T Starting { get; private set; }

        /// <summary>
        /// 寻路的终点;
        /// </summary>
        public T Destination { get; private set; }

        /// <summary>
        /// 寻路范围限制;
        /// </summary>
        public IRange<T> SearchRange { get; private set; }

        /// <summary>
        /// 开始寻路;若无法找到目标点则返回异常;
        /// </summary>
        public WayPath<T> Search(INavigationMap<T> map, T starting, T destination)
        {
            return Search(map, UnlimitedRange<T>.Instance, starting, destination);
        }

        /// <summary>
        /// 开始寻路;若无法找到目标点则返回异常;
        /// </summary>
        public WayPath<T> Search(INavigationMap<T> map, IRange<T> searchRange, T starting, T destination)
        {
            if (map == null)
                throw new ArgumentNullException("map");
            if (searchRange == null)
                throw new ArgumentNullException("searchRange");

            Map = map;
            SearchRange = searchRange;
            Starting = starting;
            Destination = destination;

            if (SearchRange.IsOutRange(Destination))
                throw new DestinationNotFoundException("目的地超出了最大搜索范围的定义;");
            if (IsTrapped(Starting))
                throw new TrappedException("起点周围不可行走,物体可能被困住;");
            if (IsTrapped(Destination))
                throw new TrappedException("目的地本身或周围不可行走;");

            return Search();
        }

        /// <summary>
        /// 是否这个点的周围点都无法行走?
        /// </summary>
        bool IsTrapped(T position)
        {
            var neighbors = Map.GetNeighbors(position);
            foreach (var neighbor in neighbors)
            {
                if (Map.CanWalk(neighbor))
                {
                    return false;
                }
            }
            return true;
        }

        WayPath<T> Search()
        {
            PathNode currentNode = new PathNode(Starting);
            openPointsSet.Add(currentNode);

            while (openPointsSet.Count != 0)
            {
                PathNode destinationNode;
                if (openPointsSet.TryGetValue(Destination, out destinationNode))
                {
                    var wayPath = PathNodeToWayPath(destinationNode);
                    openPointsSet.Clear();
                    closePointsSet.Clear();
                    return wayPath;
                }

                currentNode = openPointsSet.Extract();
                closePointsSet.Add(currentNode.Position);
                OpenNeighbors(currentNode);
            }
            throw new DestinationNotFoundException("已经遍历完所有可行走节点,无法到达目的地");
        }

        /// <summary>
        /// 从A*路径点转换成双向链表;
        /// </summary>
        WayPath<T> PathNodeToWayPath(PathNode node)
        {
#if UNITY_EDITOR_DUBUG
            Debug.Log("路线代价值:" + node.PathCost + "检索的点:" + (openPointsSet.Count + closePointsSet.Count));
#endif
            var wayPath = new WayPath<T>();
            while (node != null)
            {
                wayPath.AddFirst(node.Position);
                node = node.Parent;
            }
            return wayPath;
        }

        /// <summary>
        /// 将这个点的周围都加入到开放合集;
        /// </summary>
        void OpenNeighbors(PathNode parent)
        {
            var neighbors = Map.GetNeighbors(parent.Position);
            foreach (var neighbor in neighbors)
            {
                if (!closePointsSet.Contains(neighbor))
                {
                    PathNode neighborNode;
                    if (IsOutRange(neighbor))
                    {
                        closePointsSet.Add(neighbor);
                    }
                    else if (openPointsSet.TryGetValue(neighbor, out neighborNode))
                    {
                        neighborNode.TryChangeParent(parent);
                    }
                    else
                    {
                        NodeInfo<T> info = Map.GetInfo(neighbor, Destination);
                        if (info.IsWalkable)
                        {
                            neighborNode = new PathNode(parent, info);
                            openPointsSet.Add(neighborNode);
                        }
                        else
                        {
                            closePointsSet.Add(neighbor);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 这个位置是否在搜索范围之外?
        /// </summary>
        bool IsOutRange(T position)
        {
            return SearchRange.IsOutRange(position);
        }

        /// <summary>
        /// 寻路节点;
        /// </summary>
        class PathNode : IComparable<PathNode>
        {
            public PathNode(T position)
            {
                Parent = null;
                Position = position;
                NodeCost = 0;
            }

            public PathNode(PathNode parent, NodeInfo<T> info)
            {
                Parent = parent;
                Position = info.Position;
                NodeCost = info.Cost;
            }

            /// <summary>
            /// 所指的地图坐标;
            /// </summary>
            public T Position { get; private set; }

            /// <summary>
            /// 这个点的父节点;
            /// </summary>
            public PathNode Parent { get; private set; }

            /// <summary>
            /// 这个节点在这次寻路中的代价值;
            /// </summary>
            public int NodeCost { get; private set; }

            /// <summary>
            /// 路径总代价值;
            /// </summary>
            public int GetPathCost()
            {
                int previousCost = Parent != null ? Parent.GetPathCost() : 0;
                return previousCost + NodeCost;
            }

            /// <summary>
            /// 尝试替换本结点的 父节点;
            /// 若 挑战者 路径值小于原本 父节点,则替换,并返回true;
            /// </summary>
            public bool TryChangeParent(PathNode newParent)
            {
                if (Parent.GetPathCost() > newParent.GetPathCost())
                {
                    Parent = newParent;
                    return true;
                }
                else
                {
                    return false;
                }
            }

            int IComparable<PathNode>.CompareTo(PathNode other)
            {
                if (NodeCost < other.NodeCost)
                    return -1;
                else if (NodeCost == other.NodeCost)
                    return 0;
                else
                    return 1;
            }

            public override int GetHashCode()
            {
                return Position.GetHashCode();
            }
        }

        /// <summary>
        /// 对键使用哈希表记录;
        /// 对值使用最小堆;
        /// </summary>
        class OpenDictionary
        {
            public OpenDictionary()
            {
                keyDictionary = new Dictionary<T, PathNode>();
                valueHeap = new BinaryHeap<PathNode>();
            }

            Dictionary<T, PathNode> keyDictionary;
            BinaryHeap<PathNode> valueHeap;

            public int Count
            {
                get { return valueHeap.Count; }
            }

            public void Add(PathNode item)
            {
                keyDictionary.Add(item.Position, item);
                valueHeap.Add(item);
            }

            /// <summary>
            /// 输出最小值并且移除
            /// </summary>
            public PathNode Extract()
            {
                PathNode min = valueHeap.Extract();
                keyDictionary.Remove(min.Position);
                return min;
            }

            public bool TryGetValue(T position, out PathNode node)
            {
                return keyDictionary.TryGetValue(position, out node);
            }

            public void Clear()
            {
                keyDictionary.Clear();
                valueHeap.Clear();
            }
        }
    }
}
