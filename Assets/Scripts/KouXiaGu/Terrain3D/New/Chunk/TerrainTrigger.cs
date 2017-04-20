using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace KouXiaGu.Terrain3D
{


    public class TerrainTrigger
    {

        #region 网格定义;

        const string MESH_NAME = "Terrain Collision Mesh";

        //网格细分程度;
        const int SUB_X = 5;
        const int SUB_Z = 5;

        static readonly List<KeyValuePair<Vector3, Vector2>> VERTICES = GetVerticesAndUV();
        static readonly int[] TRIANGLES = GetTriangles();

        /// <summary>
        /// 获取到细分到的定点坐标 和 对应的UV坐标;
        /// </summary>
        static List<KeyValuePair<Vector3, Vector2>> GetVerticesAndUV()
        {
            List<KeyValuePair<Vector3, Vector2>> list = new List<KeyValuePair<Vector3, Vector2>>();

            float lengthX = LandformChunk.CHUNK_WIDTH / SUB_X;
            float lengthZ = LandformChunk.CHUNK_HEIGHT / SUB_Z;

            for (int z = 0; z <= SUB_Z; z++)
            {
                for (int x = 0; x <= SUB_X; x++)
                {
                    Vector3 vertice = new Vector3(x * lengthX, 0, z * lengthZ);
                    vertice.x -= LandformChunk.CHUNK_WIDTH_HALF;
                    vertice.z -= LandformChunk.CHUNK_HEIGHT_HALF;

                    Vector2 uv = new Vector2(x / (float)SUB_X, z / (float)SUB_Z);

                    KeyValuePair<Vector3, Vector2> pair = new KeyValuePair<Vector3, Vector2>(vertice, uv);
                    list.Add(pair);
                }
            }

            return list;
        }

        static int[] GetTriangles()
        {
            List<int> triangles = new List<int>();
            int SUB_X1 = SUB_X + 1;
            int SUB_X2 = SUB_X + 2;

            for (int y = 0; y < SUB_Z; y++)
            {
                for (int x = 0; x < SUB_X; x++)
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

        #endregion


        public TerrainTrigger(MeshCollider collider)
        {

        }

        //[ContextMenu("重置碰撞网格")]
        //public void ResetCollisionMesh()
        //{
        //    LandformRenderer terrainChunk = GetComponent<LandformRenderer>();
        //    ResetCollisionMesh(terrainChunk);
        //}

        //public void ResetCollisionMesh(LandformRenderer terrainChunk)
        //{
        //    MeshCollider meshCollider = GetComponent<MeshCollider>();
        //    Mesh mesh = meshCollider.sharedMesh;

        //    if (mesh == null || mesh.name != MESH_NAME)
        //    {
        //        mesh = new Mesh();
        //    }

        //    mesh.name = MESH_NAME;
        //    mesh.vertices = GetVertices(terrainChunk);
        //    mesh.triangles = TRIANGLES;

        //    meshCollider.sharedMesh = mesh;
        //}

        ///// <summary>
        ///// 获取到高度对应的顶点坐标;
        ///// </summary>
        //Vector3[] GetVertices(LandformRenderer chunk)
        //{
        //    List<Vector3> vertices = new List<Vector3>();
        //    foreach (var pair in VERTICES)
        //    {
        //        Vector3 vertice = pair.Key;
        //        vertice.y = TerrainData.GetHeight(chunk, pair.Value);
        //        vertices.Add(vertice);
        //    }
        //    return vertices.ToArray();
        //}

    }

}
