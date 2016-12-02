using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace KouXiaGu.Terrain
{

    /// <summary>
    /// 平顶的六边形;
    /// </summary>
    [RequireComponent(typeof(MeshFilter), typeof(MeshRenderer)), ExecuteInEditMode]
    public class FlatHexMesh : MonoBehaviour
    {

        /// <summary>
        /// 六边形外半径;
        /// </summary>
        const float outerRadius = 1f;
        static readonly float innerRadius = (float)(Math.Sqrt(3) / 2 * outerRadius);
        const float halfOuterRadius = outerRadius / 2;
        const float height = 0;
        const string hexMeshName = "Hex Mesh";

        /// <summary>
        /// 定点定义;
        /// </summary>
        static readonly Vector3[] vertices = new Vector3[]
            {
                new Vector3(-halfOuterRadius , height, innerRadius),
                new Vector3(halfOuterRadius, height, innerRadius),
                new Vector3(outerRadius, height, 0f),
                new Vector3(halfOuterRadius, height, -innerRadius),
                new Vector3(-halfOuterRadius, height, -innerRadius),
                new Vector3(-outerRadius, height, 0f)
            };


        static readonly int[] triangles = new int[]
            {
                0,1,2,
                0,2,5,
                5,2,3,
                3,4,5
            };

        static readonly Vector2[] uv = new Vector2[]
            {
                new Vector2(0.25f, 1),
                new Vector2(0.75f, 1),
                new Vector2(1, 0.5f),
                new Vector2(0.75f, 0),
                new Vector2(0.25f, 0),
                new Vector2(0, 0.5f),
            };

        Mesh hexMesh;
       
        void Awake()
        {
            GetComponent<MeshFilter>().mesh = hexMesh = new Mesh();
            hexMesh.name = hexMeshName;

            //if (meshFilter.mesh != null && meshFilter.mesh.name != hexMeshName)
            //{
            //    hexMesh = new Mesh();
            //    hexMesh.name = hexMeshName;
            //    meshFilter.mesh = hexMesh;
            //}
        }

        void Start()
        {
            hexMesh.vertices = vertices;
            hexMesh.triangles = triangles;
            hexMesh.uv = uv;
            hexMesh.RecalculateNormals();
        }

    }
}
