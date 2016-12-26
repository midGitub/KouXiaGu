using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KouXiaGu.Grids;
using UnityEngine;

namespace KouXiaGu.Terrain3D
{

    /// <summary>
    /// 大小为六边形外直径和内直径,宽高比约为 "1.154701";
    /// </summary>
    public class ClipRectMesh : CustomMesh
    {
        ClipRectMesh() { }

        const string MESH_NAME = "ClipRectMesh";

        /// <summary>
        /// 游戏使用的六边形参数;
        /// </summary>
        static readonly Hexagon HEXAGON = GridConvert.hexagon;

        /// <summary>
        /// 矩形大小;
        /// </summary>
        static readonly float WIDTH = (float)(HEXAGON.OuterDiameters);
        static readonly float HALF_WIDTH = WIDTH / 2;
        static readonly float HEIGHT = (float)(HEXAGON.InnerDiameters);
        static readonly float HALF_HEIGHT = HEIGHT / 2;
        const float ALTITUDE = 0;

        /// <summary>
        /// 网格顶点数据;
        /// </summary>
        static readonly Vector3[] VERTICES = new Vector3[]
            {
                new Vector3(-HALF_WIDTH , ALTITUDE, HALF_HEIGHT),
                new Vector3(HALF_WIDTH, ALTITUDE, HALF_HEIGHT),
                new Vector3(HALF_WIDTH, ALTITUDE, -HALF_HEIGHT),
                new Vector3(-HALF_WIDTH, ALTITUDE, -HALF_HEIGHT),
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


        protected override Mesh CreateMesh()
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

        protected override Mesh PublicMesh
        {
            get { return _publicMesh ?? (_publicMesh = CreateMesh()); }
            set { _publicMesh = value; }
        }
    }

}
