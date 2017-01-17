using System;
using System.Collections.Generic;
using UnityEngine;

namespace KouXiaGu.Terrain3D
{

    /// <summary>
    /// 根据样条线生成网格,
    /// </summary>
    [RequireComponent(typeof(MeshFilter), typeof(MeshRenderer)), DisallowMultipleComponent]
    public sealed class RoadMesh : MonoBehaviour
    {
        RoadMesh() { }

        const string MESH_NAME = "Road Mesh";

        /// <summary>
        /// 道路宽度;
        /// </summary>
        float roadWidth;

        List<Offset> roadSpline;

        MeshFilter meshFilter;


        /// <summary>
        /// 是否已经生成网格?
        /// </summary>
        public bool IsBuilt
        {
            get { return roadSpline != null || meshFilter.mesh.name != MESH_NAME; }
        }


        void Awake()
        {
            meshFilter = GetComponent<MeshFilter>();
        }

        /// <summary>
        /// 设置道路宽度,若网格已经构建,则重新构建网格;
        /// </summary>
        public void SetRoadWidth(float width)
        {
            if (IsBuilt)
                InitMesh();
            else
                this.roadWidth = width;
        }


        /// <summary>
        /// 设置样条线;
        /// </summary>
        public void SetSpline(IList<Vector3> spline)
        {
            SetSpline(spline, roadWidth);
        }

        /// <summary>
        /// 设置样条线,并且重置宽度信息;
        /// </summary>
        public void SetSpline(IList<Vector3> spline, float width)
        {
            this.roadWidth = width;
            roadSpline = CalculatedOffsets(spline);
            InitMesh();
        }


        /// <summary>
        /// 计算出点偏移;
        /// </summary>
        List<Offset> CalculatedOffsets(IList<Vector3> spline)
        {
            List<Offset> offsets = new List<Offset>(spline.Count);
            int endIndex = spline.Count - 1;

            for (int i = 0; i < spline.Count; i++)
            {
                Offset offset;
                if (i == endIndex)
                {
                    offset = GetOffset(spline[i], spline[i - 1]);
                    offset.Reversal();
                }
                else
                {
                    offset = GetOffset(spline[i], spline[i + 1]);
                }
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
            offset.Left = Circle(roadWidth, LEFT_ANGLE + angle) + from;
            offset.Right = Circle(roadWidth, RIGHT_ANGLE + angle) + from;

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
        void InitMesh()
        {
            Mesh mesh;

#if UNITY_EDITOR
            meshFilter = meshFilter ?? GetComponent<MeshFilter>();
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

            InitMesh(ref mesh);
            meshFilter.mesh = mesh;
        }

        /// <summary>
        /// 初始化网格结构;
        /// </summary>
        void InitMesh(ref Mesh mesh)
        {
            int verticesCapacity = roadSpline.Count * 4;
            int trianglesCapacity = roadSpline.Count * 6;

            List<Vector3> vertices = new List<Vector3>(verticesCapacity);
            List<Vector2> uv = new List<Vector2>(verticesCapacity);
            List<int> triangles = new List<int>(trianglesCapacity);

            for (int i = 0; i < roadSpline.Count - 1; i++)
            {
                vertices.Add(roadSpline[i + 1].Left);
                uv.Add(new Vector2(0, 1));

                vertices.Add(roadSpline[i + 1].Right);
                uv.Add(new Vector2(1, 1));

                vertices.Add(roadSpline[i].Right);
                uv.Add(new Vector2(1, 0));

                vertices.Add(roadSpline[i].Left);
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

            /// <summary>
            /// 交换 Left 和 Right;
            /// </summary>
            public void Reversal()
            {
                Vector3 temp = Left;
                Left = Right;
                Right = temp;
            }

        }

    }

}
