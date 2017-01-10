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
    public sealed class TerrainInitializer : UnitySington<TerrainInitializer>
    {

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

        protected override void Awake()
        {
            base.Awake();
            StartCoroutine(Initialize());

            MapFiler.Initialize();
        }

        public static IEnumerator Initialize()
        {
            yield return TerrainRes.Initialize();
        }


        public static IEnumerator GameStart(Archive archive)
        {
            ArchiveDescription description = ArchiveDescription.Read(archive);
            yield return null;

            TerrainMap = MapFiler.Find(description.UseMapID);
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
            ArchiveDescription description = DescriptionFromGame();
            yield return null;

            ArchiveDescription.Write(archive, description);
            yield return null;

            MapArchiver.Write(archive);
            yield return null;
        }

        public static IEnumerator GameEnd(Archive archive)
        {
            MapArchiver.Clear();
            yield break;
        }


        static ArchiveDescription DescriptionFromGame()
        {
            ArchiveDescription description = new ArchiveDescription()
            {
                UseMapID = TerrainMap.Description.Id,
            };
            return description;
        }

    }

}
