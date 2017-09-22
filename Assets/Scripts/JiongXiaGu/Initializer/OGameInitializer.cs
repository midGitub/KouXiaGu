using System;
using UnityEngine;
using JiongXiaGu.Resources;
using JiongXiaGu.Concurrent;
using System.Collections;

namespace JiongXiaGu
{

    /// <summary>
    /// 负责对游戏资源初始化;
    /// </summary>
    [DisallowMultipleComponent]
    public class OGameInitializer : UnitySington<OGameInitializer>, IOperationState
    {
        OGameInitializer()
        {
        }

        static GameResourceInitializer resource = new GameResourceInitializer();

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

            resource.InitializeAsync(this);
            StartCoroutine(WaitGameDataInitialize());
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
