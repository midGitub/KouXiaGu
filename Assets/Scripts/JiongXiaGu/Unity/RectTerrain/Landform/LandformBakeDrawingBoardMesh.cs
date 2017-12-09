using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace JiongXiaGu.Unity.RectTerrain
{

    [ExecuteInEditMode]
    [DisallowMultipleComponent]
    [RequireComponent(typeof(MeshFilter))]
    public sealed class LandformBakeDrawingBoardMesh : MonoBehaviour
    {
        private LandformBakeDrawingBoardMesh()
        {
        }

        public const string MeshName = "LandformBakeDrawingBoardMesh";
        private static readonly float chunkHalfWidth = RectTerrainInfo.NodeWidth / 2 + (RectTerrainInfo.NodeWidth / 3);
        private static readonly float chunkHalfHeight = RectTerrainInfo.NodeHeight / 2 + (RectTerrainInfo.NodeHeight / 3);
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

        private MeshFilter meshFilter;
        private static Mesh landformMesh;

        private void Awake()
        {
            meshFilter = GetComponent<MeshFilter>();
            meshFilter.sharedMesh = GetPublicMesh();
        }

        private void Reset()
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
        private static Mesh CreateMesh()
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

        private static Mesh GetPublicMesh()
        {
            if (landformMesh == null)
            {
                landformMesh = CreateMesh();
            }
            return landformMesh;
        }
    }
}
