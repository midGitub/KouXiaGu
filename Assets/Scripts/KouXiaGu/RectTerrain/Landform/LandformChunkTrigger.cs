using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace KouXiaGu.RectTerrain
{

    [ExecuteInEditMode]
    [DisallowMultipleComponent]
    [RequireComponent(typeof(MeshCollider), typeof(LandformChunkRenderer))]
    public class LandformChunkTrigger : MonoBehaviour
    {
        LandformChunkTrigger()
        {
        }

        const string MeshName = "Terrain Collision Mesh";

        //网格细分程度;
        const int sub_x = 4;
        const int sub_z = 4;

        static readonly List<KeyValuePair<Vector3, Vector2>> vertices = GetVerticesAndUV();
        static readonly int[] triangles = GetTriangles();

        MeshCollider meshCollider;
        LandformChunkRenderer landformRenderer;
        Mesh collisionMesh;

        void Awake()
        {
            meshCollider = GetComponent<MeshCollider>();
            landformRenderer = GetComponent<LandformChunkRenderer>();
            collisionMesh = meshCollider.sharedMesh = CreateMesh();
            landformRenderer.OnHeightChanged += UpdateMesh;
        }

        void Reset()
        {
            UpdateMesh(null);
        }

        Mesh CreateMesh()
        {
            var collisionMesh = new Mesh();
            collisionMesh.name = MeshName;
            UpdateMesh(ref collisionMesh);
            return collisionMesh;
        }

        void UpdateMesh(LandformChunkRenderer renderer)
        {
            if (collisionMesh == null)
            {
                collisionMesh = meshCollider.sharedMesh = CreateMesh();
            }
            else
            {
                UpdateMesh(ref collisionMesh);
                meshCollider.sharedMesh = collisionMesh;
            }
        }

        void UpdateMesh(ref Mesh collisionMesh)
        {
            collisionMesh.Clear();
            collisionMesh.vertices = GetVertices();
            collisionMesh.triangles = triangles;
        }

        /// <summary>
        /// 获取到高度对应的顶点坐标;
        /// </summary>
        Vector3[] GetVertices()
        {
            List<Vector3> vertices = new List<Vector3>();
            foreach (var pair in LandformChunkTrigger.vertices)
            {
                Vector3 vertice = pair.Key;
                vertice.y = landformRenderer.GetHeight(pair.Value);
                vertices.Add(vertice);
            }
            return vertices.ToArray();
        }


        /// <summary>
        /// 获取到细分到的顶点坐标 和 对应的UV坐标;
        /// </summary>
        static List<KeyValuePair<Vector3, Vector2>> GetVerticesAndUV()
        {
            List<KeyValuePair<Vector3, Vector2>> list = new List<KeyValuePair<Vector3, Vector2>>();

            float lengthX = LandformInfo.ChunkWidth / sub_x;
            float lengthZ = LandformInfo.ChunkHeight / sub_z;

            for (int z = 0; z <= sub_z; z++)
            {
                for (int x = 0; x <= sub_x; x++)
                {
                    Vector3 vertice = new Vector3(x * lengthX, 0, z * lengthZ);
                    vertice.x -= LandformInfo.ChunkHalfWidth;
                    vertice.z -= LandformInfo.ChunkHalfHeight;

                    Vector2 uv = new Vector2(x / (float)sub_x, z / (float)sub_z);
                    KeyValuePair<Vector3, Vector2> pair = new KeyValuePair<Vector3, Vector2>(vertice, uv);
                    list.Add(pair);
                }
            }

            return list;
        }

        static int[] GetTriangles()
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
    }
}
