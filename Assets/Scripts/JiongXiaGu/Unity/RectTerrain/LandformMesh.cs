using System;
using UnityEngine;

namespace JiongXiaGu.Unity.RectTerrain
{
    public class LandformMesh
    {
        public const string MeshName = "Landform Mesh";
        private static readonly float chunkHalfWidth = ChunkInfo.Height + 0.001f;
        private static readonly float chunkHalfHeight = ChunkInfo.Width + 0.001f;

        /// <summary>
        /// 网格生成的高度;
        /// </summary>
        private const float altitude = 0;

        /// <summary>
        /// 网格顶点数据;
        /// </summary>
        private static readonly Vector3[] vertices = new Vector3[]
            {
                new Vector3(-chunkHalfWidth , altitude, chunkHalfHeight),
                new Vector3(chunkHalfWidth, altitude, chunkHalfHeight),
                new Vector3(chunkHalfWidth, altitude, -chunkHalfHeight),
                new Vector3(-chunkHalfWidth, altitude, -chunkHalfHeight),
            };

        /// <summary>
        /// 网格三角形数据;
        /// </summary>
        private static readonly int[] triangles = new int[]
           {
                0,1,2,
                0,2,3,
           };

        /// <summary>
        /// 网格UV坐标数据;
        /// </summary>
        private static readonly Vector2[] uvs = new Vector2[]
           {
                new Vector2(0f, 1f),
                new Vector2(1f, 1f),
                new Vector2(1f, 0f),
                new Vector2(0f, 0f),
           };

        private static Mesh publicMesh;

        /// <summary>
        /// 创建一个新的地形块网格结构;
        /// </summary>
        public static Mesh CreateMesh()
        {
            Mesh mesh = new Mesh();
            UpdateMesh(ref mesh);
            return mesh;
        }

        private static void UpdateMesh(ref Mesh mesh)
        {
            mesh.Clear();
            mesh.name = MeshName;
            mesh.vertices = vertices;
            mesh.triangles = triangles;
            mesh.uv = uvs;
            mesh.RecalculateNormals();
        }

        public static Mesh GetPublicMesh()
        {
            if (publicMesh == null)
            {
                publicMesh = CreateMesh();
            }
            return publicMesh;
        }


        private MeshFilter meshFilter;

        public LandformMesh(MeshFilter meshFilter)
        {
            if (meshFilter == null)
                throw new ArgumentNullException(nameof(meshFilter));

            this.meshFilter = meshFilter;
            meshFilter.sharedMesh = GetPublicMesh();
        }

        public void Reset()
        {
            meshFilter.sharedMesh = GetPublicMesh();
        }
    }


}
