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
            int[] aa = new int[] { 10, 20, 30};

            terrainMap.Add(CubicHexCoord.Zero, new LandformNode(10, 0));

            foreach (var item in HexGrids.GetHexRange(CubicHexCoord.Zero, 10))
            {
                try
                {
                    terrainMap.Add(item, new LandformNode(aa[UnityEngine.Random.Range(0,3)], UnityEngine.Random.Range(0, 360)));
                }
                catch (ArgumentException)
                {
                    Debug.Log(item);
                }
            }
            Debug.Log("12313");
            Debug.Log((((4 - 1) / 2 + (4 - 1)) * 2).ToString());
            Debug.LogWarning((((4 - 1) / 2 + (4 - 1)) * 2).ToString());
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
