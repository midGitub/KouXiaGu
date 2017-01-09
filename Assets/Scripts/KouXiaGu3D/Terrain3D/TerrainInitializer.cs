using System;
using System.Collections;
using System.Collections.Generic;
using KouXiaGu.Grids;
using KouXiaGu.Initialization;
using UnityEngine;
using KouXiaGu.Collections;

namespace KouXiaGu.Terrain3D
{

    /// <summary>
    /// 地形资源初始化,负责初始化次序;
    /// 控制整个地形初始化;
    /// </summary>
    [DisallowMultipleComponent]
    public sealed class TerrainInitializer : MonoBehaviour
    {

        /// <summary>
        /// 使用的地图;
        /// </summary>
        [Obsolete]
        public static TerrainMapO CurrentMap { get; set; }


        /// <summary>
        /// 当前游戏使用的地图;
        /// </summary>
        public static TerrainMap TerrainMap { get; private set; }

        /// <summary>
        /// 当前游戏使用的地图;
        /// </summary>
        public static IObservableDictionary<CubicHexCoord, TerrainNode> Map
        {
            get { return TerrainMap.Map; }
        }


        public static IEnumerator PreparationStart()
        {
            yield return TerrainRes.Initialize();
        }


        public static IEnumerator GameStart(Archive archive)
        {
            TerrainArchiver.Load(archive.DirectoryPath);
            yield return null;

            TerrainMap.ReadMap();
            yield return null;

            MapArchiver.Initialize(archive, TerrainMap);
            yield return null;

            TerrainCreater.Load();
            yield return null;
        }

        public static IEnumerator GameSave(Archive archive)
        {
            TerrainArchiver.Save(archive.DirectoryPath);
            yield return null;

            MapArchiver.Write(archive);
            yield return null;
        }

    }

}
