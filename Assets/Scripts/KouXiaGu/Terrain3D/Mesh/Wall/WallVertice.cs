using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace KouXiaGu.Terrain3D
{

    /// <summary>
    /// 记录墙的所有顶点信息;
    /// </summary>
    [Serializable]
    public class WallVertice
    {

        public WallVertice()
        {
        }

        /// <summary>
        /// 构造函数;
        /// </summary>
        /// <param name="vertices">原本的顶点数据;</param>
        /// <param name="wall">分段后的顶点数据;</param>
        public WallVertice(Vector3[] vertices, List<WallSection> wall)
        {
            this.wall = wall;
            this.vertices = vertices;
        }


        /// <summary>
        /// 墙体顶点数据;
        /// </summary>
        [SerializeField]
        List<WallSection> wall;

        /// <summary>
        /// 顶点数组;
        /// </summary>
        [SerializeField]
        Vector3[] vertices;


        /// <summary>
        /// 墙体顶点数据;
        /// </summary>
        public List<WallSection> Wall
        {
            get { return wall; }
            set { wall = value; }
        }

        /// <summary>
        /// 原本的顶点数据;
        /// </summary>
        public Vector3[] Vertices
        {
            get { return vertices; }
            set { vertices = value; }
        }


        /// <summary>
        /// 重新计算墙的顶点坐标,并且返回;
        /// </summary>
        public Vector3[] Recalculate(ISpline spline)
        {
            float d = 0;
            float increment = 1 / wall.Count;
            Vector3[] newVertices = (Vector3[])vertices.Clone();

            for (int i = 0; i < wall.Count; i++)
            {
                Vector3 anchorPoint = spline.InterpolatedPoint(d);
                d += increment;
                Vector3 nextPoint = spline.InterpolatedPoint(d);

                WallSection section = wall[i];
                section.Recalculate(anchorPoint, nextPoint);

                Replace(ref newVertices, section);
            }

            return newVertices;
        }

        /// <summary>
        /// 将分段信息替换到数组;
        /// </summary>
        void Replace(ref Vector3[] vertices, WallSection section)
        {
            foreach (var vertice in section.CheckPoints)
            {
                vertices[vertice.ID] = vertice.Point;
            }
        }

        /// <summary>
        /// 获取到定义的顶点总数;
        /// </summary>
        public int GetCount()
        {
            return wall.Sum(item => item.Count);
        }

    }

}
