//using JiongXiaGu.Grids;
//using System.Linq;
//using UniRx;
//using UnityEngine;
//using UnityEngine.UI;

//namespace JiongXiaGu.Tests
//{

//    /// <summary>
//    /// 网格内容显示;
//    /// </summary>
//    [DisallowMultipleComponent]
//    sealed class GridContent : MonoBehaviour
//    {
//        GridContent()
//        {
//        }

//        [SerializeField]
//        Text textObject;
//        IWorld world;

//        void Update()
//        {
//            textObject.text = GetLog(Input.mousePosition);
//        }

//        string GetLog(Vector3 mousePosition)
//        {
//            LandformRay landformRay = LandformRay.Instance;
//            Vector3 terrainPixel = landformRay.MouseRayPointOrDefault();
//            CubicHexCoord cube = LandformConvert.Grid.GetCubic(terrainPixel);
//            Vector3 cubePixel = LandformConvert.Grid.GetPixel(cube);

//            RectCoord chunkCoord = LandformChunkInfo.ChunkGrid.GetCoord(terrainPixel);
//            Vector3 chunkCenter = LandformChunkInfo.ChunkGrid.GetCenter(chunkCoord);
//            CubicHexCoord chunkHexCenter = LandformChunkInfo.GetChunkHexCenter(chunkCoord);
//            Vector2 chunkLocal = LandformChunkInfo.ChunkGrid.GetLocal(terrainPixel, out chunkCoord);
//            Vector2 chunkUV = LandformChunkInfo.ChunkGrid.GetUV(terrainPixel, out chunkCoord);
//            RectCoord[] belongChunks = LandformChunkInfo.GetBelongChunks(cube).ToArray();

//            string str = 
//                "视窗坐标 :" + mousePosition
//                + "\n节点: 像素坐标:" + terrainPixel
//                + "\n    立方坐标:" + cube
//                + " 中心像素:" + cubePixel

//                + "\n块: 矩形坐标:" + chunkCoord
//                + "\n    中心像素:" + chunkCenter
//                + " 中心立方:" + chunkHexCenter
//                + "\n    本地坐标:" + chunkLocal
//                + " 块UV坐标:" + chunkUV

//                + GetBelongChunksString(belongChunks)
//                ;

//            return str;
//        }

//        string GetBelongChunksString(RectCoord[] chunkCoords)
//        {
//            string str = "\n";
//            for (int i = 0; i < chunkCoords.Length; i++)
//            {
//                str += " 所属" + i.ToString() + ":" + chunkCoords[i].ToString();
//            }
//            return str;
//        }
//    }
//}
