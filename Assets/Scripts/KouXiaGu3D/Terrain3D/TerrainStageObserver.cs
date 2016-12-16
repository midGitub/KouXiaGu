using System;
using System.Collections;
using KouXiaGu.Initialization;
using UnityEngine;

namespace KouXiaGu.Terrain3D
{

    /// <summary>
    /// 地形资源初始化,负责初始化次序;
    /// </summary>
    [DisallowMultipleComponent]
    public sealed class TerrainStageObserver : MonoBehaviour
    {

        /// <summary>
        /// 是否为允许编辑状态?若为 true 则在保存游戏时,对地图进行保存;
        /// </summary>
        public static bool EditMode { get; private set; }

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
        class GameStageInit : IStageObserver<ArchiveFile>
        {
            GameStageInit() { }
            public static readonly GameStageInit instance = new GameStageInit();

            IEnumerator IStageObserver<ArchiveFile>.OnEnter(ArchiveFile item)
            {
                yield break;
            }

            IEnumerator IStageObserver<ArchiveFile>.OnEnterRollBack(ArchiveFile item)
            {
                yield break;
            }

            void IStageObserver<ArchiveFile>.OnEnterCompleted()
            {
                return;
            }



            IEnumerator IStageObserver<ArchiveFile>.OnLeave(ArchiveFile item)
            {
                yield break;
            }

            IEnumerator IStageObserver<ArchiveFile>.OnLeaveRollBack(ArchiveFile item)
            {
                yield break;
            }

            void IStageObserver<ArchiveFile>.OnLeaveCompleted()
            {
                return;
            }

        }

        /// <summary>
        /// 存档时调用;
        /// </summary>
        class ArchiveStageInit : IStageObserver<ArchiveFile>
        {
            ArchiveStageInit() { }
            public static readonly ArchiveStageInit instance = new ArchiveStageInit();

            IEnumerator IStageObserver<ArchiveFile>.OnEnter(ArchiveFile item)
            {
                yield break;
            }

            IEnumerator IStageObserver<ArchiveFile>.OnEnterRollBack(ArchiveFile item)
            {
                yield break;
            }

            void IStageObserver<ArchiveFile>.OnEnterCompleted()
            {
                return;
            }



            IEnumerator IStageObserver<ArchiveFile>.OnLeave(ArchiveFile item)
            {
                yield break;
            }

            IEnumerator IStageObserver<ArchiveFile>.OnLeaveRollBack(ArchiveFile item)
            {
                yield break;
            }

            void IStageObserver<ArchiveFile>.OnLeaveCompleted()
            {
                return;
            }
        }

    }

}
