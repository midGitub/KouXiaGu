using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KouXiaGu.Grids;

namespace KouXiaGu.Terrain3D
{

    /// <summary>
    /// 地形地图保存和提供;
    /// </summary>
    public static class TerrainMap
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
        /// 地图是否为空?
        /// </summary>
        public static bool IsEmpty
        {
            get { return terrainMap.Count == 0; }
        }



        /// <summary>
        /// 保存需要保存为文件;
        /// </summary>
        public static void Save(string directoryPath)
        {
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
        /// <param name="directoryPath"></param>
        public static void SaveAll(string directoryPath)
        {
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

    }

}
