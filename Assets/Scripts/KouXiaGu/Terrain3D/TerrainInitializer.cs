using System.Collections;
using KouXiaGu.Grids;
using KouXiaGu.Initialization;
using UnityEngine;
using KouXiaGu.Collections;
using System;

namespace KouXiaGu.Terrain3D
{

    /// <summary>
    /// 地形资源初始化,负责初始化次序;
    /// 控制整个地形初始化;
    /// </summary>
    [DisallowMultipleComponent]
    public sealed class TerrainInitializer : MonoBehaviour, IStartOperate, IRecoveryOperate, IArchiveOperate
    {
        TerrainInitializer() { }


        public bool IsCompleted { get; private set; }
        public bool IsFaulted { get; private set; }
        public Exception Ex { get; private set; }

        /// <summary>
        /// 当前游戏使用的地图,若不在游戏中则为null;
        /// </summary>
        public static ObservableDictionary<CubicHexCoord, TerrainNode> Map
        {
            get { return MapDataManager.ActiveData.Data; }
        }

        void ResetState()
        {
            IsCompleted = false;
            IsFaulted = false;
            Ex = null;
        }

        /// <summary>
        /// 使用类信息初始化;
        /// </summary>
        void IStartOperate.Initialize()
        {
            ResetState();
            StartCoroutine(Begin());
        }

        /// <summary>
        /// 使用存档初始化;
        /// </summary>
        void IRecoveryOperate.Initialize(ArchiveFile archive)
        {
            ResetState();
            StartCoroutine(Begin(archive));
        }

        /// <summary>
        /// 保存状态为存档;
        /// </summary>
        void IArchiveOperate.SaveState(ArchiveFile archive)
        {
            ResetState();
            StartCoroutine(SaveState(archive));
        }

        /// <summary>
        /// 使用类信息初始化;
        /// </summary>
        IEnumerator Begin()
        {
            yield return ResInitializer.Initialize();

            MapDataManager.Load();

            TerrainBaker.Initialize();
            Architect.Initialize();

            TerrainChangedCreater.Initialize(Map);
            TerrainCreater.AllowCreation = true;

            IsCompleted = true;
            yield break;
        }

        /// <summary>
        /// 使用存档初始化;
        /// </summary>
        IEnumerator Begin(ArchiveFile archive)
        {
            yield return ResInitializer.Initialize();

            MapDataManager.Load(archive);

            TerrainBaker.Initialize();
            Architect.Initialize();

            TerrainChangedCreater.Initialize(Map);
            TerrainCreater.AllowCreation = true;

            IsCompleted = true;
            yield break;
        }

        /// <summary>
        /// 保存游戏内容;
        /// </summary>
        IEnumerator SaveState(ArchiveFile archive)
        {
            MapDataManager.Save(archive);

            IsCompleted = true;
            yield break;
        }

        void OnDestroy()
        {
            MapDataManager.Unload();
        }

    }

}
