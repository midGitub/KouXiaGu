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
    public class LandformMesh
    {

        #region Static

        const string meshName = "LandformChunkMesh";

        static readonly float chunkHalfWidth = LandformChunkInfo.ChunkHalfWidth;
        static readonly float chunkHalfHeight = LandformChunkInfo.ChunkHalfHeight;

        /// <summary>
        /// 网格生成的高度;
        /// </summary>
        const float altitude = 0;

        /// <summary>
        /// 网格顶点数据;
        /// </summary>
        static readonly Vector3[] vertices = new Vector3[]
            {
                new Vector3(-chunkHalfWidth , altitude, chunkHalfHeight),
                new Vector3(chunkHalfWidth, altitude, chunkHalfHeight),
                new Vector3(chunkHalfWidth, altitude, -chunkHalfHeight),
                new Vector3(-chunkHalfWidth, altitude, -chunkHalfHeight),
            };

        /// <summary>
        /// 网格三角形数据;
        /// </summary>
        static readonly int[] triangles = new int[]
           {
                0,1,2,
                0,2,3,
           };

        /// <summary>
        /// 网格UV坐标数据;
        /// </summary>
        static readonly Vector2[] UV_Coordinates = new Vector2[]
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

            mesh.name = meshName;
            mesh.vertices = vertices;
            mesh.triangles = triangles;
            mesh.uv = UV_Coordinates;
            mesh.RecalculateNormals();

            return mesh;
        }

        static Mesh publicMesh;

        /// <summary>
        /// 获取到公共使用的地形块网格结构;
        /// </summary>
        internal static Mesh PublicMesh
        {
            get { return publicMesh ?? (publicMesh = CreateMesh()); }
            set { publicMesh = value; }
        }

        #endregion

        public LandformMesh(MeshFilter meshFilter)
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
