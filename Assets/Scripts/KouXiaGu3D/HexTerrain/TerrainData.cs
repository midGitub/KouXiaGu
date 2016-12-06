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

    }

}
