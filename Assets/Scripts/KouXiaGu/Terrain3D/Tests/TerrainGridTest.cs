﻿using KouXiaGu.Grids;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace KouXiaGu.Terrain3D.Tests
{

    public class TerrainGridTest : MonoBehaviour
    {

        [SerializeField]
        Text textObject;

        void Awake()
        {
            textObject = textObject ?? GetComponent<Text>();
        }

        void Start()
        {
            this.ObserveEveryValueChanged(_ => UnityEngine.Input.mousePosition).
                SubscribeToText(textObject, TextUpdate);
        }

        string TextUpdate(Vector3 mousePosition)
        {
            string str = "";

            str += GetScreenPoint(mousePosition);
            str += GetTestPointsLog(mousePosition);

            return str;
        }

        string GetScreenPoint(Vector2 mousePosition)
        {
            string str = "";

            str += "视窗坐标 :" + mousePosition;

            return str;
        }


        string GetTestPointsLog(Vector3 mousePosition)
        {
            LandformRay landformRay = new LandformRay();

            Vector3 terrainPixel = landformRay.MouseRayPointOrDefault();
            CubicHexCoord cube = TerrainConvert.Grid.GetCubic(terrainPixel);

            Vector3 cubePixel = TerrainConvert.Grid.GetPixel(cube);

            RectCoord terrainBlockCoord = ChunkInfo.ChunkGrid.GetCoord(terrainPixel);
            Vector3 terrainBlockCenter = ChunkInfo.ChunkGrid.GetCenter(terrainBlockCoord);
            CubicHexCoord terrainBlockHexCenter = ChunkInfo.GetChunkHexCenter(terrainBlockCoord);

            Vector2 terrainBlockLocal = ChunkInfo.ChunkGrid.GetLocal(terrainPixel, out terrainBlockCoord);
            Vector2 terrainBlockUV = ChunkInfo.ChunkGrid.GetUV(terrainPixel, out terrainBlockCoord);
            float terrainHeight = /*TerrainData.GetHeight(terrainPixel);*/ 0;
            RectCoord[] terrainBlocks = ChunkInfo.GetBelongChunks(cube);

            string str = "";

            str = str
                + "\n基本数值: 地形像素:" + terrainPixel
                + "立方:" + cube

                + "\n立方转换: 中心:" + cubePixel

                + "\n地貌块: 块编号:" + terrainBlockCoord
                + "中心:" + terrainBlockCenter
                + "立方:" + terrainBlockHexCenter

                + "\n块坐标:" + terrainBlockLocal
                + "UV:" + terrainBlockUV

                + "\n高度:" + terrainHeight
                + "所属1:" + terrainBlocks[0]
                + "所属2:" + terrainBlocks[1]
                ;

            return str;
        }

    }


}
