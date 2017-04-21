using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UniRx;

namespace KouXiaGu.Terrain3D
{


    public class LandformTrigger : IObserver<LandformRenderer>
    {

        #region 网格定义;

        const string MeshName = "Terrain Collision Mesh";

        //网格细分程度;
        const int sub_x = 5;
        const int sub_z = 5;

        static readonly List<KeyValuePair<Vector3, Vector2>> vertices = GetVerticesAndUV();
        static readonly int[] triangles = GetTriangles();

        /// <summary>
        /// 获取到细分到的顶点坐标 和 对应的UV坐标;
        /// </summary>
        static List<KeyValuePair<Vector3, Vector2>> GetVerticesAndUV()
        {
            List<KeyValuePair<Vector3, Vector2>> list = new List<KeyValuePair<Vector3, Vector2>>();

            float lengthX = ChunkInfo.ChunkWidth / sub_x;
            float lengthZ = ChunkInfo.ChunkHeight / sub_z;

            for (int z = 0; z <= sub_z; z++)
            {
                for (int x = 0; x <= sub_x; x++)
                {
                    Vector3 vertice = new Vector3(x * lengthX, 0, z * lengthZ);
                    vertice.x -= ChunkInfo.ChunkHalfWidth;
                    vertice.z -= ChunkInfo.ChunkHalfHeight;

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

        #endregion


        public LandformTrigger(MeshCollider collider, LandformRenderer renderer, IObservable<LandformRenderer> onHeightMapUpdate)
        {
            this.collider = collider;
            this.renderer = renderer;
            BuildCollisionMesh();
            onHeightMapUpdate.Subscribe(this);
        }

        MeshCollider collider;
        LandformRenderer renderer;

        /// <summary>
        /// 构建碰撞网格;
        /// </summary>
        void BuildCollisionMesh()
        {
            Mesh mesh = new Mesh();
            mesh.name = MeshName;
            collider.sharedMesh = BuildCollisionMesh(mesh);
        }

        /// <summary>
        /// 重新构建碰撞网格;
        /// </summary>
        public void RebuildCollisionMesh()
        {
            Mesh mesh = collider.sharedMesh;
            collider.sharedMesh = BuildCollisionMesh(mesh);
        }

        /// <summary>
        /// 构建碰撞网格;
        /// </summary>
        Mesh BuildCollisionMesh(Mesh mesh)
        {
            mesh.vertices = GetVertices();
            mesh.triangles = triangles;
            return mesh;
        }

        /// <summary>
        /// 获取到高度对应的顶点坐标;
        /// </summary>
        Vector3[] GetVertices()
        {
            List<Vector3> vertices = new List<Vector3>();
            foreach (var pair in LandformTrigger.vertices)
            {
                Vector3 vertice = pair.Key;
                vertice.y = renderer.GetHeight(pair.Value);
                vertices.Add(vertice);
            }
            return vertices.ToArray();
        }

        void IObserver<LandformRenderer>.OnNext(LandformRenderer value)
        {
            RebuildCollisionMesh();
        }

        void IObserver<LandformRenderer>.OnError(Exception error) { }
        void IObserver<LandformRenderer>.OnCompleted() { }

    }

}
