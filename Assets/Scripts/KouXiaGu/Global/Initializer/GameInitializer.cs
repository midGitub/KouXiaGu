using System;
using UnityEngine;
using KouXiaGu.Resources;

namespace KouXiaGu
{

    /// <summary>
    /// 负责对游戏资源初始化;
    /// </summary>
    [DisallowMultipleComponent]
    public class GameInitializer : UnitySington<GameInitializer>
    {
        GameInitializer()
        {
        }

        ComponentInitializer componentInitialize = new ComponentInitializer();
        [SerializeField]
        BasicResourceInitializer gameDataInitialize;

        public IAsyncOperation ComponentInitialize
        {
            get { return componentInitialize; }
        }

        public IAsyncOperation<BasicResource> GameDataInitialize
        {
            get { return gameDataInitialize; }
        }

        public BasicResource GameData
        {
            get { return GameDataInitialize.Result; }
        }

        /// <summary>
        /// 对内容进行初始化;
        /// </summary>
        void Awake()
        {
            SetInstance(this);
            Resource.Initialize();
            Initialize(this);
        }

        void Initialize(GameInitializer initializer)
        {
            componentInitialize.Start();
            componentInitialize.SubscribeCompleted(initializer, OnComponentInitializeCompleted);
        }

        void OnComponentInitializeCompleted(IAsyncOperation operation)
        {
            gameDataInitialize.Start();
        }

    }

}
