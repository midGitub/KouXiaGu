//using System.Collections;
//using KouXiaGu.Grids;
//using KouXiaGu.Initialization;
//using UnityEngine;
//using KouXiaGu.Collections;
//using System;

//namespace KouXiaGu.Terrain3D
//{


//    /// <summary>
//    /// 地形资源初始化,负责初始化次序;
//    /// 控制整个地形初始化;
//    /// </summary>
//    [DisallowMultipleComponent]
//    public sealed class OTerrainInitializer : MonoBehaviour, IStartOperate, IRecoveryOperate, IArchiveOperate
//    {
//        OTerrainInitializer() { }


//        public bool IsCompleted { get; private set; }
//        public bool IsFaulted { get; private set; }
//        public Exception Exception { get; private set; }

//        /// <summary>
//        /// 当前游戏使用的地图,若不在游戏中则为null;
//        /// </summary>
//        public static ObservableDictionary<CubicHexCoord, TerrainNode> Map
//        {
//            get { return MapDataManager.Data.Data; }
//        }

//        void ResetState()
//        {
//            IsCompleted = false;
//            IsFaulted = false;
//            Exception = null;
//        }

//        /// <summary>
//        /// 使用类信息初始化;
//        /// </summary>
//        Action IStartOperate.Initialize()
//        {
//            ResetState();
//            StartCoroutine(Begin());
//            return null;
//        }

//        /// <summary>
//        /// 使用存档初始化;
//        /// </summary>
//        Action IRecoveryOperate.Initialize(Initialization.ArchiveFile archive)
//        {
//            ResetState();
//            StartCoroutine(Begin(archive));
//            return null;
//        }

//        /// <summary>
//        /// 保存状态为存档;
//        /// </summary>
//        void IArchiveOperate.SaveState(Initialization.ArchiveFile archive)
//        {
//            ResetState();
//            StartCoroutine(SaveState(archive));
//        }

//        /// <summary>
//        /// 使用类信息初始化;
//        /// </summary>
//        IEnumerator Begin()
//        {
//            yield return ResInitializer.Initialize();

//            MapDataManager.Load();
//            TerrainData.Initialize(MapDataManager.Data);

//            TerrainChangedCreater.Initialize(Map);

//            IsCompleted = true;
//            yield break;
//        }

//        /// <summary>
//        /// 使用存档初始化;
//        /// </summary>
//        IEnumerator Begin(Initialization.ArchiveFile archive)
//        {
//            yield return ResInitializer.Initialize();

//            MapDataManager.Load(archive);
//            TerrainData.Initialize(MapDataManager.Data);

//            TerrainChangedCreater.Initialize(Map);

//            IsCompleted = true;
//            yield break;
//        }

//        /// <summary>
//        /// 保存游戏内容;
//        /// </summary>
//        IEnumerator SaveState(Initialization.ArchiveFile archive)
//        {
//            MapDataManager.Save(archive);

//            IsCompleted = true;
//            yield break;
//        }

//        void OnDestroy()
//        {
//            MapDataManager.Unload();
//        }

//    }

//}
