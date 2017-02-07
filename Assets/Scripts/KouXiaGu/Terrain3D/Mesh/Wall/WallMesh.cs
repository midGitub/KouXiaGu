using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace KouXiaGu.Terrain3D
{

    /// <summary>
    /// 根据地形起伏的城墙;
    /// </summary>
    [DisallowMultipleComponent, RequireComponent(typeof(MeshFilter))]
    public sealed class WallMesh : MonoBehaviour
    {

        MeshFilter meshFilter;

        void Awake()
        {
            meshFilter = GetComponent<MeshFilter>();
        }


        [ContextMenu("Test")]
        void Test()
        {
            meshFilter = GetComponent<MeshFilter>();
            Vector3[] vertices = meshFilter.sharedMesh.vertices.ToArray();


            for (int i = 0; i < vertices.Length; i++)
            {
                Vector3 vertice = vertices[i];
                if (vertice.y == 0)
                {
                    vertice.y = 0;
                    vertices[i] = vertice;
                }
                else
                {
                    vertice.y = 1;
                    vertices[i] = vertice;
                }
            }

            Mesh mesh = new Mesh();
            mesh.vertices = vertices;
            mesh.triangles = meshFilter.sharedMesh.triangles;
            mesh.uv = meshFilter.sharedMesh.uv;
            mesh.RecalculateNormals();
            mesh.name = "test";
            meshFilter.mesh = mesh;
        }


    }

}
