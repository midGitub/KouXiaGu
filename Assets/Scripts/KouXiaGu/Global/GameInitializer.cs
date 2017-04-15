using System;
using System.Collections.Generic;
using System.Linq;
using KouXiaGu.Globalization;
using KouXiaGu.Terrain3D;
using KouXiaGu.World;
using UnityEngine;

namespace KouXiaGu
{

    /// <summary>
    /// 负责对游戏资源初始化;
    /// </summary>
    [DisallowMultipleComponent]
    public class GameInitializer : OperationMonoBehaviour
    {
        GameInitializer()
        {
        }

        IAsyncOperation[] missions;
        public GameData Data { get; private set; }

        /// <summary>
        /// 对内容进行初始化;
        /// </summary>
        protected override void Awake()
        {
            base.Awake();
            ResourcePath.Initialize();
            Initialize();
        }

        void Initialize()
        {
            missions = new IAsyncOperation[]
                {
                    CustomInput.ReadAsync().Subscribe(OnCustomInputCompleted, OnFaulted),
                    Localization.InitializeAsync().Subscribe(OnLocalizationCompleted, OnFaulted),
                    GameData.CreateAsync().Subscribe(OnGameDataCompleted, OnFaulted),
                };
            (missions as IEnumerable<IAsyncOperation>).Subscribe(OnCompleted, OnFaulted);
        }

        void OnCompleted(IList<IAsyncOperation> operations)
        {
            OnCompleted();
            Debug.Log("游戏资源初始化完毕;");
        }

        void OnFaulted(IList<IAsyncOperation> operations)
        {
            AggregateException ex = operations.ToAggregateException();
            OnFaulted(ex);
        }

        void OnFaulted(IAsyncOperation operation)
        {
            Debug.LogError("游戏初始化时遇到错误:" + operation.Exception);
        }

        void OnCustomInputCompleted(IAsyncOperation operation)
        {
            const string prefix = "[输入映射]";
            var emptyKeys = CustomInput.GetEmptyKeys().ToList();
            if (emptyKeys.Count != 0)
            {
                Debug.LogWarning(prefix + "存在未定义的按键:" + emptyKeys.ToLog());
            }
            else
            {
                Debug.Log(prefix + "按键读取完毕;");
            }
        }

        void OnLocalizationCompleted(IAsyncOperation operation)
        {
            const string prefix = "[本地化]";
            string log = "条目总数:" + Localization.EntriesCount;
            Debug.Log(prefix + log);
        }

        void OnGameDataCompleted(IAsyncOperation<GameData> operation)
        {
            Data = operation.Result;
            const string prefix = "[游戏资源]\n";
            string log = GetGameDateLog(Data);
            Debug.Log(prefix + log);
        }

        string GetGameDateLog(GameData data)
        {
            string log =
                GetWorldElementResourceLog(Data.ElementInfo) +
                GetTerrainResourceLog(Data.Terrain);
            return log;
        }

        string GetWorldElementResourceLog(WorldElementResource item)
        {
            string str = "[1.基础资源]" +
               "\nRoad:" + item.RoadInfos.Count +
               "\nLandform:" + item.LandformInfos.Count +
               "\nBuilding:" + item.BuildingInfos.Count +
               "\nProduct:" + item.ProductInfos.Count +
               "\n";
            return str;
        }

        string GetTerrainResourceLog(TerrainResource item)
        {
            string str = "[地形资源]" +
               "\nLandform:" + item.LandformInfos.Count +
               "\n";
            return str;
        }

    }

}
