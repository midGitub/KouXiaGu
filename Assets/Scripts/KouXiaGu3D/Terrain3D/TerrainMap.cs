using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KouXiaGu.Grids;
using UnityEngine;

namespace KouXiaGu.Terrain3D
{

    /// <summary>
    /// 地形地图保存和提供;
    /// </summary>
    public class TerrainMap : MonoBehaviour
    {

        /// <summary>
        /// 地图分块大小;
        /// </summary>
        const short MapBlockSize = 1000;

        /// <summary>
        /// 地形地图结构;
        /// </summary>
        static readonly BlockMapRecord<TerrainNode> terrainMap = new BlockMapRecord<TerrainNode>(MapBlockSize);

        /// <summary>
        /// 地图当前的进行状态;
        /// </summary>
        static ArchiveState state = ArchiveState.Empty;
        
        /// <summary>
        /// 地图当前的进行状态;
        /// </summary>
        public static ArchiveState State
        {
            get { return state; }
            private set { state = value; }
        }

        /// <summary>
        /// 地形地图;
        /// </summary>
        public static IMap<CubicHexCoord, TerrainNode> Map
        {
            get { return terrainMap; }
        }



        /// <summary>
        /// 保存需要保存为文件;
        /// </summary>
        public static void Save(string directoryPath)
        {
            if (State != ArchiveState.Complete)
                throw new Exception("地图尚未准备完毕;");

            ArchiveState currentState = State;
            try
            {
                State = ArchiveState.Writing;
                terrainMap.Save(directoryPath, FileMode.Create);
                State = currentState;
            }
            finally
            {
                State = currentState;
            }
        }

        /// <summary>
        /// 将所有地图保存为文件;
        /// </summary>
        public static void SaveAll(string directoryPath)
        {
            if (State != ArchiveState.Complete)
                throw new Exception("地图尚未准备完毕;");

            ArchiveState currentState = State;
            try
            {
                State = ArchiveState.Writing;
                terrainMap.SaveAll(directoryPath, FileMode.Create);
                State = currentState;
            }
            finally
            {
                State = currentState;
            }
        }

        /// <summary>
        /// 读取文件夹下的地图;
        /// </summary>
        public static void Load(string directoryPath)
        {
            if (State == ArchiveState.Writing)
                throw new Exception("地图正在保存;");

            ArchiveState currentState = State;
            try
            {
                State = ArchiveState.Reading;
                terrainMap.Load(directoryPath);
                State = ArchiveState.Complete;
            }
            finally
            {
                State = currentState;
            }
        }

        /// <summary>
        /// 确认这个文件夹下是否存在地图文件;
        /// </summary>
        public static bool ConfirmDirectory(string directoryPath)
        {
            var paths = BlockProtoBufExtensions.GetFilePaths(directoryPath);
            return paths.FirstOrDefault() != string.Empty;
        }

        #region Test

        /// <summary>
        /// 使用测试的地图文件夹;
        /// </summary>
        [SerializeField]
        string mapDirectory;

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
        }

        [ContextMenu("保存修改的地图")]
        void SaveMap()
        {
            Save(mapDirectory);
        }

        [ContextMenu("保存地图")]
        void SaveAllMap()
        {
            SaveAll(mapDirectory);
        }

        [ContextMenu("读取地图")]
        void LoadMap()
        {
            Load(mapDirectory);
        }

        [ContextMenu("输出所有地图文件")]
        void ShowAllMapFile()
        {
            var paths = BlockProtoBufExtensions.GetFilePaths(mapDirectory);
            Debug.Log(paths.ToLog());
        }

        /// <summary>
        /// 返回一个随机地图;
        /// </summary>
        Map<CubicHexCoord, TerrainNode> RandomMap()
        {
            Map<CubicHexCoord, TerrainNode> terrainMap = new Map<CubicHexCoord, TerrainNode>();
            int[] aa = new int[] { 10, 20, 30, 20 };

            terrainMap.Add(CubicHexCoord.Self, new TerrainNode(10, 0));

            foreach (var item in CubicHexCoord.GetHexRange(CubicHexCoord.Self, 10))
            {
                terrainMap.Add(item, new TerrainNode(aa[UnityEngine.Random.Range(0, aa.Length)], UnityEngine.Random.Range(0, 360)));
            }
            return terrainMap;
        }

        void Awake()
        {
            if (ConfirmDirectory(mapDirectory))
            {
                LoadMap();
            }
            else
            {

            }

            //if (terrainMap == null)
            //{
            //    terrainMap = RandomMap();
            //}
        }

        #endregion


    }

}
