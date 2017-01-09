using System;
using System.Collections;
using System.Collections.Generic;
using KouXiaGu.Grids;
using KouXiaGu.Initialization;
using UnityEngine;

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
        public static TerrainMap CurrentMap { get; set; }

        /// <summary>
        /// 当前游戏使用的地图;
        /// </summary>
        public static IDictionary<CubicHexCoord, TerrainNode> ActivatedMap
        {
            get { return CurrentMap.Map; }
        }


        public static IEnumerator PreparationStart()
        {
            yield return TerrainRes.Initialize();
        }


        public static IEnumerator GameStart(Archive archive)
        {
            TerrainArchiver.Load(archive.DirectoryPath);
            yield return null;

            CurrentMap.Load();
            yield return null;

            MapArchiver.LoadMap(archive.DirectoryPath);
            yield return null;

            MapArchiver.Subscribe(CurrentMap);
            yield return null;

            TerrainCreater.Load();
            yield return null;
        }

        public static IEnumerator GameSave(Archive archive)
        {
            TerrainArchiver.Save(archive.DirectoryPath);
            yield return null;

            MapArchiver.SaveMap(archive.DirectoryPath);
            yield return null;
        }

    }

}
