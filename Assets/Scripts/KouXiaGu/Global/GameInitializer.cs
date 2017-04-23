using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using KouXiaGu.Globalization;
using KouXiaGu.KeyInput;
using KouXiaGu.Terrain3D;
using KouXiaGu.World;
using KouXiaGu.World.Map;
using UnityEngine;

namespace KouXiaGu
{

    /// <summary>
    /// 负责对游戏资源初始化;
    /// </summary>
    [DisallowMultipleComponent]
    public class GameInitializer : UnitySington<GameInitializer>
    {
        /// <summary>
        /// 提供初始化使用的协程方法;
        /// </summary>
        internal static Coroutine _StartCoroutine(IEnumerator routine)
        {
            return Instance.StartCoroutine(routine);
        }

        GameInitializer()
        {
        }

        public static IAsyncOperation ComponentInitialize { get; private set; }
        public static IAsyncOperation<DataInitializer> GameDataInitialize { get; private set; }

        public static IGameData GameData
        {
            get { return GameDataInitialize != null && GameDataInitialize.IsCompleted ? GameDataInitialize.Result : null; }
        }

        /// <summary>
        /// 对内容进行初始化;
        /// </summary>
        void Awake()
        {
            SetInstance(this);
            ResourcePath.Initialize();
            ComponentInitialize = ComponentInitializer.InitializeAsync().SubscribeCompleted(OnComponentInitializeCompleted);
        }

        void OnComponentInitializeCompleted(IAsyncOperation operation)
        {
            GameDataInitialize = DataInitializer.CreateAsync();
        }


        //void Initialize()
        //{
        //    IAsyncOperation[] missions = new IAsyncOperation[]
        //        {
        //            CustomInput.ReadOrDefaultAsync().Subscribe(OnCustomInputCompleted, OnFaulted),
        //            Localization.InitializeAsync().Subscribe(OnLocalizationCompleted, OnFaulted),
        //            DataInitializer.CreateAsync().Subscribe(OnGameDataCompleted, OnFaulted),
        //        };
        //    (missions as IEnumerable<IAsyncOperation>).Subscribe(OnCompleted, OnFaulted);
        //}

        //void OnCompleted(IList<IAsyncOperation> operations)
        //{
        //    OnCompleted();
        //    Debug.Log("游戏资源初始化完毕;");
        //}

        //void OnFaulted(IList<IAsyncOperation> operations)
        //{
        //    AggregateException ex = operations.ToAggregateException();
        //    OnFaulted(ex);
        //    Debug.LogError("游戏资源初始化失败;");
        //}

        //void OnFaulted(IAsyncOperation operation)
        //{
        //    Debug.LogError("游戏初始化时遇到错误:\n" + operation.Exception);
        //}

        //void OnCustomInputCompleted(IAsyncOperation operation)
        //{
        //    const string prefix = "[输入映射]";
        //    var emptyKeys = CustomInput.GetEmptyKeys().ToList();
        //    if (emptyKeys.Count != 0)
        //    {
        //        Debug.LogWarning(prefix + "初始化成功;存在未定义的按键:" + emptyKeys.ToLog());
        //    }
        //    else
        //    {
        //        Debug.Log(prefix + "初始化成功;");
        //    }
        //}

        //void OnLocalizationCompleted(IAsyncOperation operation)
        //{
        //    const string prefix = "[本地化]";
        //    string log = "初始化成功;条目总数:" + Localization.EntriesCount;
        //    Debug.Log(prefix + log);
        //}

        //void OnGameDataCompleted(IAsyncOperation<DataInitializer> operation)
        //{
        //    Data = operation.Result;
        //}

        //string GetGameDateLog(DataInitializer data)
        //{
        //    string log =
        //        GetWorldElementResourceLog(Data.ElementInfo) +
        //        GetTerrainResourceLog(Data.Terrain);
        //    return log;
        //}

        //string GetWorldElementResourceLog(WorldElementResource item)
        //{
        //    string str = 
        //        "\n[基础资源]"
        //       + "\nLandform:" + item.LandformInfos.Count
        //       + "\nRoad:" + item.RoadInfos.Count
        //       + "\nBuilding:" + item.BuildingInfos.Count
        //       + "\nProduct:" + item.ProductInfos.Count;
        //    return str;
        //}

        //string GetTerrainResourceLog(TerrainResource item)
        //{
        //    string str =
        //        "\n[地形资源]"
        //       + "\nLandform:" + item.LandformInfos.Count
        //       + "\nRoad:" + item.RoadInfos.Count;
        //    return str;
        //}

        //[ContextMenu("输出异常")]
        //public string DebugError()
        //{
        //    const string prefix = "[游戏初始程序]";
        //    string log;

        //    if (IsFaulted)
        //    {
        //        log = prefix + Exception;
        //    }
        //    else
        //    {
        //        log = prefix + "未出现异常;";
        //    }

        //    Debug.Log(log);
        //    return log;
        //}

    }

}
