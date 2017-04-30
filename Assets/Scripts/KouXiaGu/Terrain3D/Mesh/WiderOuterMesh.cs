using KouXiaGu.Grids;
using UnityEngine;

namespace KouXiaGu.Terrain3D
{

    /// <summary>
    /// 略宽于六边形的矩形;
    /// </summary>
    [RequireComponent(typeof(MeshFilter), typeof(MeshRenderer)), ExecuteInEditMode, DisallowMultipleComponent]
    public class WiderOuterMesh : MonoBehaviour
    {
        WiderOuterMesh() { }

        const string MESH_NAME = "WiderOuterMesh";

        /// <summary>
        /// 游戏使用的六边形参数;
        /// </summary>
        static readonly Hexagon HEXAGON = LandformConvert.hexagon;

        /// <summary>
        /// 矩形大小;
        /// </summary>
        static readonly float SIZE = (float)(HEXAGON.OuterDiameters + HEXAGON.OuterRadius);
        static readonly float HALF_SIZE = SIZE / 2;
        const float ALTITUDE = 0;

        /// <summary>
        /// 网格顶点数据;
        /// </summary>
        static readonly Vector3[] VERTICES = new Vector3[]
            {
                new Vector3(-HALF_SIZE , ALTITUDE, HALF_SIZE),
                new Vector3(HALF_SIZE, ALTITUDE, HALF_SIZE),
                new Vector3(HALF_SIZE, ALTITUDE, -HALF_SIZE),
                new Vector3(-HALF_SIZE, ALTITUDE, -HALF_SIZE),
            };

        /// <summary>
        /// 网格三角形数据;
        /// </summary>
        static readonly int[] TRIANGLES = new int[]
           {
                0,1,2,
                0,2,3,
           };

        /// <summary>
        /// 网格UV坐标数据;
        /// </summary>
        static readonly Vector2[] UV = new Vector2[]
           {
                new Vector2(0f, 1f),
                new Vector2(1f, 1f),
                new Vector2(1f, 0f),
                new Vector2(0f, 0f),
           };


        Mesh CreateMesh()
        {
            Mesh mesh = new Mesh();

            mesh.name = MESH_NAME;
            mesh.vertices = VERTICES;
            mesh.triangles = TRIANGLES;
            mesh.uv = UV;
            mesh.RecalculateNormals();

            return mesh;
        }

        static Mesh _publicMesh;

        Mesh PublicMesh
        {
            get { return _publicMesh ?? (_publicMesh = CreateMesh()); }
            set { _publicMesh = value; }
        }

        void Reset()
        {
            PublicMesh = CreateMesh();
            GetComponent<MeshFilter>().sharedMesh = PublicMesh;
        }

        void Awake()
        {
            GetComponent<MeshFilter>().sharedMesh = PublicMesh;
        }

    }

}
