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
    public class ComponentInitializer : AsyncInitializer<IWorldComponent>, IWorldComponent
    {

        IWorldData worldData;
        IObservable<IWorld> starter;
        public Landform Landform { get; private set; }

        public override string Prefix
        {
            get { return "游戏世界组件"; }
        }

        public IAsyncOperation<IWorldComponent> Start(IWorldData worldData, IObservable<IWorld> starter)
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
              };
            (missions as IEnumerable<IAsyncOperation>).Subscribe(this, OnBuildingSceneCompleted, OnFaulted);
        }

        void OnBuildingSceneCompleted(IList<IAsyncOperation> operations)
        {
            OnLandformCompleted();
            OnCompleted(operations, this);
        }

        void OnLandformCompleted()
        {
            const string prefix = "[地形]";
            Landform = SceneObject.GetObject<Landform>().Initialize(worldData);
            Debug.Log(prefix + InitializationCompletedStr);
        }
    }

}
