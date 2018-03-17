using System;
using System.Collections.Generic;
using UnityEngine;

namespace JiongXiaGu.Unity.RectTerrain
{
    /// <summary>
    /// 地形碰撞器;
    /// </summary>
    public class LandformCollider
    {
        public const string MeshName = "Landform Collision Mesh";

        //网格细分程度;
        private const int sub_x = 4;
        private const int sub_z = 4;

        private static readonly IReadOnlyList<KeyValuePair<Vector3, Vector2>> vertices = GetVerticesAndUV();
        private static readonly int[] triangles = GetTriangles();

        /// <summary>
        /// 获取到细分到的顶点坐标 和 对应的UV坐标;
        /// </summary>
        private static List<KeyValuePair<Vector3, Vector2>> GetVerticesAndUV()
        {
            List<KeyValuePair<Vector3, Vector2>> list = new List<KeyValuePair<Vector3, Vector2>>();

            float lengthX = LandformChunkInfo.ChunkWidth / sub_x;
            float lengthZ = LandformChunkInfo.ChunkHeight / sub_z;

            for (int z = 0; z <= sub_z; z++)
            {
                for (int x = 0; x <= sub_x; x++)
                {
                    Vector3 vertice = new Vector3(x * lengthX, 0, z * lengthZ);
                    vertice.x -= LandformChunkInfo.ChunkHalfWidth;
                    vertice.z -= LandformChunkInfo.ChunkHalfHeight;

                    Vector2 uv = new Vector2(x / (float)sub_x, z / (float)sub_z);
                    KeyValuePair<Vector3, Vector2> pair = new KeyValuePair<Vector3, Vector2>(vertice, uv);
                    list.Add(pair);
                }
            }

            return list;
        }

        private static int[] GetTriangles()
        {
            List<int> triangles = new List<int>();
            int SUB_X1 = sub_x + 1;
            int SUB_X2 = sub_x + 2;

            for (int y = 0; y < sub_z; y++)
            {
                for (int x = 0; x < sub_x; x++)
                {
                    int pos = y * SUB_X1 + x;

                    triangles.Add(pos);
                    triangles.Add(pos + SUB_X1);
                    triangles.Add(pos + 1);

                    triangles.Add(pos + 1);
                    triangles.Add(pos + SUB_X1);
                    triangles.Add(pos + SUB_X2);
                }
            }

            return triangles.ToArray();
        }

        private static Mesh CreateMesh(LandformRenderer renderer)
        {
            var collisionMesh = new Mesh();
            collisionMesh.name = MeshName;
            UpdateMesh(collisionMesh, renderer);
            return collisionMesh;
        }

        private static void UpdateMesh(Mesh collisionMesh, LandformRenderer renderer)
        {
            collisionMesh.Clear();
            collisionMesh.vertices = GetVertices(renderer);
            collisionMesh.triangles = triangles;
        }

        /// <summary>
        /// 获取到高度对应的顶点坐标;
        /// </summary>
        private static Vector3[] GetVertices(LandformRenderer renderer)
        {
            List<Vector3> vertices = new List<Vector3>();
            foreach (var pair in LandformCollider.vertices)
            {
                Vector3 vertice = pair.Key;
                vertice.y = renderer.GetHeight(pair.Value);
                vertices.Add(vertice);
            }
            return vertices.ToArray();
        }


        private MeshCollider meshCollider;
        private LandformRenderer renderer;
        private Mesh collisionMesh;

        public LandformCollider(MeshCollider meshCollider, LandformRenderer renderer)
        {
            if (meshCollider == null)
                throw new ArgumentNullException(nameof(meshCollider));
            if (renderer == null)
                throw new ArgumentNullException(nameof(renderer));

            this.meshCollider = meshCollider;
            this.renderer = renderer;
            meshCollider.sharedMesh = collisionMesh = new Mesh();
        }

        /// <summary>
        /// 更新网格;
        /// </summary>
        public void Update()
        {
            if (collisionMesh != null)
            {
                collisionMesh = meshCollider.sharedMesh = CreateMesh(renderer);
            }
            else
            {
                Mesh mesh = new Mesh();
                UpdateMesh(mesh, renderer);
                collisionMesh = meshCollider.sharedMesh = mesh;
            }
        }
    }
}
