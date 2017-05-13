using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using KouXiaGu.Collections;

namespace KouXiaGu.Terrain3D
{

    /// <summary>
    /// 动态墙体网格;
    /// </summary>
    [DisallowMultipleComponent]
    [RequireComponent(typeof(MeshFilter))]
    public sealed class DynamicWallMesh : MonoBehaviour
    {
        DynamicWallMesh()
        {
        }

        [SerializeField]
        float spacing;
        [SerializeField]
        DynamicWallSectionInfo dynamicWall;

        public DynamicWallSectionInfo DynamicWall
        {
            get { return dynamicWall; }
        }

        [ContextMenu("Build")]
        void Build()
        {
            MeshFilter meshFilter = GetComponent<MeshFilter>();
            Mesh mesh = meshFilter.sharedMesh;
            Build(mesh.vertices);
        }

        /// <summary>
        /// 构建节点记录;
        /// </summary>
        void Build(Vector3[] vertices)
        {
            dynamicWall = new DynamicWallSectionInfo(vertices, spacing);
        }

        /// <summary>
        /// 更改顶点坐标到曲线;
        /// </summary>
        void Transformation(ref Vector3[] vertices, ISpline spline)
        {
            foreach (var section in dynamicWall.Sections)
            {
                Vector3 newSection = spline.InterpolatedPoint(section.InterpolatedValue);
                foreach (var childIndex in section.Children)
                {
                    Vector3 newVertice = DynamicWall.Points[childIndex].LocalPosition + newSection;
                    vertices[childIndex] = newVertice;
                }
            }
        }

        [ContextMenu("Test")]
        void Test()
        {
            MeshFilter meshFilter = GetComponent<MeshFilter>();
            Mesh mesh = meshFilter.sharedMesh;
            Vector3[] vertices = mesh.vertices;
            ISpline spline = new CatmullRomSpline(
                new Vector3(-1, 0, 0),
                new Vector3(0, 0, 0),
                new Vector3(1, 0, 0),
                new Vector3(2, 0, 0)
                );

            ISpline spline2 = new CatmullRomSpline(
                new Vector3(2, 0, 0),
                new Vector3(1, 0, 0),
                new Vector3(0, 0, 0),
                new Vector3(-1, 0, 0)
                );
            Transformation(ref vertices, spline2);
            mesh.vertices = vertices;
        }

    }

}
