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
        [Obsolete]
        internal static UnityEngine.Coroutine _StartCoroutine(IEnumerator routine)
        {
            return Instance.StartCoroutine(routine);
        }

        GameInitializer()
        {
        }

        ComponentInitializer componentInitialize = new ComponentInitializer();
        [SerializeField]
        GameDataInitializer gameDataInitialize;

        public IAsyncOperation ComponentInitialize
        {
            get { return componentInitialize; }
        }

        public IAsyncOperation<IGameData> GameDataInitialize
        {
            get { return gameDataInitialize; }
        }

        public IGameData GameData
        {
            get { return GameDataInitialize.Result; }
        }

        /// <summary>
        /// 对内容进行初始化;
        /// </summary>
        void Awake()
        {
            SetInstance(this);
            ResourcePath.Initialize();
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
