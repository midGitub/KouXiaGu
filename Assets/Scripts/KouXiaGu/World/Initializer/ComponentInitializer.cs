using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KouXiaGu.Terrain3D;
using UniRx;
using UnityEngine;

namespace KouXiaGu.World
{

    /// <summary>
    /// 场景功能组件初始化;
    /// </summary>
    public class ComponentInitializer : AsyncInitializer<IWorldScene>, IWorldScene
    {

        IWorldData worldData;
        IObservable<IWorld> starter;
        public Landform Landform { get; private set; }

        public override string Prefix
        {
            get { return "等待游戏世界组件初始化"; }
        }

        public IAsyncOperation<IWorldScene> Start(IWorldData worldData, IObservable<IWorld> starter)
        {
            this.worldData = worldData;
            this.starter = starter;

            StartInitialize();
            BuildingScene(worldData);
            return this;
        }

        /// <summary>
        /// 初始化游戏场景;
        /// </summary>
        void BuildingScene(IWorldData worldData)
        {
            IAsyncOperation[] missions = new IAsyncOperation[]
              {
                  Landform.InitializeAsync(worldData).Subscribe("等待地形组件初始化", OnLandformCompleted, OnFaulted),
              };
            (missions as IEnumerable<IAsyncOperation>).Subscribe("等待场景组件初始化", OnBuildingSceneCompleted, OnFaulted);
        }

        void OnBuildingSceneCompleted(IList<IAsyncOperation> operations)
        {
            OnCompleted(operations, this);
        }

        void OnLandformCompleted(IAsyncOperation<Landform> operations)
        {
            const string prefix = "[地形]";
            Landform = operations.Result;
            Debug.Log(prefix + InitializationCompletedStr);
        }
    }

}
