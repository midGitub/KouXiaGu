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
        /// 当前游戏使用的地图;
        /// </summary>
        public static TerrainMap Map { get; private set; }

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
        void IRecoveryOperate.Initialize(Archive archive)
        {
            ResetState();
            StartCoroutine(Begin(archive));
        }

        /// <summary>
        /// 保存状态为存档;
        /// </summary>
        void IArchiveOperate.SaveState(Archive archive)
        {
            ResetState();
            StartCoroutine(SaveState(archive));
        }

        /// <summary>
        /// 使用类信息初始化;
        /// </summary>
        public static IEnumerator Begin()
        {
            yield return ResInitializer.Initialize();

            Map = MapFiler.Read();

            MapArchiver.Initialize(TerrainInitializer.Map);

            TerrainCreater.Load();

            yield break;
        }

        /// <summary>
        /// 使用存档初始化;
        /// </summary>
        public static IEnumerator Begin(Archive archive)
        {
            yield return ResInitializer.Initialize();

            Map = MapFiler.Read();

            MapArchiver.Initialize(archive, Map);

            TerrainCreater.Load();

            yield break;
        }

        /// <summary>
        /// 保存游戏内容;
        /// </summary>
        public static IEnumerator SaveState(Archive archive)
        {
            MapArchiver.Write(archive);
            yield return null;
        }

    }

}
