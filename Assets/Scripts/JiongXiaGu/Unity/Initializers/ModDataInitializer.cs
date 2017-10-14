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
        /// 初始化数据资源;
        /// </summary>
        Task Initialize(ModResource mods, CancellationToken token);
    }

    /// <summary>
    /// 模组数据初始化;
    /// </summary>
    internal sealed class ModDataInitializer : InitializerBase<ModDataInitializer>
    {
        private ModSearcher modSearcher;
        private ModOrderReader modOrderReader;

        private ModResource ModResource
        {
            get { return Resource.ModResource; }
            set { Resource.ModResource = value; }
        }

        protected override string InitializerName
        {
            get { return "模组数据初始化"; }
        }

        protected override void Awake()
        {
            base.Awake();
            modSearcher = new ModSearcher();
            modOrderReader = new ModOrderReader();
            StartCoroutine(WaitInitializers(Initialize, GameComponentInitializer.Instance));
        }

        /// <summary>
        /// 使用指定模组信息初始化(仅在Unity线程调用);
        /// </summary>
        private async void Initialize()
        {
            ModResource = await Task.Run(() => ReadModResource());
            IModDataInitializeHandle[] initializers = GetComponentsInChildren<IModDataInitializeHandle>();
            initializeCancellation = new CancellationTokenSource();
            var cancellationToken = initializeCancellation.Token;

            ModResource.IsReadOnly = true;
            Task task = WhenAll(initializers, initializer => initializer.Initialize(ModResource, cancellationToken));
            initializeTask = task.ContinueWith(OnInitializeTaskCompleted);
            ModResource.IsReadOnly = false;
        }

        internal ModResource ReadModResource()
        {
            var modInfos = modSearcher.EnumerateModInfos();
            try
            {
                ModOrder modOrder = modOrderReader.Read();
                return new ModResource(modInfos, modOrder);
            }
            catch (Exception ex)
            {
                Debug.LogWarning(string.Format("读取资源排列顺序文件失败:{0}", ex));
                return new ModResource(modInfos);
            }
        }
    }
}
