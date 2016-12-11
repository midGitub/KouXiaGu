using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KouXiaGu.HexTerrain
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
        static readonly BlockMapRecord<LandformNode> terrainMap = new BlockMapRecord<LandformNode>(MapBlockSize);

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
        public static IMap<CubicHexCoord, LandformNode> Map
        {
            get { return terrainMap; }
        }

        /// <summary>
        /// 只读的地形地图;
        /// </summary>
        public static IReadOnlyMap<CubicHexCoord, LandformNode> ReadOnlyMap
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
        /// 保存需要保存的内容到文件(同步的);
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
        /// 读取文件夹下的地图(同步的);
        /// </summary>
        public static void Load(string directoryPath)
        {
            ArchiveState currentState = State;
            try
            {
                State = ArchiveState.Reading;
                terrainMap.Load(directoryPath);
                State = currentState;
            }
            finally
            {
                State = currentState;
            }
        }

    }

}
