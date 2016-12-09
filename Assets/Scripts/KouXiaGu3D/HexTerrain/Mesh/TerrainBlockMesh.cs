//using UnityEngine;

//namespace KouXiaGu.HexTerrain
//{

//    /// <summary>
//    /// 根据地形信息变换大小的网格结构;
//    /// </summary>
//    [RequireComponent(typeof(MeshFilter), typeof(MeshRenderer)), ExecuteInEditMode, DisallowMultipleComponent]
//    public class TerrainBlockMesh : MonoBehaviour
//    {
//        protected TerrainBlockMesh() { }

//        const string meshName = "Terrain Mesh";

//        //为了地形相接的地方不存在明显的缝隙,所以加上 小数 的数值;
//        static readonly float halfWidth = TerrainBlock.BlockWidth / 2 + 0.01f;
//        static readonly float halfHeight = TerrainBlock.BlockHeight / 2 + 0.01f;
//        const float altitude = 0;

//        static readonly Vector3[] vertices = new Vector3[]
//            {
//                new Vector3(-halfWidth , altitude, halfHeight),
//                new Vector3(halfWidth, altitude, halfHeight),
//                new Vector3(halfWidth, altitude, -halfHeight),
//                new Vector3(-halfWidth, altitude, -halfHeight),
//            };

//        static readonly int[] triangles = new int[]
//           {
//                0,1,2,
//                0,2,3,
//           };

//        static readonly Vector2[] uv = new Vector2[]
//           {
//                new Vector2(0f, 1f),
//                new Vector2(1f, 1f),
//                new Vector2(1f, 0f),
//                new Vector2(0f, 0f),
//           };

//        protected virtual void Awake()
//        {
//            Mesh mesh;
//            GetComponent<MeshFilter>().mesh = mesh = new Mesh();
//            mesh.name = meshName;

//            mesh.vertices = vertices;
//            mesh.triangles = triangles;
//            mesh.uv = uv;
//            mesh.RecalculateNormals();
//        }

//    }

//}
