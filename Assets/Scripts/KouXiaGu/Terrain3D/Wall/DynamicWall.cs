using KouXiaGu.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace KouXiaGu.Terrain3D
{

    public class DynamicWall
    {

        /// <summary>
        /// 构建节点记录;
        /// </summary>
        public void Build(Vector3[] vertices)
        {
            SortedList<Vector3> verticeSortedList = new SortedList<Vector3>(vertices, VerticeComparer_x.instance);


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
        class Node
        {
            public Node(float interpolatedValue)
            {
                this.interpolatedValue = interpolatedValue;
            }

            /// <summary>
            /// 插入值;
            /// </summary>
            float interpolatedValue;
            List<Point> points;
        }

        /// <summary>
        /// 点记录;
        /// </summary>
        class Point
        {
            /// <summary>
            /// 相对于父节点的角度;
            /// </summary>
            float localAngle;

            /// <summary>
            /// 相对于父节点的位置;
            /// </summary>
            Vector3 localPosition;
        }

    }

}
