using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KouXiaGu.HexTerrain;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace KouXiaGu.Test
{


    public class Test_Point : MonoBehaviour
    {

        Text textObject;

        Vector3 currentPixelPosition;
        string checkPointText;

        void Awake()
        {
            textObject = GetComponent<Text>();
        }

        void Start()
        {
            this.ObserveEveryValueChanged(_ => UnityEngine.Input.mousePosition).
                SubscribeToText(textObject, TextUpdate);

            this.ObserveEveryValueChanged(_ => Input.GetKeyDown(KeyCode.Mouse0)).
                Subscribe(_ => currentPixelPosition = MouseConvert.MouseToPixel());

        }

        string TextUpdate(Vector3 mousePosition)
        {
            string str = "";

            str += GetScreenPoint(mousePosition);
            str += GetPlanePoint(mousePosition);
            str += OnMouseDown();

            return str;
        }

        string GetScreenPoint(Vector2 mousePosition)
        {
            string str = "";

            str += "视窗坐标 :" + mousePosition;

            return str;
        }

        string GetPlanePoint(Vector3 mousePosition)
        {
            Vector3 pixel = MouseConvert.MouseToPixel();
            ShortVector2 offset = HexGrids.PixelToOffset(pixel);
            CubicHexCoord cube = HexGrids.PixelToHex(pixel);

            Vector3 offsetPixel = HexGrids.OffsetToPixel(offset);
            CubicHexCoord offsetCube = HexGrids.OffsetToHex(offset);

            Vector3 cubePixel = HexGrids.HexToPixel(cube);
            ShortVector2 cubeOffset = HexGrids.HexToOffset(cube);

            ShortVector2 terrainBlockCoord = TerrainBlock.PixelToBlockCoord(pixel);
            Vector3 terrainBlockCenter = TerrainBlock.BlockCoordToPixelCenter(terrainBlockCoord);
            CubicHexCoord terrainBlockHexCenter = TerrainBlock.BlockCoordToHexCenter(terrainBlockCoord);

            string str = "";

            str += "\n基本数值: 像素:" + pixel
                + "偏移:" + offset
                + "立方:" + cube

                + "\n偏移转换: 中心:" + offsetPixel
                + "立方:" + offsetCube

                + "\n立方转换: 中心:" + cubePixel
                + "偏移:" + cubeOffset

                + "\n地貌块: 块编号:" + terrainBlockCoord
                + "中心:" + terrainBlockCenter
                + "立方:" + terrainBlockHexCenter;

            return str;
        }

        string OnMouseDown()
        {
            string str = "";

            ShortVector2 offset = HexGrids.PixelToOffset(currentPixelPosition);

            foreach (var item in HexGrids.GetNeighboursAndSelf(offset))
            {
                str += "\n" + item.ToString();
            }

            return str;
        }

    }


}
