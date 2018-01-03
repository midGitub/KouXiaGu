using System;
using System.Linq;
using JiongXiaGu.Collections;
using System.Threading;
using UnityEngine;
using System.Threading.Tasks;

namespace JiongXiaGu.Unity.Resources
{
    /// <summary>
    /// 游戏数据初始化器(在游戏开始前进行初始化,若初始化失败意味着游戏无法开始);
    /// </summary>
    [DisallowMultipleComponent]
    public class ResourceInitializer : MonoBehaviour
    {
        private static readonly GlobalSingleton<ResourceInitializer> singleton = new GlobalSingleton<ResourceInitializer>();
        public static ResourceInitializer Instance => singleton.GetInstance();
        private IResourceIntegrateHandle[] integrateHandlers;
        private ResourceLoadHandler loadHandler;

        private void Awake()
        {
            integrateHandlers = GetComponentsInChildren<IResourceIntegrateHandle>();
            loadHandler = new ResourceLoadHandler();
        }

        /// <summary>
        /// 读取资源,若已经读取完毕则或正在读取则不进行任何操作;
        /// </summary>
        public Task Load()
        {
            //return loadHandler.LoadAsync(integrateHandlers);
            throw new NotImplementedException();
        }

        /// <summary>
        /// 按照指定顺序读取资源,若已经读取完毕则或正在读取重新读取;
        /// </summary>
        public Task Reload(LoadOrder loadOrder)
        {
            throw new NotImplementedException();
        }
    }
}
