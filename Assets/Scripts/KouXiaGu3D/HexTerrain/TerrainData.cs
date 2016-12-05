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

        Map2D<LandformNode> terrainMap;

        void Awake()
        {
            terrainMap = new Map2D<LandformNode>();
        }

        //Test
        void Start()
        {
            terrainMap.Add(new ShortVector2(0, 0), new LandformNode(10));
            terrainMap.Add(ShortVector2.Up, new LandformNode(20));
            terrainMap.Add(ShortVector2.Down, new LandformNode(20));
            terrainMap.Add(ShortVector2.Left, new LandformNode(20));
            terrainMap.Add(ShortVector2.Right, new LandformNode(20));
            terrainMap.Add(new ShortVector2(1,1), new LandformNode(20));
            terrainMap.Add(new ShortVector2(-1, -1), new LandformNode(20));
            terrainMap.Add(new ShortVector2(-1, 1), new LandformNode(20));
            terrainMap.Add(new ShortVector2(1, -1), new LandformNode(20));
        }

        /// <summary>
        /// 烘焙这个位置的地貌;
        /// </summary>
        public void BakingLandform(ShortVector2 mapPoint)
        {
            BakingRequest bakingRequest = new BakingRequest(mapPoint, terrainMap);
            BakingQueue.GetInstance.Enqueue(bakingRequest);
        }

        [ContextMenu("Test_Baking")]
        void Test_Baking()
        {
            BakingLandform(ShortVector2.Zero);
            BakingLandform(ShortVector2.Up);
            BakingLandform(ShortVector2.Down);
            BakingLandform(ShortVector2.Left);
            BakingLandform(ShortVector2.Right);
            BakingLandform(new ShortVector2(1, 1));
            BakingLandform(new ShortVector2(-1, -1));
            BakingLandform(new ShortVector2(-1, 1));
            BakingLandform(new ShortVector2(1, -1));
        }

    }

}
