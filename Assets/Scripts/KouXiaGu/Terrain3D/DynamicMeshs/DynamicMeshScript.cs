using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using KouXiaGu.Collections;
using KouXiaGu.Grids;

namespace KouXiaGu.Terrain3D.DynamicMeshs
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

        public string dynamicMeshName = string.Empty;
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
            meshData = manager.Find(dynamicMeshName);
            return meshData;
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
            meshData.Transformation(spline, ref vertices);
            mesh.vertices = vertices;
            mesh.RecalculateNormals();
            mesh.RecalculateBounds();
            mesh.RecalculateTangents();
        }

        /// <summary>
        /// 转换到曲线路径,并且指定起点和终点的旋转角度;
        /// </summary>
        public void Transformation(ISpline spline, float start, float end)
        {
            Mesh mesh = meshFilter.mesh;
            Vector3[] vertices = mesh.vertices;
            meshData.Transformation(spline, start, end, ref vertices);
            mesh.vertices = vertices;
            mesh.RecalculateNormals();
            mesh.RecalculateBounds();
        }


        #region Test

        static readonly ISpline spline1 = new CatmullRomSpline(
                new CubicHexCoord(-1, 0, 1).GetTerrainPixel(),
                new CubicHexCoord(0, 0, 0).GetTerrainPixel(),
                new CubicHexCoord(1, -1, 0).GetTerrainPixel(),
                new CubicHexCoord(2, -2, 0).GetTerrainPixel()
            );

        static readonly ISpline spline2 = new CatmullRomSpline(
                new CubicHexCoord(0, -1, 1).GetTerrainPixel(),
                new CubicHexCoord(0, 0, 0).GetTerrainPixel(),
                new CubicHexCoord(0, 1, -1).GetTerrainPixel(),
                new CubicHexCoord(0, 2, -2).GetTerrainPixel()
            );

        static readonly ISpline spline3 = new CatmullRomSpline(
                new CubicHexCoord(-1, 0, 1).GetTerrainPixel(),
                new CubicHexCoord(0, 0, 0).GetTerrainPixel(),
                new CubicHexCoord(1, 0, -1).GetTerrainPixel(),
                new CubicHexCoord(2, 0, -2).GetTerrainPixel()
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

        public void Save()
        {
            manager.AddOrUpdate(dynamicMeshName, meshData);
        }

        /// <summary>
        /// 获取 from 到 to 的角度(忽略Y),单位弧度;原型:Mathf.Atan2();
        /// </summary>
        float AngleY(Vector3 from, Vector3 to)
        {
            return Mathf.Atan2((to.x - from.x), (to.z - from.z));
        }

        [ContextMenu("123")]
        void Test123()
        {
            Debug.Log((int)(0.75 / (1 / (double)3)));
        }

        #endregion
    }

}
