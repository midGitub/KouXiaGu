using System;
using UnityEngine;
using KouXiaGu.Resources;
using KouXiaGu.Concurrent;
using System.Collections;

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

        public static IAsyncOperation ComponentInitialize
        {
            get { return component; }
        }

        public static IAsyncOperation<IGameResource> GameDataInitialize
        {
            get { return resource; }
        }

        /// <summary>
        /// 若为初始化完毕则为null;
        /// </summary>
        public static IGameResource GameData { get; private set; }

        public bool IsCanceled { get; private set; }

        void Awake()
        {
            SetInstance(this);
            IsCanceled = false;

            component.InitializeAsync(this);
            resource.InitializeAsync(this);
            StartCoroutine(WaitComponentInitialize());
            StartCoroutine(WaitGameDataInitialize());
        }

        IEnumerator WaitComponentInitialize()
        {
            while (true)
            {
                if (ComponentInitialize.IsCompleted)
                {
                    if (ComponentInitialize.IsFaulted)
                    {
                        Debug.LogError("组件初始化时遇到错误:" + ComponentInitialize.Exception);
                    }
                    yield break;
                }
                yield return null;
            }
        }

        IEnumerator WaitGameDataInitialize()
        {
            while (true)
            {
                if (GameDataInitialize.IsCompleted)
                {
                    if (GameDataInitialize.IsFaulted)
                    {
                        Debug.LogError("资源初始化时遇到错误:" + GameDataInitialize.Exception);
                    }
                    else
                    {
                        GameData = GameDataInitialize.Result;
                    }
                    yield break;
                }
                yield return null;
            }
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            IsCanceled = true;
        }
    }
}
