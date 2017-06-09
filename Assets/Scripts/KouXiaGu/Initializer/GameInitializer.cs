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
    public class GameInitializer : UnitySington<GameInitializer>, ISign
    {
        GameInitializer()
        {
        }

        ComponentInitializer component;
        BasicResourceInitializer resource;

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

            component = new ComponentInitializer();
            component.Initialize();

            resource = new BasicResourceInitializer();
            resource.InitializeAsync(this);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            IsCanceled = true;
        }
    }
}
