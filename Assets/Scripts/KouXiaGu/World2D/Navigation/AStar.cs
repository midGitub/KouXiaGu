using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KouXiaGu.World2D.Map;
using UnityEngine;

namespace KouXiaGu.World2D.Navigation
{

    /// <summary>
    /// A*寻路;非线程安全;
    /// </summary>
    public class AStar<TNode, TMover>
    {
        AStar()
        {
            openPointsSet = new Dictionary<ShortVector2, AStartPathNode>();
            closePointsSet = new HashSet<ShortVector2>();
            maximumRange = new RectRange();
        }
        public AStar(IObstructive<TNode, TMover> obstructive, IMap<ShortVector2, TNode> wroldMap) : this()
        {
            this.Obstructive = obstructive;
            this.WroldMap = wroldMap;
        }

        /// <summary>
        /// 代价值获取;
        /// </summary>
        public IObstructive<TNode, TMover> Obstructive { get; set; }
        /// <summary>
        /// 地图;
        /// </summary>
        public IMap<ShortVector2, TNode> WroldMap { get; set; }

        /// <summary>
        /// 需要进行搜索的点;
        /// </summary>
        private Dictionary<ShortVector2, AStartPathNode> openPointsSet;
        /// <summary>
        /// 丢弃点合集;
        /// </summary>
        private HashSet<ShortVector2> closePointsSet;
        /// <summary>
        /// 范围限制;
        /// </summary>
        RectRange maximumRange;
        /// <summary>
        /// 当前正在寻路的坐标;
        /// </summary>
        private AStartPathNode currentNode;

        /// <summary>
        /// 进行寻路的人物;
        /// </summary>
        public TMover Mover { get; private set; }
        /// <summary>
        /// 起点;
        /// </summary>
        public ShortVector2 Starting { get; private set; }
        /// <summary>
        /// 寻路的终点;
        /// </summary>
        public ShortVector2 Destination { get; private set; }

        /// <summary>
        /// 开始寻路;若无法找到目标点则返回异常;
        /// </summary>
        public LinkedList<ShortVector2> Start(ShortVector2 starting, ShortVector2 destination, ShortVector2 maximumRange)
        {
            this.Starting = starting;
            this.Destination = destination;
            this.maximumRange.SetMaximumRange(starting, maximumRange);

            if (this.maximumRange.IsOutRange(Starting))
                throw new PathNotFoundException("开始点超出了最大范围的定义");

            AddStartingPointToOpenSet();
            return Pathfinding();
        }

        /// <summary>
        /// 清除信息;
        /// </summary>
        public void Clear()
        {
            openPointsSet.Clear();
            closePointsSet.Clear();
        }

        /// <summary>
        /// 加入起点到开放点合集;
        /// </summary>
        void AddStartingPointToOpenSet()
        {
            AStartPathNode startingNode = new AStartPathNode(Starting);
            openPointsSet.Add(Starting, startingNode);
        }

        /// <summary>
        /// 开始寻路循环;
        /// </summary>
        LinkedList<ShortVector2> Pathfinding()
        {
            LinkedList<ShortVector2> wayPath;

            while (openPointsSet.Count != 0)
            {
                currentNode = GetMinCostInOpenSet();

                AddAroundToOpenSet(currentNode.point);

                if (TryFindDestinationInOpenSet(out wayPath))
                    return wayPath;

                TransferredToCloseSet(currentNode.point);
            }

            throw new PathNotFoundException("已经遍历完所有可行走节点!");
        }

        /// <summary>
        /// 将这个点的周围都加入到开放合集;
        /// </summary>
        void AddAroundToOpenSet(ShortVector2 point)
        {
            var around = WroldMap.GetAround(point).Where(WhereAddOpenSet);

            foreach (KeyValuePair<ShortVector2, TNode> info in around)
            {
                AddToOpenPoints(currentNode, info.Key, info.Value);
            }
        }

        /// <summary>
        /// 加入到开放节点的过滤;
        /// </summary>
        bool WhereAddOpenSet(KeyValuePair<ShortVector2, TNode> nodePair)
        {
            return !closePointsSet.Contains(nodePair.Key) && Obstructive.CanWalk(Mover, nodePair.Value) &&
                    !maximumRange.IsOutRange(nodePair.Key);
        }

        /// <summary>
        /// 加入到开放点合集;
        /// </summary>
        /// <param name="previous">父节点</param>
        /// <param name="point">当前点的位置</param>
        /// <param name="node">当前节点</param>
        void AddToOpenPoints(AStartPathNode previous, ShortVector2 point, TNode node)
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
                openPointsSet.Add(point, newNode);
            }
        }

        /// <summary>
        /// 获取到A*寻路的点结构;
        /// </summary>
        AStartPathNode GetPathNode(AStartPathNode previous, ShortVector2 point, TNode node)
        {
            float cost = Obstructive.GetCost(Mover, point, node, Destination);
            return new AStartPathNode(point, previous, cost);
        }

        /// <summary>
        /// 将这个点从开放合集内移除,且加入到关闭合集;
        /// </summary>
        void TransferredToCloseSet(ShortVector2 point)
        {
            openPointsSet.Remove(point);
            closePointsSet.Add(point);
        }

        /// <summary>
        /// 获取到最小的A*寻路点;
        /// </summary>
        AStartPathNode GetMinCostInOpenSet()
        {
            try
            { 
                var minNode = openPointsSet.Values.Min();
                Debug.Log("minNode :" + minNode.point + " NodeCost" + minNode.nodeCost + " PathCost:" + minNode.PathCost);
                return minNode;
            }
            catch (InvalidOperationException e)
            {
                throw new PathNotFoundException("已经遍历完所有可行走节点!", e);
            }
        }

        /// <summary>
        /// 尝试在开放点合集中获取到终点;获取到了返回true,否则返回false;
        /// </summary>
        bool TryFindDestinationInOpenSet(out LinkedList<ShortVector2> wayPath)
        {
            AStartPathNode node;
            if (openPointsSet.TryGetValue(Destination, out node))
            {
                wayPath = PathNodeToWayPath(node);
                return true;
            }
            else
            {
                wayPath = default(LinkedList<ShortVector2>);
                return false;
            }
        }

        /// <summary>
        /// 从A*路径点转换成双向链表;
        /// </summary>
        LinkedList<ShortVector2> PathNodeToWayPath(AStartPathNode node)
        {
            Debug.Log("路线代价值:" + node.PathCost);
            LinkedList<ShortVector2> path = new LinkedList<ShortVector2>();
            for (; node != null; node = node.Previous)
            {
                path.AddFirst(node.point);
            }
            return path;
        }

        /// <summary>
        /// A*寻路点;
        /// </summary>
        class AStartPathNode : IComparable<AStartPathNode>
        {
            public AStartPathNode(ShortVector2 position)
            {
                this.point = position;
            }

            public AStartPathNode(ShortVector2 position, AStartPathNode previous, float nodeCost)
            {
                this.nodeCost = nodeCost;
                this.point = position;
                this.Previous = previous;
            }

            /// <summary>
            /// 这个节点在这次寻路中的代价值;
            /// </summary>
            public float nodeCost { get; private set; }
            /// <summary>
            /// 在路径中的总代价值;
            /// </summary>
            public float PathCost
            {
                get
                {
                    float PreviousCost = Previous != null ? Previous.PathCost : 0;
                    return PreviousCost + nodeCost;
                }
            }
            /// <summary>
            /// 所指的地图坐标;
            /// </summary>
            public ShortVector2 point { get; private set; }
            /// <summary>
            /// 这个点的父节点;
            /// </summary>
            public AStartPathNode Previous { get; private set; }

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

        }

    }

}
