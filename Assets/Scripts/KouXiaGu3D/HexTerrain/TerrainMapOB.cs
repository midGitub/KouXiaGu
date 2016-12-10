using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using System.IO;

namespace KouXiaGu.HexTerrain
{

    /// <summary>
    /// 游戏地形数据保存;
    /// </summary>
    public sealed class TerrainMapOB : UnitySingleton<TerrainMapOB>
    {

        Map<CubicHexCoord, LandformNode> terrainMap;

        void Awake()
        {
            //terrainMap = new Map2D<CubicHexCoord, LandformNode>();
        }

        //Test
        void Start()
        {
            terrainMap = LoadMap();
            if (terrainMap == null)
            {
                terrainMap = RandomMap();
            }
        }

        [ContextMenu("烘焙测试")]
        void Test_Baking()
        {
            BasicRenderer.GetInstance.Enqueue(new RenderRequest(terrainMap, ShortVector2.Zero));
            BasicRenderer.GetInstance.Enqueue(new RenderRequest(terrainMap, ShortVector2.Left));
            BasicRenderer.GetInstance.Enqueue(new RenderRequest(terrainMap, ShortVector2.Right));
            BasicRenderer.GetInstance.Enqueue(new RenderRequest(terrainMap, ShortVector2.Up));
            BasicRenderer.GetInstance.Enqueue(new RenderRequest(terrainMap, ShortVector2.Down));
            BasicRenderer.GetInstance.Enqueue(new RenderRequest(terrainMap, ShortVector2.Down + ShortVector2.Left));
            BasicRenderer.GetInstance.Enqueue(new RenderRequest(terrainMap, ShortVector2.Down + ShortVector2.Right));
            BasicRenderer.GetInstance.Enqueue(new RenderRequest(terrainMap, ShortVector2.Up + ShortVector2.Left));
            BasicRenderer.GetInstance.Enqueue(new RenderRequest(terrainMap, ShortVector2.Up + ShortVector2.Right));

            //BasicRenderer.GetInstance.Enqueue(new RenderRequest(terrainMap, new ShortVector2(1,0)));
            //BasicRenderer.GetInstance.Enqueue(new RenderRequest(terrainMap, new ShortVector2(1, -1)));

        }


        [ContextMenu("保存地图")]
        void SaveMap()
        {
            string filePath = Path.Combine(Application.dataPath, "MAP.temp");
            SerializeHelper.SerializeProtoBuf(filePath, terrainMap);
        }

        Map<CubicHexCoord, LandformNode> LoadMap()
        {
            string filePath = Path.Combine(Application.dataPath, "MAP.temp");

            if (File.Exists(filePath))
            {
                return SerializeHelper.DeserializeProtoBuf<Map<CubicHexCoord, LandformNode>>(filePath);
            }
            return null;
        }


        Map<CubicHexCoord, LandformNode> RandomMap()
        {
            Map<CubicHexCoord, LandformNode> terrainMap = new Map<CubicHexCoord, LandformNode>();
              int[] aa = new int[] { 10, 20, 30, 20 };

            terrainMap.Add(CubicHexCoord.Zero, new LandformNode(10, 0));

            foreach (var item in HexGrids.GetHexRange(CubicHexCoord.Zero, 10))
            {
                try
                {
                    terrainMap.Add(item, new LandformNode(aa[UnityEngine.Random.Range(0, aa.Length)], UnityEngine.Random.Range(0, 360)));
                }
                catch (ArgumentException)
                {
                    //Debug.Log(item);
                }
            }
            return terrainMap;
        }

    }

}
