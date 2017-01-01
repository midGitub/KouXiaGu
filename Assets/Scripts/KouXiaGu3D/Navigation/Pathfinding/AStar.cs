using System;
using System.Collections.Generic;
using System.Linq;
using KouXiaGu.Collections;
using KouXiaGu.Grids;

namespace KouXiaGu.Navigation
{

    /// <summary>
    /// 寻路;单线程;
    /// </summary>
    public class AStar<TPoint, TNode>
        where TPoint : IGrid
    {

        public AStar()
        {
            openPointsSet = new OpenDictionary<AStartPathNode>();
            closePointsSet = new HashSet<TPoint>();
        }

        /// <summary>
        /// 需要进行搜索的点;
        /// </summary>
        OpenDictionary<AStartPathNode> openPointsSet;

        /// <summary>
        /// 丢弃点合集;
        /// </summary>
        HashSet<TPoint> closePointsSet;

        /// <summary>
        /// 当前正在进行寻路的坐标;
        /// </summary>
        AStartPathNode currentNode;

        /// <summary>
        /// 代价值获取;
        /// </summary>
        public IObstructive<TPoint, TNode> Obstructive { get; set; }

        /// <summary>
        /// 使用的地图;
        /// </summary>
        public IDictionary<TPoint, TNode> WroldMap { get; set; }

        /// <summary>
        /// 寻路范围限制;
        /// </summary>
        public IRange<TPoint> range { get; set; }

        /// <summary>
        /// 起点;
        /// </summary>
        public TPoint Starting { get; private set; }

        /// <summary>
        /// 寻路的终点;
        /// </summary>
        public TPoint Destination { get; private set; }

        /// <summary>
        /// 开始寻路;若无法找到目标点则返回异常;
        /// </summary>
        public LinkedList<TPoint> Start(TPoint starting, TPoint destination)
        {
            Clear();

            this.Starting = starting;
            this.Destination = destination;

            if (IsTrapped(Starting))
                throw new TrappedException("起点周围不可行走,物体可能被困住;");

            if (this.range.IsOutRange(Starting))
                throw new DestinationNotFoundException("目的地超出了最大搜索范围的定义;");

            if (IsTrapped(Destination))
                throw new DestinationNotFoundException("目的地本身或周围不可行走;");

            AddStartingPointToOpenSet();
            return Pathfinding();
        }

        /// <summary>
        /// 清除信息;
        /// </summary>
        void Clear()
        {
            openPointsSet.Clear();
            closePointsSet.Clear();
        }

        /// <summary>
        /// 是否这个点和周围的点都无法行走?
        /// </summary>
        bool IsTrapped(TPoint point)
        {
            TNode worldNode;
            if (WroldMap.TryGetValue(point, out worldNode))
            {
                if (!Obstructive.CanWalk(worldNode))
                    return true;
            }
            if (GetAround(point).Count() == 0)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 加入起点到开放点合集;
        /// </summary>
        void AddStartingPointToOpenSet()
        {
            AStartPathNode startingNode = new AStartPathNode(Starting);
            openPointsSet.Add(startingNode);
        }

        /// <summary>
        /// 开始寻路循环;
        /// </summary>
        LinkedList<TPoint> Pathfinding()
        {
            LinkedList<TPoint> wayPath;

            while (openPointsSet.Count != 0)
            {
                if (TryFindDestinationInOpenSet(out wayPath))
                    return wayPath;

                currentNode = DequeueMinCostInOpenSet();

                AddAroundToOpenSet(currentNode.point);
            }

            throw new DestinationNotFoundException("已经遍历完所有可行走节点,无法到达目的地");
        }

        /// <summary>
        /// 将这个点的周围都加入到开放合集;
        /// </summary>
        void AddAroundToOpenSet(TPoint point)
        {
            var around = GetAround(point);

            foreach (var info in around)
            {
                AddToOpenPoints(currentNode, info.Key, info.Value);
            }
        }

        /// <summary>
        /// 获取到周围的节点;
        /// </summary>
        IEnumerable<KeyValuePair<TPoint, TNode>> GetAround(TPoint point)
        {
            TNode node;
            foreach (var item in point.GetNeighbours())
            {
                TPoint dirPoint = (TPoint)item;
                if (WroldMap.TryGetValue(dirPoint, out node))
                {
                    yield return new KeyValuePair<TPoint, TNode>(dirPoint, node);
                }
            }
        }

        /// <summary>
        /// 加入到开放节点的过滤;
        /// </summary>
        bool WhereAddOpenSet(KeyValuePair<TPoint, TNode> nodePair)
        {
            return !closePointsSet.Contains(nodePair.Key) &&
                Obstructive.CanWalk(nodePair.Value) &&
                !range.IsOutRange(nodePair.Key);
        }

        /// <summary>
        /// 加入到开放点合集;
        /// </summary>
        /// <param name="previous">父节点</param>
        /// <param name="point">当前点的位置</param>
        /// <param name="node">当前节点</param>
        void AddToOpenPoints(AStartPathNode previous, TPoint point, TNode node)
        {
            AStartPathNode beforeNode;
            AStartPathNode newNode;

            if (openPointsSet.TryGetValue(point, out beforeNode))
            {
                beforeNode.TryChangePrevious(currentNode);
            }
            else
            {
                newNode = GetPathNode(previous, point, node);
                openPointsSet.Add(newNode);
            }
        }

        /// <summary>
        /// 获取到A*寻路的点结构;
        /// </summary>
        AStartPathNode GetPathNode(AStartPathNode previous, TPoint point, TNode node)
        {
            float cost = Obstructive.GetCost(point, node, Destination);
            return new AStartPathNode(point, previous, cost);
        }

        /// <summary>
        /// 从开放节点获取到最小的A*寻路点,
        /// 并且从开放合集内移除,加入到关闭合集;
        /// </summary>
        AStartPathNode DequeueMinCostInOpenSet()
        {
            var minNode = openPointsSet.Extract();
            closePointsSet.Add(minNode.point);
#if UNITY_EDITOR_DUBUG
            Debug.Log("minNode :" + minNode.point + " NodeCost" + minNode.nodeCost + " PathCost:" + minNode.PathCost);
#endif
            return minNode;
        }

        /// <summary>
        /// 尝试在开放点合集中获取到终点;获取到了返回true,否则返回false;
        /// </summary>
        bool TryFindDestinationInOpenSet(out LinkedList<TPoint> wayPath)
        {
            AStartPathNode node;
            if (openPointsSet.TryGetValue(Destination, out node))
            {
                wayPath = PathNodeToWayPath(node);
                return true;
            }
            else
            {
                wayPath = default(LinkedList<TPoint>);
                return false;
            }
        }

        /// <summary>
        /// 从A*路径点转换成双向链表;
        /// </summary>
        LinkedList<TPoint> PathNodeToWayPath(AStartPathNode node)
        {
#if UNITY_EDITOR_DUBUG
            Debug.Log("路线代价值:" + node.PathCost + "检索的点:" + (openPointsSet.Count + closePointsSet.Count));
#endif
            LinkedList<TPoint> path = new LinkedList<TPoint>();
            for (; node != null; node = node.Previous)
            {
                path.AddFirst(node.point);
            }
            return path;
        }

        /// <summary>
        /// A*寻路节点;
        /// </summary>
        class AStartPathNode : IComparable<AStartPathNode>
        {

            public AStartPathNode(TPoint point)
            {
                this.point = point;
            }

            public AStartPathNode(TPoint point, AStartPathNode previous, float nodeCost)
            {
                this.nodeCost = nodeCost;
                this.point = point;
                this.Previous = previous;
            }

            /// <summary>
            /// 这个节点在这次寻路中的代价值;
            /// </summary>
            public float nodeCost { get; private set; }

            /// <summary>
            /// 所指的地图坐标;
            /// </summary>
            public TPoint point { get; private set; }

            /// <summary>
            /// 这个点的父节点;
            /// </summary>
            public AStartPathNode Previous { get; private set; }

            /// <summary>
            /// 在路径中的总代价值;
            /// </summary>
            public float PathCost
            {
                get
                {
                    float previousCost = Previous != null ? Previous.PathCost : 0;
                    return previousCost + nodeCost;
                }
            }

            /// <summary>
            /// 尝试替换本结点的 父节点;
            /// 若 挑战者 路径值小于原本 父节点,则替换,并返回true;
            /// </summary>
            public bool TryChangePrevious(AStartPathNode challenger)
            {
                if (Previous.PathCost > challenger.PathCost)
                {
                    Previous = challenger;
                    return true;
                }
                else
                {
                    return false;
                }
            }

            int IComparable<AStartPathNode>.CompareTo(AStartPathNode other)
            {
                if (this.nodeCost < other.nodeCost)
                    return -1;
                else if (this.nodeCost == other.nodeCost)
                    return 0;
                else
                    return 1;
            }

            public override int GetHashCode()
            {
                return point.GetHashCode();
            }

        }

        /// <summary>
        /// 对键使用哈希表记录;
        /// 对值使用最小堆;
        /// </summary>
        class OpenDictionary<TValue>
            where TValue : IComparable<TValue>
        {
            public OpenDictionary()
            {
                keyDictionary = new Dictionary<int, TValue>();
                valueHeap = new MinHeap<TValue>();
            }

            Dictionary<int, TValue> keyDictionary;
            MinHeap<TValue> valueHeap;

            public int Count
            {
                get { return valueHeap.Count; }
            }

            public void Add(TValue item)
            {
                keyDictionary.Add(item.GetHashCode(), item);
                valueHeap.Add(item);
            }

            /// <summary>
            /// 输出最小值并且移除
            /// </summary>
            public TValue Extract()
            {
                TValue min = valueHeap.Extract();
                keyDictionary.Remove(min.GetHashCode());
                return min;
            }

            public bool TryGetValue(object key, out TValue value)
            {
                return keyDictionary.TryGetValue(key.GetHashCode(), out value);
            }

            public void Clear()
            {
                keyDictionary.Clear();
                valueHeap.Clear();
            }

        }

    }

}
