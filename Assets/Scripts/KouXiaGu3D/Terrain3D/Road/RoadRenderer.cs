using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace KouXiaGu.Terrain3D
{

    /// <summary>
    /// 根据样条线生成网格,
    /// </summary>
    [DisallowMultipleComponent, ExecuteInEditMode]
    public sealed class RoadRenderer : MonoBehaviour
    {
        RoadRenderer() { }

        const string MESH_NAME = "Road Mesh";


        /// <summary>
        /// 道路宽度;
        /// </summary>
        [SerializeField, Range(0.01f, TerrainConvert.OuterRadius)]
        float width;

        /// <summary>
        /// 细分程度;
        /// </summary>
        [SerializeField, Range(1, 60)]
        int segmentPoints;


        /// <summary>
        /// 设置样条线;
        /// </summary>
        public void SetSpline(IList<Vector3> path)
        {
            List<Offset> offsets = CalculatedOffsets(path);
            InitMesh(offsets);
        }

        /// <summary>
        /// 计算出点偏移;
        /// </summary>
        List<Offset> CalculatedOffsets(IList<Vector3> spline)
        {
            List<Offset> offsets = new List<Offset>(spline.Count);
            int endIndex = spline.Count - 1;

            for (int i = 0; i < endIndex; i++)
            {
                Offset offset = GetOffset(spline[i], spline[Math.Min(i + 1, endIndex)]);
                offsets.Add(offset);
            }

            return offsets;
        }

        Offset GetOffset(Vector3 from, Vector3 to)
        {
            Offset offset = new Offset();

            const double LEFT_ANGLE = -90 * (Math.PI / 180);
            const double RIGHT_ANGLE = 90 * (Math.PI / 180);

            double angle = AngleY(from, to);
            offset.Original = from;
            offset.Left = Circle(width, LEFT_ANGLE + angle) + from;
            offset.Right = Circle(width, RIGHT_ANGLE + angle) + from;

            return offset;
        }

        /// <summary>
        /// 获取 from 到 to 的角度(忽略Y),单位弧度;原型:Math.Atan2()
        /// </summary>
        double AngleY(Vector3 from, Vector3 to)
        {
            double angle = (Math.Atan2((to.x - from.x), (to.z - from.z)));
            return angle;
        }

        /// <summary>
        /// 获取到半径圆上任何一点,忽略Y轴;
        /// </summary>
        Vector3 Circle(float radius, double angle)
        {
            double x = Math.Sin(angle) * radius;
            double y = Math.Cos(angle) * radius;

            return new Vector3((float)x, 0, (float)y);
        }


        /// <summary>
        /// 初始化网格结构;
        /// </summary>
        void InitMesh(IList<Offset> offsets)
        {
            MeshFilter meshFilter= GetComponent<MeshFilter>();
            Mesh mesh;

#if UNITY_EDITOR
            if (!Application.isPlaying)
            {
                mesh = new Mesh();
                mesh.name = MESH_NAME;
            }
#endif
            else if (meshFilter.mesh != null && meshFilter.mesh.name == MESH_NAME)
            {
                mesh = meshFilter.mesh;
            }
            else
            {
                mesh = new Mesh();
                mesh.name = MESH_NAME;
            }

            InitMesh(offsets, ref mesh);
            meshFilter.mesh = mesh;
        }

        /// <summary>
        /// 初始化网格结构;
        /// </summary>
        void InitMesh(IList<Offset> offsets, ref Mesh mesh)
        {
            int verticesCapacity = offsets.Count * 4;
            int trianglesCapacity = offsets.Count * 6;

            List<Vector3> vertices = new List<Vector3>(verticesCapacity);
            List<Vector2> uv = new List<Vector2>(verticesCapacity);
            List<int> triangles = new List<int>(trianglesCapacity);

            for (int i = 0; i < offsets.Count - 1; i++)
            {
                vertices.Add(offsets[i + 1].Left);
                uv.Add(new Vector2(0, 1));

                vertices.Add(offsets[i + 1].Right);
                uv.Add(new Vector2(1, 1));

                vertices.Add(offsets[i].Right);
                uv.Add(new Vector2(1, 0));

                vertices.Add(offsets[i].Left);
                uv.Add(new Vector2(0, 0));

                int j = i * 4;

                triangles.AddRange(
                    new int[]
                    {
                        0 + j,
                        1 + j,
                        2 + j,

                        0 + j,
                        2 + j,
                        3 + j,
                    }
                    );
            }

            mesh.vertices = vertices.ToArray();
            mesh.triangles = triangles.ToArray();
            mesh.uv = uv.ToArray();
            mesh.RecalculateNormals();
        }

        [Serializable]
        struct Offset
        {
            public Offset(Vector3 original, Vector3 left, Vector3 right)
            {
                this.Original = original;
                this.Left = left;
                this.Right = right;
            }

            public Vector3 Original;
            public Vector3 Left;
            public Vector3 Right;
        }

    }

}
