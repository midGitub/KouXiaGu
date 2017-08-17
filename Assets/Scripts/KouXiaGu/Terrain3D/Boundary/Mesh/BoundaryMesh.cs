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

        [SerializeField]
        List<CubicHexCoord> points;
        List<Vector3> vertices;
        List<int> triangles;
        List<Vector2> uv;
        Mesh Mesh;
        bool isUpdate;
        bool isInitialized;

        [ContextMenu("Destroy Mesh")]
        void OnDestroy()
        {
            DestroyImmediate(Mesh);
            Mesh = null;
        }

        void Initialize()
        {
            if (!isInitialized)
            {
                points = new List<CubicHexCoord>();
                vertices = new List<Vector3>();
                triangles = new List<int>();
                uv = new List<Vector2>();

                MeshFilter meshFilter = GetComponent<MeshFilter>();
                meshFilter.mesh = Mesh = new Mesh();
                Mesh.name = meshName;

                isInitialized = true;
            }
        }

        /// <summary>
        /// 更新坐标;
        /// </summary>
        public void UpdatePoints(IEnumerable<CubicHexCoord> points)
        {
            Initialize();
            ClearCollection();
            foreach (var point in points)
            {
                Add_internal(point);
            }
            isUpdate = true;
            UpdateMesh();
        }

        /// <summary>
        /// 添加坐标;
        /// </summary>
        public bool AddPoint(CubicHexCoord point)
        {
            Initialize();
            if (!points.Contains(point))
            {
                Add_internal(point);
                UpdateMesh();
                return true;
            }
            return false;
        }

        /// <summary>
        /// 添加坐标;
        /// </summary>
        public void AddPoints(IEnumerable<CubicHexCoord> points)
        {
            Initialize();
            foreach (var point in points)
            {
                if (!this.points.Contains(point))
                {
                    Add_internal(point);
                }
            }
            UpdateMesh();
        }

        /// <summary>
        /// 更新内部合集内容;
        /// </summary>
        void Add_internal(CubicHexCoord point)
        {
            isUpdate = true;
            points.Add(point);

            Vector3 pixelPosition = point.GetTerrainPixel();
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
            Initialize();
            int index = points.FindIndex(position);
            if (index >= 0)
            {
                Remove_internal(index, position);
                UpdateMesh();
                return true;
            }
            return false;
        }

        public void Remove(IEnumerable<CubicHexCoord> positions)
        {
            Initialize();
            foreach (var position in positions)
            {
                int index = points.FindIndex(position);
                if (index >= 0)
                {
                    Remove_internal(index, position);
                }
            }
            UpdateMesh();
        }

        void Remove_internal(int index, CubicHexCoord position)
        {
            isUpdate = true;
            points.RemoveAt(index);
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
            if (isUpdate)
            {
                Mesh.Clear();
                Mesh.vertices = vertices.ToArray();
                Mesh.triangles = triangles.ToArray();
                Mesh.uv = uv.ToArray();
                Mesh.RecalculateBounds();
                Mesh.RecalculateNormals();
                isUpdate = false;
            }
        }

        [ContextMenu("Clear")]
        public void Clear()
        {
            if (isInitialized)
            {
                ClearCollection();
                Mesh.Clear();
            }
        }

        /// <summary>
        /// 清空数据,但不清除网格;
        /// </summary>
        void ClearCollection()
        {
            points.Clear();
            vertices.Clear();
            triangles.Clear();
            uv.Clear();
        }

        [ContextMenu("TestAdd")]
        void TestAdd()
        {
            AddPoint(CubicHexCoord.Self);
            AddPoint(CubicHexCoord.DIR_North);
            AddPoint(CubicHexCoord.DIR_South);
            AddPoint(CubicHexCoord.DIR_Northeast);
            AddPoint(CubicHexCoord.DIR_Northwest);
        }

        [ContextMenu("TestRemove")]
        void TestRemove()
        {
            Remove(CubicHexCoord.Self);
        }
    }
}
