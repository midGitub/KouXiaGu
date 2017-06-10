using System;
using UnityEngine;
using KouXiaGu.Resources;
using KouXiaGu.Concurrent;

namespace KouXiaGu
{

    /// <summary>
    /// 负责对游戏资源初始化;
    /// </summary>
    [DisallowMultipleComponent]
    public class GameInitializer : UnitySington<GameInitializer>, IOperationState
    {
        GameInitializer()
        {
        }

        static ComponentInitializer component = new ComponentInitializer();
        static GameResourceInitializer resource = new GameResourceInitializer();

        public IAsyncOperation ComponentInitialize
        {
            get { return component; }
        }

        public IAsyncOperation<IGameResource> GameDataInitialize
        {
            get { return resource; }
        }

        public bool IsCanceled { get; private set; }

        void Awake()
        {
            SetInstance(this);
            IsCanceled = false;

            component.InitializeAsync(this);
            resource.InitializeAsync(this);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            IsCanceled = true;
        }
    }
}
