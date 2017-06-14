using System;
using UnityEngine;

namespace KouXiaGu.Terrain3D
{

    /// <summary>
    /// 平顶的六边形网格结构;
    /// </summary>
    [RequireComponent(typeof(MeshFilter), typeof(MeshRenderer)), ExecuteInEditMode, DisallowMultipleComponent]
    public sealed class FlatHexMesh : MonoBehaviour
    {
        FlatHexMesh() { }

        const string meshName = "Hex Mesh";

        /// <summary>
        /// 六边形外半径;
        /// </summary>
        const float outerRadius = LandformConvert.OuterRadius;
        static readonly float innerRadius = (float)(Math.Sqrt(3) / 2 * outerRadius);
        const float halfOuterRadius = outerRadius / 2;
        const float altitude = 0;

        static readonly Vector3[] vertices = new Vector3[]
            {
                new Vector3(-halfOuterRadius , altitude, innerRadius),
                new Vector3(halfOuterRadius, altitude, innerRadius),
                new Vector3(outerRadius, altitude, 0f),
                new Vector3(halfOuterRadius, altitude, -innerRadius),
                new Vector3(-halfOuterRadius, altitude, -innerRadius),
                new Vector3(-outerRadius, altitude, 0f)
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

        void Awake()
        {
            Mesh mesh;
            GetComponent<MeshFilter>().mesh = mesh = new Mesh();
            mesh.name = meshName;

            mesh.vertices = vertices;
            mesh.triangles = triangles;
            mesh.uv = uv;
            mesh.RecalculateNormals();
        }
    }
}
