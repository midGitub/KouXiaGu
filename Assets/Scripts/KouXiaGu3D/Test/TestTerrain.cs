using KouXiaGu.Grids;
using KouXiaGu.Terrain3D;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace KouXiaGu.Test
{

    
    public class TestTerrain : MonoBehaviour
    {

        [SerializeField]
        Text textObject;

        Vector3 currentPixelPosition;
        string checkPointText;

        void Awake()
        {
            textObject = textObject ?? GetComponent<Text>();
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
            str += GetTestPointsLog(mousePosition);
            str += OnMouseDown();

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
            Vector3 pixel = MouseConvert.MouseToPixel();
            //ShortVector2 offset = HexGrids.PixelToOffset(pixel);
            CubicHexCoord cube = HexGridConvert.ToHexCubic(pixel);

            //Vector3 offsetPixel = HexGrids.OffsetToPixel(offset);
            //CubicHexCoord offsetCube = HexGrids.OffsetToHex(offset);

            Vector3 cubePixel = HexGridConvert.ToPixel(cube);
            //ShortVector2 cubeOffset = HexGrids.HexToOffset(cube);

            ShortVector2 terrainBlockCoord = Terrain3D.TerrainData.PixelToBlock(pixel);
            Vector3 terrainBlockCenter = Terrain3D.TerrainData.BlockToPixelCenter(terrainBlockCoord);
            CubicHexCoord terrainBlockHexCenter = Terrain3D.TerrainData.BlockToHexCenter(terrainBlockCoord);
            Rect terrainBlockRect = Terrain3D.TerrainData.CenterToRect(terrainBlockCenter);
            Vector2 terrainBlockLocal = Terrain3D.TerrainData.PixelToLocal(pixel);
            Vector2 terrainBlockUV = Terrain3D.TerrainData.PixelToUV(pixel);
            float terrainHeight = Terrain3D.TerrainData.GetHeight(pixel);
            ShortVector2[] terrainBlocks = Terrain3D.TerrainData.GetBelongBlocks(pixel);

            string str = "";

            str += "\n基本数值: 像素:" + pixel
                //+ "偏移:" + offset
                + "立方:" + cube

                //+ "\n偏移转换: 中心:" + offsetPixel
                //+ "立方:" + offsetCube

                + "\n立方转换: 中心:" + cubePixel
                //+ "偏移:" + cubeOffset

                + "\n地貌块: 块编号:" + terrainBlockCoord
                + "中心:" + terrainBlockCenter
                + "立方:" + terrainBlockHexCenter
                + "矩形:" + terrainBlockRect
                + "\n块坐标:" + terrainBlockLocal
                + "UV:" + terrainBlockUV
                + "高度:" + terrainHeight + ";"
                + "所属1:" + terrainBlocks[0]
                + "所属2:" + terrainBlocks[1];

            return str;
        }

        string OnMouseDown()
        {
            string str = "";

            CubicHexCoord hex = HexGridConvert.ToHexCubic(currentPixelPosition);

            foreach (var item in hex.GetNeighboursAndSelf())
            {
                str += "\n" + item.ToString();
            }

            return str;
        }

    }


}
