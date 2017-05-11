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

        }


        List<Node> nodeList;


        /// <summary>
        /// 构建节点记录;
        /// </summary>
        public void Build(Vector3[] vertices)
        {
            SortedList<Vector3> verticeSortedList = new SortedList<Vector3>(vertices, VerticeComparer_x.instance);

            foreach (var vertice in verticeSortedList)
            {

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
        class Node
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

        }

        /// <summary>
        /// 点记录;
        /// </summary>
        [Serializable]
        class Point
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

            public override string ToString()
            {
                return "[Position:" + localPosition + ",Angle:" + localAngle + "]";
            }

        }

    }

}
