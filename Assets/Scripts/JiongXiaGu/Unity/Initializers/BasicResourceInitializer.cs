using JiongXiaGu.Unity.Resources;
using JiongXiaGu.Unity.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using UnityEngine;
using System.Threading.Tasks;

namespace JiongXiaGu.Unity.Initializers
{

    public interface IBasicResourceInitializeHandle
    {
        void Initialize(ILoadOrder order, CancellationToken token);
    }

    /// <summary>
    /// 游戏可配置模组化资源初始化;
    /// </summary>
    internal class BasicResourceInitializer : MonoBehaviour
    {
        private BasicResourceInitializer()
        {
        }

        private IBasicResourceInitializeHandle[] initializeHandlers;
        private static readonly GlobalSingleton<BasicResourceInitializer> singleton = new GlobalSingleton<BasicResourceInitializer>();

        private void Awake()
        {
            singleton.SetInstance(this);
            initializeHandlers = GetComponentsInChildren<IBasicResourceInitializeHandle>();
        }

        private void OnDestroy()
        {
            singleton.RemoveInstance(this);
        }

        public static Task Initialize(ILoadOrder order, IProgress<ProgressInfo> progress, CancellationToken token)
        {
            return singleton.GetInstance().InternalInitialize(order, progress, token);
        }

        private Task InternalInitialize(ILoadOrder order, IProgress<ProgressInfo> progress, CancellationToken token)
        {
            token.ThrowIfCancellationRequested();

            return Task.Run(delegate ()
            {
                token.ThrowIfCancellationRequested();

                float progressValue = 0;
                float progressIncrement = 1 / initializeHandlers.Length;
                foreach (var initializeHandler in initializeHandlers)
                {
                    token.ThrowIfCancellationRequested();
                    initializeHandler.Initialize(order, token);
                    progressValue += progressIncrement;
                    progress.Report(new ProgressInfo(progressValue));
                }
            }, token);
        }
    }
}
