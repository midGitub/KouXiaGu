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

        [SerializeField]
        string dynamicMeshName;
        MeshFilter meshFilter;
        DynamicMeshData meshData;

        public DynamicMeshManager manager
        {
            get { return DynamicMeshManager.Instance; }
        }

        public string DynamicMeshName
        {
            get { return dynamicMeshName; }
        }

        public DynamicMeshData MeshData
        {
            get { return meshData; }
        }

        void Awake()
        {
            meshFilter = GetComponent<MeshFilter>();
            meshData = manager.Find(dynamicMeshName);
        }

        /// <summary>
        /// 恢复为原本的顶点数据;
        /// </summary>
        [ContextMenu("复原网格")]
        public void RestoreVertices()
        {
            Vector3[] vertices = meshData.GetOriginalVertices();
            Mesh mesh = meshFilter.mesh;
            mesh.vertices = vertices;
        }

        /// <summary>
        /// 转换到曲线路径;
        /// </summary>
        public void Transformation(ISpline spline)
        {
            Mesh mesh = meshFilter.mesh;
            Vector3[] vertices = mesh.vertices;
            Transformation(spline1, ref vertices);
            mesh.vertices = vertices;
            mesh.RecalculateNormals();
        }

        /// <summary>
        /// 转换到曲线路径;
        /// </summary>
        void Transformation(ISpline spline, ref Vector3[] vertices)
        {
            meshData.Transformation(spline, ref vertices);
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
            Mesh mesh = meshFilter.mesh;
            Vector3[] vertices = mesh.vertices;
            meshData.TransformSection(0, 0.195f, ref vertices);
            mesh.vertices = vertices;
        }

        /// <summary>
        /// 构建节点记录;
        /// </summary>
        public void Build(float spacing)
        {
            Mesh mesh = GetComponent<MeshFilter>().sharedMesh;
            Vector3[] vertices = mesh.vertices;
            JointInfo jointInfo = new JointInfo(vertices, spacing);
            meshData = new DynamicMeshData(jointInfo, vertices);
            manager.AddOrUpdate(dynamicMeshName, meshData);
            Debug.Log("Build:" + DateTime.Now);
        }

#endregion
    }

}
