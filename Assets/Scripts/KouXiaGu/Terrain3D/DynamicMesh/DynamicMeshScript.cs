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
    public sealed class DynamicMeshScript : MonoBehaviour
    {
        DynamicMeshScript()
        {
        }

        MeshFilter _meshFilter;
        DynamicMeshData wallInfo;

        public DynamicMeshData WallInfo
        {
            get { return wallInfo; }
        }

        MeshFilter meshFilter
        {
            get { return _meshFilter == null ? (_meshFilter = GetComponent<MeshFilter>()) : _meshFilter; }
        }

        Mesh currentMesh
        {
            get { return Application.isPlaying ? meshFilter.mesh : meshFilter.sharedMesh; }
        }

        /// <summary>
        /// 恢复为原本的顶点数据;
        /// </summary>
        [ContextMenu("复原网格")]
        public void RestoreVertices()
        {
            Vector3[] vertices = wallInfo.GetOriginalVertices();
            currentMesh.vertices = vertices;
        }

        /// <summary>
        /// 转换到曲线路径;
        /// </summary>
        public void Transformation(ISpline spline)
        {
            Vector3[] vertices = currentMesh.vertices;
            Transformation(spline1, ref vertices);
            currentMesh.vertices = vertices;
        }

        /// <summary>
        /// 转换到曲线路径;
        /// </summary>
        void Transformation(ISpline spline, ref Vector3[] vertices)
        {
            wallInfo.Transformation(spline, ref vertices);
        }

#region Test

        static readonly ISpline spline1 = new CatmullRomSpline(
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
            Transformation(spline1);
        }

        [ContextMenu("Test_Angle")]
        void Test_Angle()
        {
            var meshFilter = GetComponent<MeshFilter>();
            Mesh mesh = meshFilter.sharedMesh;
            Vector3[] vertices = mesh.vertices;
            wallInfo.TransformSection(0, 0.195f, ref vertices);
            mesh.vertices = vertices;
        }

        /// <summary>
        /// 构建节点记录;
        /// </summary>
        public void Build(float spacing)
        {
            Vector3[] vertices = currentMesh.vertices;
            JointInfo jointInfo = new JointInfo(vertices, spacing);
            wallInfo = new DynamicMeshData(jointInfo, vertices);
        }

#endregion

    }

}
