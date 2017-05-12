using KouXiaGu.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace KouXiaGu.Terrain3D
{

    [Serializable]
    public class DynamicWall
    {
        public DynamicWall()
        {
            nodeList = new List<Node>();
        }

        List<Node> nodeList;

        public List<Node> Nodes
        {
            get { return nodeList; }
        }

        /// <summary>
        /// 构建节点记录;
        /// </summary>
        public void Build(Vector3[] vertices, float spacing)
        {
            SortedList<Vector3> verticeSortedList = new SortedList<Vector3>(vertices, VerticeComparer_x.instance);
            Node currentNode = CreateNode(verticeSortedList, 0);

            for (int index = 0; index < verticeSortedList.Count; index++)
            {
                Vector3 point = verticeSortedList[index];
                if (point.x - currentNode.Position.x > spacing)
                {
                    currentNode = CreateNode(verticeSortedList, index);
                }
                currentNode.Add(point);
            }
        }

        Node CreateNode(SortedList<Vector3> verticeSortedList, int index)
        {
            Vector3 point = verticeSortedList[index];
            float start = verticeSortedList[0].x;
            float end = verticeSortedList[verticeSortedList.Count - 1].x;
            float interpolatedValue = (point.x - start) / (end - start);
            Vector3 nodePosition = new Vector3(point.x, 0, 0);
            var node = new Node(nodePosition, interpolatedValue);
            nodeList.Add(node);
            return node;
        }

        /// <summary>
        /// 获取到所有原始的顶点坐标;
        /// </summary>
        public IEnumerable<Vector3> GetOriginalVertices()
        {
            foreach (var node in nodeList)
            {
                var originalVertices = node.GetOriginalVertices();
                foreach (var originalVertice in originalVertices)
                {
                    yield return originalVertice;
                }
            }
        }

        /// <summary>
        /// 对比坐标的x值;
        /// </summary>
        class VerticeComparer_x : IComparer<Vector3>
        {
            public static readonly VerticeComparer_x instance = new VerticeComparer_x();

            /// <summary>
            /// 小于零  x 小于 y。
            /// 零      x 等于 y。
            /// 大于零  x 大于 y。
            /// </summary>
            public int Compare(Vector3 x, Vector3 y)
            {
                return (x.x - y.x) < 0 ? -1 : 1;
            }
        }

        /// <summary>
        /// 节点记录;
        /// </summary>
        [Serializable]
        public class Node
        {
            public Node(Vector3 position, float interpolatedValue)
            {
                this.position = position;
                this.interpolatedValue = interpolatedValue;
                points = new List<Point>();
            }

            Vector3 position;
            float interpolatedValue;
            List<Point> points;

            public Vector3 Position
            {
                get { return position; }
            }

            public void Add(Vector3 childPosition)
            {
                Vector3 localPosition = position - childPosition;
                float localAngle = AngleY(position, childPosition);
                Point point = new Point(localPosition, localAngle);
                points.Add(point);
            }

            /// <summary>
            /// 返回弧度;
            /// </summary>
            float AngleY(Vector3 from, Vector3 to)
            {
                return Mathf.Atan2((to.x - from.x), (to.z - from.z));
            }

            /// <summary>
            /// 获取到所有原始的顶点坐标;
            /// </summary>
            public IEnumerable<Vector3> GetOriginalVertices()
            {
                foreach (var point in points)
                {
                    yield return position + point.LocalPosition;
                }
            }
        }

        /// <summary>
        /// 点记录;
        /// </summary>
        [Serializable]
        public struct Point
        {
            public Point(Vector3 localPosition, float localAngle)
            {
                this.localPosition = localPosition;
                this.localAngle = localAngle;
            }

            /// <summary>
            /// 相对于父节点的位置;
            /// </summary>
            Vector3 localPosition;

            /// <summary>
            /// 相对于父节点的角度;
            /// </summary>
            float localAngle;

            public Vector3 LocalPosition
            {
                get { return localPosition; }
            }

            public float LocalAngle
            {
                get { return localAngle; }
            }

            public override string ToString()
            {
                return "[Position:" + localPosition + ",Angle:" + localAngle + "]";
            }
        }
    }
}
