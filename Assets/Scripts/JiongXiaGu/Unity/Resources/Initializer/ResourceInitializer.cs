using JiongXiaGu.Unity.UI;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace JiongXiaGu.Unity.Resources
{

    /// <summary>
    /// 游戏数据初始化器(在游戏开始前进行初始化,若初始化失败意味着游戏无法开始);
    /// </summary>
    [DisallowMultipleComponent]
    internal class ResourceInitializer : MonoBehaviour
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

        public static Task Initialize(IProgress<ProgressInfo> progress, CancellationToken token)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 读取核心资源;
        /// </summary>
        public static Task LoadCore()
        {
            //var order = ReadLoadOrderFromFile();
            //return loadHandler.LoadAsync(order, integrateHandlers);
            throw new NotImplementedException();
        }

        /// <summary>
        /// 按照指定顺序读取资源,若已经读取完毕则或正在读取重新读取;
        /// </summary>
        public Task Load(IReadOnlyList<ModificationContent> order)
        {
            return loadHandler.LoadAsync(order, integrateHandlers);
        }

        /// <summary>
        /// 根据读取顺序
        /// </summary>
        public static Task Prepare()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 读取其它资源;
        /// </summary>
        public static Task LoadOther(ModificationOrder order)
        {
            throw new NotImplementedException();
        }


        ///// <summary>
        ///// 读取到定义的资源读取顺序,若不存在则返回null;
        ///// </summary>
        //private static ModificationOrder ReadLoadOrderFromFile()
        //{
        //    LoadOrderDefinitionSerializer definitionSerializer = new LoadOrderDefinitionSerializer();
        //    try
        //    {
        //        var definitions = definitionSerializer.Deserialize(Resource.ConfigContent);
        //        var order = new ModificationOrder(LoadableResource.All, definitions);
        //        return order;
        //    }
        //    catch (FileNotFoundException)
        //    {
        //        return null;
        //    }
        //}
    }
}
