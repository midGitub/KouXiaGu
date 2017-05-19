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
        string dynamicMeshName = string.Empty;
        MeshFilter meshFilter;
        DynamicMeshData meshData;

        public DynamicMeshManager manager
        {
            get { return DynamicMeshManager.Instance; }
        }

        public string DynamicMeshName
        {
            get { return dynamicMeshName; }
            set { dynamicMeshName = value; }
        }

        public DynamicMeshData MeshData
        {
            get { return meshData; }
        }

        void Awake()
        {
            meshFilter = GetComponent<MeshFilter>();
            ReadPredefinedDynamicMesh();
        }

        /// <summary>
        /// 读取并设置预定义的网格信息;
        /// </summary>
        [ContextMenu("读取预定义的网格")]
        public DynamicMeshData ReadPredefinedDynamicMesh()
        {
            return meshData = manager.Find(dynamicMeshName);
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
            Transformation(spline, ref vertices);
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

        [ContextMenu("Test1")]
        void Test1()
        {
            Transformation(spline1);
        }

        [ContextMenu("Test2")]
        void Test2()
        {
            Transformation(spline2);
        }

        [ContextMenu("Test3")]
        void Test3()
        {
            Transformation(spline3);
        }

        /// <summary>
        /// 构建节点记录,并且持久保存,若DynamicMeshManager为预制物体,需要重新保存预制物体;
        /// </summary>
        public void AutoBuild(float spacing)
        {
            Mesh mesh = GetComponent<MeshFilter>().sharedMesh;
            Vector3[] vertices = mesh.vertices;
            JointInfo jointInfo = new JointInfo(vertices, spacing);
            meshData = new DynamicMeshData(jointInfo, vertices);
        }

        [ContextMenu("保存到...")]
        void Save()
        {
            manager.AddOrUpdate(dynamicMeshName, meshData);
        }
#endregion
    }

}
