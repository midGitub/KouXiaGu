using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KouXiaGu.Terrain3D;
using UniRx;
using UnityEngine;

namespace KouXiaGu.World
{

    public interface IWorldScene
    {
        Landform Landform { get; }
    }

    /// <summary>
    /// 场景功能组件初始化;
    /// </summary>
    public class SceneComponentInitializer : AsyncInitializer<IWorldScene>, IWorldScene
    {
        public Landform Landform { get; private set; }

        public override string Prefix
        {
            get { return "游戏世界场景"; }
        }

        public IAsyncOperation<IWorldScene> Start(IWorldData worldData, IObservable<IWorld> starter)
        {
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
                  Landform.Initialize(worldData).Subscribe(OnLandformCompleted, OnFaulted),
              };
            (missions as IEnumerable<IAsyncOperation>).Subscribe(OnBuildingSceneCompleted, OnFaulted);
        }

        void OnLandformCompleted(IAsyncOperation<Landform> operation)
        {
            const string prefix = "[地形]";
            Landform = operation.Result;
            Debug.Log(prefix + InitializationCompletedStr);
        }

        void OnBuildingSceneCompleted(IList<IAsyncOperation> operations)
        {
            OnCompleted(operations, this);
        }
    }

}
