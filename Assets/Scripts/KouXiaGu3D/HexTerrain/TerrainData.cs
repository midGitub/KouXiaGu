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

            foreach (var item in HexGrids.GetHexRange(CubicHexCoord.Zero, 10))
            {
                try
                {
                    terrainMap.Add(item, new LandformNode((item.X & 1) == 0 ? 10 : 20, 0));
                }
                catch (ArgumentException)
                {
                    Debug.Log(item);
                }
            }
        }

        [ContextMenu("烘焙测试")]
        void Test_Baking()
        {
            BakingQueue.GetInstance.Enqueue(new BakingRequest(terrainMap, ShortVector2.Zero));
            BakingQueue.GetInstance.Enqueue(new BakingRequest(terrainMap, ShortVector2.Left));
            BakingQueue.GetInstance.Enqueue(new BakingRequest(terrainMap, ShortVector2.Right));
            BakingQueue.GetInstance.Enqueue(new BakingRequest(terrainMap, ShortVector2.Up));
            BakingQueue.GetInstance.Enqueue(new BakingRequest(terrainMap, ShortVector2.Down));
            BakingQueue.GetInstance.Enqueue(new BakingRequest(terrainMap, ShortVector2.Down + ShortVector2.Left));
            BakingQueue.GetInstance.Enqueue(new BakingRequest(terrainMap, ShortVector2.Down + ShortVector2.Right));
            BakingQueue.GetInstance.Enqueue(new BakingRequest(terrainMap, ShortVector2.Up + ShortVector2.Left));
            BakingQueue.GetInstance.Enqueue(new BakingRequest(terrainMap, ShortVector2.Up + ShortVector2.Right));
        }

    }

}
