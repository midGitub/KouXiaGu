using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
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
    public sealed class TerrainController : MonoBehaviour
    {

        const string MAP_ARCHIVED_DIRECTORY_NAME = "Maps";

        /// <summary>
        /// 存档的地图数据文件;
        /// </summary>
        const string MAP_ARCHIVED_FILE_NAME = "TerrainMap.MAPP";

        /// <summary>
        /// 使用的地图;
        /// </summary>
        public static TerrainMap CurrentMap { get; set; }

        /// <summary>
        /// 当前游戏使用的地图;
        /// </summary>
        static IDictionary<CubicHexCoord, TerrainNode> ActivatedMap
        {
            get { return CurrentMap.Map; }
        }

        /// <summary>
        /// 对地图进行存档;
        /// </summary>
        static void SaveArchiveMap(string archiveDirectory)
        {
            archiveDirectory = Path.Combine(archiveDirectory, MAP_ARCHIVED_DIRECTORY_NAME);
            string filePath = Path.Combine(archiveDirectory, MAP_ARCHIVED_FILE_NAME);

            if (!Directory.Exists(archiveDirectory))
                Directory.CreateDirectory(archiveDirectory);

            CurrentMap.SaveArchive(filePath);
        }

        /// <summary>
        /// 读取地图 或 若已经读取过了,则为重新加载地图;
        /// </summary>
        static void LoadMap(string archiveDirectory)
        {
            string filePath = Path.Combine(archiveDirectory, MAP_ARCHIVED_FILE_NAME);
            CurrentMap.Load(filePath);
        }


        void Awake()
        {
            InitialStage.Subscribe(InitialStageInit.instance);
            GameStage.Subscribe(GameStageInit.instance);
            ArchiveStage.Subscribe(ArchiveStageInit.instance);
        }

        /// <summary>
        /// 在游戏启动阶段初始化,整个游戏流程只初始化一次;
        /// </summary>
        class InitialStageInit : IStageObserver<object>
        {
            InitialStageInit() { }
            public static readonly InitialStageInit instance = new InitialStageInit();

            IEnumerator IStageObserver<object>.OnEnter(object item)
            {
                return Landform.Initialize();
            }

            IEnumerator IStageObserver<object>.OnEnterRollBack(object item)
            {
                return Landform.ClearRes();
            }

            void IStageObserver<object>.OnEnterCompleted()
            {
                return;
            }


            IEnumerator IStageObserver<object>.OnLeave(object item)
            {
                return Landform.ClearRes();
            }

            IEnumerator IStageObserver<object>.OnLeaveRollBack(object item)
            {
                yield break;
            }

            void IStageObserver<object>.OnLeaveCompleted()
            {
                return;
            }

        }

        /// <summary>
        /// 在进入游戏阶段初始化;
        /// </summary>
        class GameStageInit : IStageObserver<ArchiveDirectory>
        {
            GameStageInit() { }
            public static readonly GameStageInit instance = new GameStageInit();

            IEnumerator IStageObserver<ArchiveDirectory>.OnEnter(ArchiveDirectory item)
            {
                TerrainArchive.Load(item);
                yield return null;

                LoadMap(item.DirectoryPath);
                yield return null;
            }

            IEnumerator IStageObserver<ArchiveDirectory>.OnEnterRollBack(ArchiveDirectory item)
            {
                CurrentMap.Unload();
                yield break;
            }

            void IStageObserver<ArchiveDirectory>.OnEnterCompleted()
            {
                Debug.Log("地图读取完毕;" + ActivatedMap.ToCollectionLog());
                return;
            }


            IEnumerator IStageObserver<ArchiveDirectory>.OnLeave(ArchiveDirectory item)
            {
                CurrentMap.Unload();
                yield break;
            }

            IEnumerator IStageObserver<ArchiveDirectory>.OnLeaveRollBack(ArchiveDirectory item)
            {
                yield break;
            }

            void IStageObserver<ArchiveDirectory>.OnLeaveCompleted()
            {
                return;
            }

        }

        /// <summary>
        /// 存档时调用;
        /// </summary>
        class ArchiveStageInit : IStageObserver<ArchiveDirectory>
        {
            ArchiveStageInit() { }
            public static readonly ArchiveStageInit instance = new ArchiveStageInit();

            IEnumerator IStageObserver<ArchiveDirectory>.OnEnter(ArchiveDirectory item)
            {
                TerrainArchive.Save(item);
                yield return null;

                SaveArchiveMap(item.DirectoryPath);
                yield return null;
            }

            IEnumerator IStageObserver<ArchiveDirectory>.OnEnterRollBack(ArchiveDirectory item)
            {
                yield break;
            }

            void IStageObserver<ArchiveDirectory>.OnEnterCompleted()
            {
                return;
            }


            IEnumerator IStageObserver<ArchiveDirectory>.OnLeave(ArchiveDirectory item)
            {
                yield break;
            }

            IEnumerator IStageObserver<ArchiveDirectory>.OnLeaveRollBack(ArchiveDirectory item)
            {
                yield break;
            }

            void IStageObserver<ArchiveDirectory>.OnLeaveCompleted()
            {
                return;
            }
        }

    }

}
