//using JiongXiaGu.Unity.Resources;
//using JiongXiaGu.Unity.UI;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading;
//using UnityEngine;
//using System.Threading.Tasks;

//namespace JiongXiaGu.Unity.Initializers
//{

//    public interface IBasicResourceInitializeHandle
//    {
//        void Initialize(IReadOnlyList<ModificationContent> mods, CancellationToken token);
//    }

//    /// <summary>
//    /// 游戏可配置模组化资源初始化;
//    /// </summary>
//    internal class BasicResourceInitializer : InitializerBase
//    {
//        private BasicResourceInitializer()
//        {
//        }

//        private static readonly GlobalSingleton<BasicResourceInitializer> singleton = new GlobalSingleton<BasicResourceInitializer>();
//        public static BasicResourceInitializer Instance => singleton.GetInstance();
//        private IBasicResourceInitializeHandle[] initializeHandlers;

//        private void Awake()
//        {
//            singleton.SetInstance(this);
//            initializeHandlers = GetComponentsInChildren<IBasicResourceInitializeHandle>();
//        }

//        public static Task StartInitialize(IReadOnlyList<ModificationContent> mods, IProgress<ProgressInfo> progress, CancellationToken token)
//        {
//            return Instance.Initialize(mods, progress, token);
//        }

//        protected override List<Func<CancellationToken, string>> EnumerateHandler(object state)
//        {
//            var handlers = new List<Func<CancellationToken, string>>();
//            var mods = state as IReadOnlyList<ModificationContent>;

//            foreach (var handler in initializeHandlers)
//            {
//                handlers.Add(delegate (CancellationToken token)
//                {
//                    token.ThrowIfCancellationRequested();

//                    handler.Initialize(mods, token);
//                    return null;
//                });
//            }

//            return handlers;
//        }
//    }
//}
