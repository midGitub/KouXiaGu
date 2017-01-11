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

            Vector3 terrainPixel = TerrainTrigger.MouseRayPoint();
            CubicHexCoord cube = TerrainConvert.Grid.GetCubic(terrainPixel);

            Vector3 cubePixel = TerrainConvert.Grid.GetPixel(cube);

            RectCoord terrainBlockCoord = TerrainChunk.ChunkGrid.GetCoord(terrainPixel);
            Vector3 terrainBlockCenter = TerrainChunk.ChunkGrid.GetCenter(terrainBlockCoord);
            CubicHexCoord terrainBlockHexCenter = TerrainChunk.GetHexCenter(terrainBlockCoord);

            Vector2 terrainBlockLocal = TerrainChunk.ChunkGrid.GetLocal(terrainPixel, out terrainBlockCoord);
            Vector2 terrainBlockUV = TerrainChunk.ChunkGrid.GetUV(terrainPixel, out terrainBlockCoord);
            float terrainHeight = Terrain3D.TerrainData.GetHeight(terrainPixel);
            RectCoord[] terrainBlocks = TerrainChunk.GetBelongChunks(cube);

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


        string OnMouseDown()
        {
            string str = "";

            //CubicHexCoord hex = GridConvert.ToHexCubic(currentPixelPosition);

            //GridsExtensions.GetNeighboursAndSelf(CubicHexCoord.Self);
            //CubicHexCoord.Self.GetNeighboursAndSelf<CubicHexCoord, HexDirections>();

            //foreach (var item in hex.GetNeighboursAndSelf<CubicHexCoord, HexDirections>())
            //{
            //    str += "\n" + item.ToString();
            //}

            return str;
        }

    }


}
