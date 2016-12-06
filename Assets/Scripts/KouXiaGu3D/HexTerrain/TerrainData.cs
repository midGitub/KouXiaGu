using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace KouXiaGu.HexTerrain
{

    /// <summary>
    /// 保存当前游戏地形数据;
    /// </summary>
    public sealed class TerrainData : UnitySingleton<TerrainData>
    {

        Map2D<CubicHexCoord, LandformNode> terrainMap;

        [SerializeField]
        Camera bbbCamera;

        void Awake()
        {
            terrainMap = new Map2D<CubicHexCoord, LandformNode>();
            bbbCamera.aspect = TerrainBlock.CameraAspect;
            bbbCamera.orthographicSize = TerrainBlock.CameraSize;
            bbbCamera.transform.rotation = TerrainBlock.CameraRotation;
        }

        //Test
        void Start()
        {
            terrainMap.Add(CubicHexCoord.Zero, new LandformNode(10));

            foreach (var item in HexGrids.GetNeighbours(CubicHexCoord.Zero))
            {
                terrainMap.Add(item.Value, new LandformNode(20));
            }
        }

       

        //RenderTexture renderTexture;

        //int size = 1000;

        //void Update()
        //{
        //    renderTexture = RenderTexture.GetTemporary(size, size, 24, RenderTextureFormat.ARGB32, RenderTextureReadWrite.Default, 8);
        //    bbbCamera.targetTexture = renderTexture;
        //    bbbCamera.Render();
        //    bbbCamera.targetTexture = null;
        //    RenderTexture.active = renderTexture;
        //    var ttt = new Texture2D(size, size);
        //    Destroy(ttt);
        //    RenderTexture.active = null;
        //    renderTexture.Release();
        //    RenderTexture.ReleaseTemporary(renderTexture);
        //}


        ///// <summary>
        ///// 烘焙这个位置的地貌;
        ///// </summary>
        //public void BakingLandform(ShortVector2 mapPoint)
        //{
        //    BakingRequest bakingRequest = new BakingRequest(mapPoint, terrainMap);
        //    BakingQueue.GetInstance.Enqueue(bakingRequest);
        //}

        //[ContextMenu("Test_Baking")]
        //void Test_Baking()
        //{
        //    BakingLandform(ShortVector2.Zero);
        //    BakingLandform(ShortVector2.Up);
        //    BakingLandform(ShortVector2.Down);
        //    BakingLandform(ShortVector2.Left);
        //    BakingLandform(ShortVector2.Right);
        //    BakingLandform(new ShortVector2(1, 1));
        //    BakingLandform(new ShortVector2(-1, -1));
        //    BakingLandform(new ShortVector2(-1, 1));
        //    BakingLandform(new ShortVector2(1, -1));
        //}

    }

}
