using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using KouXiaGu.Grids;

namespace KouXiaGu.Terrain3D
{

    /// <summary>
    /// 根据加入点创建区域网格;
    /// </summary>
    [DisallowMultipleComponent]
    [RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
    public sealed class BoundaryMesh : MonoBehaviour
    {
        BoundaryMesh()
        {
        }

        const string meshName = "Boundary Mesh";
        const float outerRadius = LandformConvert.OuterRadius;
        static readonly float innerRadius = (float)(Math.Sqrt(3) / 2 * outerRadius);
        const float halfOuterRadius = outerRadius / 2;
        const float altitude = 0;

        static readonly Vector3[] prefabVertices = new Vector3[]
            {
                new Vector3(-halfOuterRadius , altitude, innerRadius),
                new Vector3(halfOuterRadius, altitude, innerRadius),
                new Vector3(outerRadius, altitude, 0f),
                new Vector3(halfOuterRadius, altitude, -innerRadius),
                new Vector3(-halfOuterRadius, altitude, -innerRadius),
                new Vector3(-outerRadius, altitude, 0f)
            };

        static readonly int[] prefabTriangles = new int[]
            {
                0,1,2,
                0,2,5,
                5,2,3,
                3,4,5
            };

        static readonly Vector2[] prefabUV = new Vector2[]
            {
                new Vector2(0.25f, 1),
                new Vector2(0.75f, 1),
                new Vector2(1, 0.5f),
                new Vector2(0.75f, 0),
                new Vector2(0.25f, 0),
                new Vector2(0, 0.5f),
            };

        List<CubicHexCoord> points;
        List<Vector3> vertices;
        List<int> triangles;
        List<Vector2> uv;
        Mesh mesh;

        void Awake()
        {
            points = new List<CubicHexCoord>();
            vertices = new List<Vector3>();
            triangles = new List<int>();
            uv = new List<Vector2>();

            MeshFilter meshFilter = GetComponent<MeshFilter>();
            meshFilter.mesh = mesh = new Mesh();
            mesh.name = meshName;
        }

        [ContextMenu("Destroy Mesh")]
        void OnDestroy()
        {
            DestroyImmediate(mesh);
        }

        public bool Add(CubicHexCoord position)
        {
            if (!points.Contains(position))
            {
                points.Add(position);
                Add_internal(position);
                UpdateMesh();
                return true;
            }
            return false;
        }

        public void Add(IEnumerable<CubicHexCoord> positions)
        {
            foreach (var position in positions)
            {
                if (!points.Contains(position))
                {
                    points.Add(position);
                    Add_internal(position);
                }
            }
            UpdateMesh();
        }

        /// <summary>
        /// 更新内部合集内容;
        /// </summary>
        void Add_internal(CubicHexCoord position)
        {
            Vector3 pixelPosition = position.GetTerrainPixel();
            foreach (var vertice in prefabVertices)
            {
                Vector3 current = pixelPosition + vertice;
                vertices.Add(current);
            }

            int index = points.Count - 1;
            foreach (var triangle in prefabTriangles)
            {
                int current = index * 6 + triangle;
                triangles.Add(current);
            }

            foreach (var uvItem in prefabUV)
            {
                uv.Add(uvItem);
            }
        }

        public bool Remove(CubicHexCoord position)
        {
            int index = points.FindIndex(position);
            if (index >= 0)
            {
                points.RemoveAt(index);
                Remove_internal(index, position);
                UpdateMesh();
                return true;
            }
            return false;
        }

        public void Remove(IEnumerable<CubicHexCoord> positions)
        {
            foreach (var position in positions)
            {
                int index = points.FindIndex(position);
                if (index >= 0)
                {
                    points.RemoveAt(index);
                    Remove_internal(index, position);
                }
            }
            UpdateMesh();
        }

        void Remove_internal(int index, CubicHexCoord position)
        {
            for (int start = index * 6, loop = 0; loop < 6; loop++)
            {
                vertices.RemoveAt(start);
                uv.RemoveAt(start);
                triangles.RemoveAt(triangles.Count - 1);
                triangles.RemoveAt(triangles.Count - 1);
            }
        }

        void UpdateMesh()
        {
            mesh.Clear();
            mesh.vertices = vertices.ToArray();
            mesh.triangles = triangles.ToArray();
            mesh.uv = uv.ToArray();
            mesh.RecalculateBounds();
            mesh.RecalculateNormals();
        }


        [ContextMenu("TestAdd")]
        void TestAdd()
        {
            Awake();
            Add(CubicHexCoord.Self);
            Add(CubicHexCoord.DIR_North);
            Add(CubicHexCoord.DIR_South);
            Add(CubicHexCoord.DIR_Northeast);
            Add(CubicHexCoord.DIR_Northwest);
        }

        [ContextMenu("TestRemove")]
        void TestRemove()
        {
            Remove(CubicHexCoord.Self);
        }
    }
}
