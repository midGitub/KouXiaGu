using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace KouXiaGu.Terrain3D
{

    /// <summary>
    /// 地形网格;
    /// </summary>
    [SerializeField]
    public class TerrainMesh
    {

        #region Static

        const string MESH_NAME = "Terrain Mesh";

        static readonly float MESH_HALF_WIDTH = LandformChunk.CHUNK_WIDTH_HALF;
        static readonly float MESH_HALF_HEIGHT = LandformChunk.CHUNK_HEIGHT_HALF;

        /// <summary>
        /// 网格生成的高度;
        /// </summary>
        const float ALTITUDE = 0;

        /// <summary>
        /// 网格顶点数据;
        /// </summary>
        static readonly Vector3[] VERTICES = new Vector3[]
            {
                new Vector3(-MESH_HALF_WIDTH , ALTITUDE, MESH_HALF_HEIGHT),
                new Vector3(MESH_HALF_WIDTH, ALTITUDE, MESH_HALF_HEIGHT),
                new Vector3(MESH_HALF_WIDTH, ALTITUDE, -MESH_HALF_HEIGHT),
                new Vector3(-MESH_HALF_WIDTH, ALTITUDE, -MESH_HALF_HEIGHT),
            };

        /// <summary>
        /// 网格三角形数据;
        /// </summary>
        static readonly int[] TRIANGLES = new int[]
           {
                0,1,2,
                0,2,3,
           };

        /// <summary>
        /// 网格UV坐标数据;
        /// </summary>
        static readonly Vector2[] UV = new Vector2[]
           {
                new Vector2(0f, 1f),
                new Vector2(1f, 1f),
                new Vector2(1f, 0f),
                new Vector2(0f, 0f),
           };

        /// <summary>
        /// 创建一个新的地形块网格结构;
        /// </summary>
        internal static Mesh CreateMesh()
        {
            Mesh mesh = new Mesh();

            mesh.name = MESH_NAME;
            mesh.vertices = VERTICES;
            mesh.triangles = TRIANGLES;
            mesh.uv = UV;
            mesh.RecalculateNormals();

            return mesh;
        }

        static Mesh publicMesh;

        /// <summary>
        /// 获取到公共使用的地形块网格结构;
        /// </summary>
        static Mesh PublicMesh
        {
            get { return publicMesh ?? (publicMesh = CreateMesh()); }
            set { publicMesh = value; }
        }

        #endregion


        public TerrainMesh(MeshFilter meshFilter)
        {
            MeshFilter = meshFilter;
            MeshFilter.sharedMesh = PublicMesh;
        }

        public MeshFilter MeshFilter { get; private set; }

        public void Reset()
        {
            PublicMesh = CreateMesh();
            MeshFilter.sharedMesh = PublicMesh;
        }

    }

}
