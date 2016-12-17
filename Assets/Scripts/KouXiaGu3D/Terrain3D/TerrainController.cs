using System;
using System.Collections;
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
        public static IMap<CubicHexCoord, TerrainNode> ActivatedMap
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
                IEnumerator enumerator;

                TerrainArchive.Load(item);
                yield return null;

                enumerator = GetMapInit();
                while (enumerator.MoveNext())
                    yield return null;

            }

            IEnumerator IStageObserver<ArchiveDirectory>.OnEnterRollBack(ArchiveDirectory item)
            {
                CurrentMap.Clear();
                yield break;
            }

            void IStageObserver<ArchiveDirectory>.OnEnterCompleted()
            {
                Debug.Log("地图读取完毕;" + ActivatedMap.ToLog());
                return;
            }


            IEnumerator GetMapInit()
            {
                if (CurrentMap == null)
                    throw new NullReferenceException("未指定具体地图;");

                return CurrentMap.LoadAsync();
            }



            IEnumerator IStageObserver<ArchiveDirectory>.OnLeave(ArchiveDirectory item)
            {
                CurrentMap.Clear();
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
                IEnumerator enumerator;

                TerrainArchive.Save(item);
                yield return null;

                enumerator = GetMapSave();
                while (enumerator.MoveNext())
                    yield return null;

                yield break;
            }

            IEnumerator IStageObserver<ArchiveDirectory>.OnEnterRollBack(ArchiveDirectory item)
            {
                yield break;
            }

            void IStageObserver<ArchiveDirectory>.OnEnterCompleted()
            {
                return;
            }


            public IEnumerator GetMapSave()
            {
                if (CurrentMap == null)
                    throw new NullReferenceException("未指定具体地图!");

                return CurrentMap.SaveAsync();
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
