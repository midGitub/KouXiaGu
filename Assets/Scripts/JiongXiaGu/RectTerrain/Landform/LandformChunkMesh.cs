using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace JiongXiaGu.RectTerrain
{

    [ExecuteInEditMode]
    [DisallowMultipleComponent]
    [RequireComponent(typeof(MeshFilter))]
    public sealed class LandformChunkMesh : MonoBehaviour
    {
        LandformChunkMesh()
        {
        }

        const string meshName = "LandformChunkMesh";

        static readonly float chunkHalfWidth = LandformInfo.ChunkHalfWidth + 0.001f;
        static readonly float chunkHalfHeight = LandformInfo.ChunkHalfHeight + 0.001f;

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
        static readonly Vector2[] uvs = new Vector2[]
           {
                new Vector2(0f, 1f),
                new Vector2(1f, 1f),
                new Vector2(1f, 0f),
                new Vector2(0f, 0f),
           };

        MeshFilter meshFilter;
        static Mesh landformMesh;

        void Awake()
        {
            meshFilter = GetComponent<MeshFilter>();
            meshFilter.sharedMesh = GetPublicMesh();
        }

        void Reset()
        {
            Mesh mesh = landformMesh;
            if (mesh == null)
            {
                mesh = landformMesh = GetPublicMesh();
            }
            else
            {
                UpdateMesh(ref mesh);
            }
            meshFilter = GetComponent<MeshFilter>();
            meshFilter.sharedMesh = mesh;
        }

        /// <summary>
        /// 创建一个新的地形块网格结构;
        /// </summary>
        static Mesh CreateMesh()
        {
            Mesh mesh = new Mesh();
            UpdateMesh(ref mesh);
            return mesh;
        }

        static void UpdateMesh(ref Mesh mesh)
        {
            mesh.Clear();
            mesh.name = meshName;
            mesh.vertices = vertices;
            mesh.triangles = triangles;
            mesh.uv = uvs;
            mesh.RecalculateNormals();
        }

        static Mesh GetPublicMesh()
        {
            if (landformMesh == null)
            {
                landformMesh = CreateMesh();
            }
            return landformMesh;
        }
    }
}
