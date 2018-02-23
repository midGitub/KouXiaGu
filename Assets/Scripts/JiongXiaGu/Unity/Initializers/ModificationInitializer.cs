using JiongXiaGu.Unity.Initializers;
using JiongXiaGu.Unity.Resources;
using JiongXiaGu.Unity.UI;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace JiongXiaGu.Unity.Initializers
{

    /// <summary>
    /// 数据整合处理接口;
    /// </summary>
    public interface IModificationInitializeHandle
    {
        void Initialize(IReadOnlyList<Modification> mods, CancellationToken token);
    }

    /// <summary>
    /// 游戏数据初始化器(在游戏开始前进行初始化,若初始化失败意味着游戏无法开始);
    /// </summary>
    [DisallowMultipleComponent]
    internal class ModificationInitializer : InitializerBase
    {
        private ModificationInitializer()
        {
        }

        private static readonly GlobalSingleton<ModificationInitializer> singleton = new GlobalSingleton<ModificationInitializer>();
        public static ModificationInitializer Instance => singleton.GetInstance();
        private IModificationInitializeHandle[] integrateHandlers;

        private void Awake()
        {
            singleton.SetInstance(this);
            integrateHandlers = GetComponentsInChildren<IModificationInitializeHandle>();
        }

        public static Task StartInitialize(IReadOnlyList<Modification> mods, IProgress<ProgressInfo> progress, CancellationToken token)
        {
            return Instance.Initialize(mods, progress, token);
        }

        protected override List<Func<CancellationToken, string>> EnumerateHandler(object state)
        {
            var handlers = new List<Func<CancellationToken, string>>();
            var mods = state as IReadOnlyList<Modification>;

            foreach (var handler in integrateHandlers)
            {
                handlers.Add(delegate (CancellationToken token)
                {
                    token.ThrowIfCancellationRequested();

                    handler.Initialize(mods, token);
                    return null;
                });
            }

            return handlers;
        }
    }
}
