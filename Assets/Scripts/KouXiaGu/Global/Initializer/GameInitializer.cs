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
        internal static Coroutine _StartCoroutine(IEnumerator routine)
        {
            return Instance.StartCoroutine(routine);
        }

        GameInitializer()
        {
        }

        static readonly ComponentInitializer componentInitialize = new ComponentInitializer();
        static readonly GameDataInitializer gameDataInitialize = new GameDataInitializer();

        public static IAsyncOperation ComponentInitialize
        {
            get { return componentInitialize; }
        }

        public static IAsyncOperation<IGameData> GameDataInitialize
        {
            get { return gameDataInitialize; }
        }

        public static IGameData GameData
        {
            get { return GameDataInitialize.Result; }
        }

        static void Initialize()
        {
            componentInitialize.Start();
            componentInitialize.SubscribeCompleted(OnComponentInitializeCompleted);
        }

        static void OnComponentInitializeCompleted(IAsyncOperation operation)
        {
            gameDataInitialize.Start();
        }

        /// <summary>
        /// 对内容进行初始化;
        /// </summary>
        void Awake()
        {
            SetInstance(this);
            ResourcePath.Initialize();
            Initialize();
        }
    }

}
