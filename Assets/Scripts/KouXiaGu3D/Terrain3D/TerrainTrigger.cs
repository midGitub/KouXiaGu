﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using KouXiaGu.Collections;

namespace KouXiaGu.Terrain3D
{

    /// <summary>
    /// 地表触发器;
    /// </summary>
    [RequireComponent(typeof(MeshCollider), typeof(TerrainChunk)), DisallowMultipleComponent]
    public class TerrainTrigger : MonoBehaviour
    {

        const string MESH_NAME = "Terrain Collision Mesh";

        //网格细分程度;
        const int SUB_X = 5;
        const int SUB_Z = 5;

        static readonly List<KeyValuePair<Vector3, UV>> VERTICES = GetVerticesAndUV();
        static readonly int[] TRIANGLES = GetTriangles();

        /// <summary>
        /// 获取到细分到的定点坐标 和 对应的UV坐标;
        /// </summary>
        static List<KeyValuePair<Vector3, UV>> GetVerticesAndUV()
        {
            List<KeyValuePair<Vector3, UV>> list = new List<KeyValuePair<Vector3, UV>>();

            float lengthX = TerrainChunk.CHUNK_WIDTH / SUB_X;
            float lengthZ = TerrainChunk.CHUNK_HEIGHT / SUB_Z;

            for (int z = 0; z <= SUB_Z; z++)
            {
                for (int x = 0; x <= SUB_X; x++)
                {
                    Vector3 vertice = new Vector3(x * lengthX, 0, z * lengthZ);
                    vertice.x -= TerrainChunk.CHUNK_WIDTH_HALF;
                    vertice.z -= TerrainChunk.CHUNK_HEIGHT_HALF;

                    UV uv = new UV(x / (float)SUB_X, z / (float)SUB_Z);

                    KeyValuePair<Vector3, UV> pair = new KeyValuePair<Vector3, UV>(vertice, uv);
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

        #region 地形射线(静态);

        const string LAYER_NAME = "Terrain";

        public static int RayLayerMask
        {
            get { return LayerMask.GetMask(LAYER_NAME); }
        }

        public static int RayLayer
        {
            get { return LayerMask.NameToLayer(LAYER_NAME); }
        }

        /// <summary>
        /// 射线最大距离;
        /// </summary>
        const float RAY_MAX_DISTANCE = 8000f;

        public static bool Raycast(Ray ray, out RaycastHit raycastHit, float maxDistance = RAY_MAX_DISTANCE)
        {
            return Physics.Raycast(ray, out raycastHit, maxDistance, RayLayerMask, QueryTriggerInteraction.Collide);
        }

        /// <summary>
        /// 获取到主摄像机的鼠标所指向的地形坐标,若无法获取到则返回默认值;
        /// </summary>
        public static Vector3 MouseRayPoint()
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit raycastHit;
            if (Raycast(ray, out raycastHit))
            {
               return raycastHit.point;
            }
            return default(Vector3);
        }

        #endregion


        void Start()
        {
            gameObject.layer = RayLayer;
            ResetCollisionMesh();
        }

        /// <summary>
        /// 重置碰撞网格;
        /// </summary>
        [ContextMenu("重置碰撞网格")]
        public void ResetCollisionMesh()
        {
            MeshCollider meshCollider = GetComponent<MeshCollider>();
            TerrainChunk terrainChunk = GetComponent<TerrainChunk>();

            Mesh mesh = meshCollider.sharedMesh;

            if (mesh == null || mesh.name != MESH_NAME)
            {
                mesh = new Mesh();
            }

            mesh.name = MESH_NAME;
            mesh.vertices = GetVertices(terrainChunk);
            mesh.triangles = TRIANGLES;

            meshCollider.sharedMesh = mesh;
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
                vertice.y = TerrainData.GetHeight(chunk, pair.Value);
                vertices.Add(vertice);
            }
            return vertices.ToArray();
        }

    }

}