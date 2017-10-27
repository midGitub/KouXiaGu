using JiongXiaGu.Unity.Resources;
using System;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace JiongXiaGu.Unity.Initializers
{

    /// <summary>
    /// 模组数据初始化处置;
    /// </summary>
    public interface IModDataInitializeHandle
    {
        /// <summary>
        /// 初始化数据;
        /// </summary>
        Task Initialize(CancellationToken token);
    }

    /// <summary>
    /// 模组数据初始化;
    /// </summary>
    [DisallowMultipleComponent]
    internal sealed class ModDataInitializer : MonoBehaviour
    {
        private static readonly GlobalSingleton<ModDataInitializer> singleton = new GlobalSingleton<ModDataInitializer>();

        private const string InitializerName = "模组数据初始化";
        private IModDataInitializeHandle[] initializeHandles;
        internal Task InitializeTask { get; private set; }
        internal CancellationTokenSource InitializeCancellation { get; private set; }

        public static ModDataInitializer Instance
        {
            get { return singleton.GetInstance(); }
        }

        private void Awake()
        {
            singleton.SetInstance(this);
            initializeHandles = GetComponentsInChildren<IModDataInitializeHandle>();
            InitializeCancellation = new CancellationTokenSource();
            InitializeTask = new Task(Initialize, InitializeCancellation.Token);
        }

        private void OnDestroy()
        {
            singleton.RemoveInstance(this);
        }

        public void InitializeAsync()
        {
            Task.Run((Action)Initialize);
        }

        public void Initialize()
        {
            try
            {
                EditorHelper.WaitAll(initializeHandles, item => item.Initialize(InitializeCancellation.Token), InitializeCancellation.Token);
                OnCompleted();
            }
            catch (Exception ex)
            {
                OnFaulted(ex);
            }
        }

        private void OnCompleted()
        {
            EditorHelper.LogComplete(InitializerName);
        }

        private void OnFaulted(Exception ex)
        {
            EditorHelper.LogFault(InitializerName, ex);
        }
    }
}
