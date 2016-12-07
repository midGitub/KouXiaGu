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

        void Awake()
        {
            terrainMap = new Map2D<CubicHexCoord, LandformNode>();
        }

        //Test
        void Start()
        {
            terrainMap.Add(CubicHexCoord.Zero, new LandformNode(10, 0));

            foreach (var item in HexGrids.GetNeighbours(CubicHexCoord.Zero))
            {
                terrainMap.Add(item.Value, new LandformNode(20, 0));
            }
        }

        [ContextMenu("烘焙测试")]
        void Test_Baking()
        {
            BakingQueue.GetInstance.Enqueue(new BakingRequest(terrainMap, ShortVector2.Zero));
        }

    }

}
