using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace KouXiaGu.Terrain3D
{

    /// <summary>
    /// 地表触发器;
    /// </summary>
    [RequireComponent(typeof(MeshCollider), typeof(TerrainChunk)), DisallowMultipleComponent]
    public class TerrainTrigger : MonoBehaviour
    {

        const string MESH_NAME = "TerrainCollisionMesh";

        //网格细分程度;
        const int SUB_X = 8;
        const int SUB_Y = 8;

        static readonly List<KeyValuePair<Vector3, UV>> VERTICES = GetVerticesAndUV();
        static readonly int[] TRIANGLES = GetTriangles();

        /// <summary>
        /// 获取到细分到的定点坐标 和 对应的UV坐标;
        /// </summary>
        static List<KeyValuePair<Vector3, UV>> GetVerticesAndUV()
        {
            List<KeyValuePair<Vector3, UV>> list = new List<KeyValuePair<Vector3, UV>>();

            float lengthX = TerrainChunk.CHUNK_WIDTH / SUB_X;
            float lengthY = TerrainChunk.CHUNK_HEIGHT / SUB_Y;

            for (int y = 0; y <= SUB_Y; y++)
            {
                for (int x = 0; x <= SUB_X; x++)
                {
                    Vector3 vertice = new Vector3(x * lengthX, 0, y * lengthY);
                    vertice.x -= TerrainChunk.CHUNK_WIDTH_HALF;
                    vertice.z -= TerrainChunk.CHUNK_HEIGHT_HALF;

                    UV uv = new UV(x / (float)SUB_X, y / (float)SUB_Y);

                    KeyValuePair<Vector3, UV> pair = new KeyValuePair<Vector3, UV>(vertice, uv);
                    list.Add(pair);
                }
            }

            return list;
        }

        static int[] GetTriangles()
        {
            List<int> triangles = new List<int>();

            for (int y = 0; y < SUB_Y; y++)
            {
                for (int x = 0; x < SUB_X; x++)
                {
                    int pos = y * (SUB_X + 1) + x;

                    triangles.Add(pos);
                    triangles.Add(pos + 1);
                    triangles.Add(pos + 1 + (SUB_X + 1));

                    triangles.Add(pos);
                    triangles.Add(pos + (SUB_X + 1));
                    triangles.Add(pos + 1 + (SUB_X + 1));
                }
            }

            return triangles.ToArray();
        }

        TerrainChunk terrainChunk;
        MeshCollider meshCollider;

        void Awake()
        {
            terrainChunk = GetComponent<TerrainChunk>();
            meshCollider = GetComponent<MeshCollider>();
        }

        void Start()
        {
            ResetCollisionMesh();
        }

        /// <summary>
        /// 获取到高度对应的定点坐标;
        /// </summary>
        Vector3[] GetVertices(TerrainChunk chunk)
        {
            List<Vector3> vertices = new List<Vector3>();
            foreach (var pair in VERTICES)
            {
                Vector3 vertice = pair.Key;
                vertice.y = GetHeight(pair.Value);
                vertices.Add(vertice);
            }
            return vertices.ToArray();
        }

        float GetHeight(UV uv)
        {
            return TerrainData.GetHeight(terrainChunk, uv);
        }

        /// <summary>
        /// 重置碰撞网格;
        /// </summary>
        [ContextMenu("重置碰撞网格")]
        public void ResetCollisionMesh()
        {
            Mesh mesh;

            if (meshCollider.sharedMesh.name == MESH_NAME)
            {
                mesh = meshCollider.sharedMesh;
            }
            else
            {
                mesh = new Mesh();
            }

            mesh.name = MESH_NAME;
            mesh.vertices = GetVertices(terrainChunk);
            mesh.triangles = TRIANGLES;

            meshCollider.sharedMesh = mesh;
        }

    }

}
