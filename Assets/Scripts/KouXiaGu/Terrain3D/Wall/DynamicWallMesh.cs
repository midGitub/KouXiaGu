using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using KouXiaGu.Collections;

namespace KouXiaGu.Terrain3D.Wall
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

        MeshFilter _meshFilter;
        [SerializeField]
        float spacing;
        [SerializeField]
        DynamicWall dynamicWall;

        public DynamicWall WallInfo
        {
            get { return dynamicWall; }
        }

        MeshFilter meshFilter
        {
            get { return _meshFilter == null ? (_meshFilter = GetComponent<MeshFilter>()) : _meshFilter; }
        }

        [ContextMenu("Build")]
        void Build()
        {
            Mesh mesh = meshFilter.sharedMesh;
            Build(mesh.vertices, spacing);
        }

        /// <summary>
        /// 构建节点记录;
        /// </summary>
        void Build(Vector3[] vertices, float spacing)
        {
            JointInfo jointInfo = new JointInfo(vertices, spacing);
            dynamicWall = new DynamicWall(jointInfo, vertices);
        }

        /// <summary>
        /// 更改顶点坐标到曲线;
        /// </summary>
        void Transformation(ISpline spline, ref Vector3[] vertices)
        {
            dynamicWall.Transformation(spline, ref vertices);
        }

        /// <summary>
        /// 更改节点坐标;
        /// </summary>
        /// <param name="sectionIndex">节点坐标下标;</param>
        /// <param name="position">更改后的位置;</param>
        public void ChangeSection(int sectionIndex, Vector3 position)
        {
            Mesh mesh = meshFilter.sharedMesh;
            Vector3[] vertices = mesh.vertices;
            WallInfo.TransformSection(sectionIndex, position, ref vertices);
            mesh.vertices = vertices;
        }


        static readonly ISpline spline = new CatmullRomSpline(
                new Vector3(-1, 0, 0),
                new Vector3(0, 0, 1),
                new Vector3(1, 0, 0),
                new Vector3(2, 0, -1)
            );

        static readonly ISpline spline2 = new CatmullRomSpline(
                new Vector3(0, 0, -1),
                new Vector3(0, 0, 0),
                new Vector3(0, 0, 1),
                new Vector3(0, 0, 2)
            );

        static readonly ISpline spline3 = new CatmullRomSpline(
                new Vector3(-1, 0, -1),
                new Vector3(0, 0, 0),
                new Vector3(1, 0, 1),
                new Vector3(2, 0, 2)
            );


        [ContextMenu("Test")]
        void Test()
        {
            var meshFilter = GetComponent<MeshFilter>();
            Mesh mesh = meshFilter.sharedMesh;
            Vector3[] vertices = mesh.vertices;
            Transformation(spline3, ref vertices);
            mesh.vertices = vertices;
        }

        [ContextMenu("Test_Angle")]
        void Test_Angle()
        {
            var meshFilter = GetComponent<MeshFilter>();
            Mesh mesh = meshFilter.sharedMesh;
            Vector3[] vertices = mesh.vertices;
            dynamicWall.TransformSection(0, 0.195f, ref vertices);
            mesh.vertices = vertices;
        }

    }

}
