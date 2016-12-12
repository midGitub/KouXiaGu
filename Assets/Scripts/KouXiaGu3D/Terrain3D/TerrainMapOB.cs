using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using System.IO;
using KouXiaGu.Grids;

namespace KouXiaGu.Terrain3D
{

    /// <summary>
    /// 游戏地形数据保存;
    /// </summary>
    public sealed class TerrainMapOB : UnitySingleton<TerrainMapOB>
    {

        Map<CubicHexCoord, TerrainNode> terrainMap;

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
            BasicRenderer.GetInstance.Enqueue(new RenderRequest(terrainMap, RectCoord.Self));
            BasicRenderer.GetInstance.Enqueue(new RenderRequest(terrainMap, RectCoord.West));
            BasicRenderer.GetInstance.Enqueue(new RenderRequest(terrainMap, RectCoord.East));
            BasicRenderer.GetInstance.Enqueue(new RenderRequest(terrainMap, RectCoord.North));
            BasicRenderer.GetInstance.Enqueue(new RenderRequest(terrainMap, RectCoord.South));
            BasicRenderer.GetInstance.Enqueue(new RenderRequest(terrainMap, RectCoord.South + RectCoord.West));
            BasicRenderer.GetInstance.Enqueue(new RenderRequest(terrainMap, RectCoord.South + RectCoord.East));
            BasicRenderer.GetInstance.Enqueue(new RenderRequest(terrainMap, RectCoord.North + RectCoord.West));
            BasicRenderer.GetInstance.Enqueue(new RenderRequest(terrainMap, RectCoord.North + RectCoord.East));

            //BasicRenderer.GetInstance.Enqueue(new RenderRequest(terrainMap, new ShortVector2(1,0)));
            //BasicRenderer.GetInstance.Enqueue(new RenderRequest(terrainMap, new ShortVector2(1, -1)));

        }


        [ContextMenu("保存地图")]
        void SaveMap()
        {
            string filePath = Path.Combine(Application.dataPath, "MAP.temp");
            SerializeHelper.SerializeProtoBuf(filePath, terrainMap);
        }

        Map<CubicHexCoord, TerrainNode> LoadMap()
        {
            string filePath = Path.Combine(Application.dataPath, "MAP.temp");

            if (File.Exists(filePath))
            {
                return SerializeHelper.DeserializeProtoBuf<Map<CubicHexCoord, TerrainNode>>(filePath);
            }
            return null;
        }


        Map<CubicHexCoord, TerrainNode> RandomMap()
        {
            Map<CubicHexCoord, TerrainNode> terrainMap = new Map<CubicHexCoord, TerrainNode>();
              int[] aa = new int[] { 10, 20, 30, 20 };

            terrainMap.Add(CubicHexCoord.Self, new TerrainNode(10, 0));

            foreach (var item in CubicHexCoord.GetHexRange(CubicHexCoord.Self, 10))
            {
                try
                {
                    terrainMap.Add(item, new TerrainNode(aa[UnityEngine.Random.Range(0, aa.Length)], UnityEngine.Random.Range(0, 360)));
                }
                catch (ArgumentException)
                {
                    //Debug.Log(item);
                }
            }
            return terrainMap;
        }

        [ContextMenu("输出所有地图文件")]
        void showAll()
        {
            var paths = BlockProtoBufExtensions.GetFilePaths(Application.dataPath);
            Debug.Log(paths.ToLog());
        }


    }

}
