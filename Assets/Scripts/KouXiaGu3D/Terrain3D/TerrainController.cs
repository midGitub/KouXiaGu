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
                TerrainArchiver.Load(item.DirectoryPath);
                yield return null;

                CurrentMap.Load();
                yield return null;

                MapArchiver.LoadMap(item.DirectoryPath);
                yield return null;

                MapArchiver.Subscribe(CurrentMap);
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
                CurrentMap = null;
                yield return null;

                MapArchiver.UnLoad();
                yield return null;

            }

            IEnumerator IStageObserver<ArchiveDirectory>.OnLeaveRollBack(ArchiveDirectory item)
            {
                yield break;
            }

            void IStageObserver<ArchiveDirectory>.OnLeaveCompleted()
            {
                Debug.Log("清除地形数据;");
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
                TerrainArchiver.Save(item.DirectoryPath);
                yield return null;

                MapArchiver.SaveMap(item.DirectoryPath);
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
                throw new NotImplementedException();
            }

            IEnumerator IStageObserver<ArchiveDirectory>.OnLeaveRollBack(ArchiveDirectory item)
            {
                throw new NotImplementedException();
            }

            void IStageObserver<ArchiveDirectory>.OnLeaveCompleted()
            {
                throw new NotImplementedException();
            }
        }

    }

}
