using System.Linq;
using System.Collections.Generic;
using JiongXiaGu.Collections;
using System;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace JiongXiaGu.Unity.Resources
{

    /// <summary>
    /// 游戏数据初始化接口;
    /// </summary>
    public interface IResourceLoadHandle
    {
        /// <summary>
        /// 读取到对应内容;
        /// </summary>
        void Read(LoadableContent[] loadableContent, CancellationToken token);
    }

    /// <summary>
    /// 组件资源读取操作;
    /// </summary>
    public class ResourceLoadHandler
    {
        public IResourceLoadHandle[] LoadHandlers { get; set; }
        public bool IsCompleted => worker != null && worker.IsCompleted;
        private Task worker;
        private CancellationTokenSource workerCancellationTokenSource;

        ///// <summary>
        ///// 从文件定义的读取顺序读取到内容;
        ///// </summary>
        //public Task LoadAllAsync(CancellationToken token)
        //{
        //    if (worker != null)
        //        throw new InvalidOperationException("已经进行了读取!");

        //    workerCancellationTokenSource = new CancellationTokenSource();
        //    var currentCancellationTokenSource = CancellationTokenSource.CreateLinkedTokenSource(token, workerCancellationTokenSource.Token);
        //    CancellationToken cancellationToken = currentCancellationTokenSource.Token;

        //    return worker = Task.Run(delegate ()
        //    {
        //        var core = LoadInternal(LoadableResource.Core.Value, cancellationToken);

        //        cancellationToken.ThrowIfCancellationRequested();
        //        var contents = ReadLoadOrderFile();
        //        if (contents != null)
        //        {
        //            var other = LoadInternal(contents, cancellationToken);
        //        }
        //    }, cancellationToken);
        //}

        ///// <summary>
        ///// 按指定顺序重新读取到内容;
        ///// </summary>
        //public void Load(IEnumerable<LoadableContent> contents)
        //{
        //    throw new NotImplementedException();
        //}

        ///// <summary>
        ///// 从文件获取到需要读取的资源,若不存在则返回null;
        ///// </summary>
        //private IEnumerable<LoadableContent> ReadLoadOrderFile()
        //{
        //    throw new NotImplementedException();
        //}

        //private List<KeyValuePair<string, ITypeDictionary>> LoadInternal(IEnumerable<LoadableContent> contents, CancellationToken token)
        //{
        //    token.ThrowIfCancellationRequested();
        //    var datas = new List<KeyValuePair<string, ITypeDictionary>>();
        //    foreach (var content in contents)
        //    {
        //        token.ThrowIfCancellationRequested();
        //        var data = LoadInternal(content, token);
        //        var value = new KeyValuePair<string, ITypeDictionary>(content.ID, data);
        //        datas.Add(value);
        //    }
        //    return datas;
        //}

        //private ITypeDictionary LoadInternal(LoadableContent content, CancellationToken token)
        //{
        //    token.ThrowIfCancellationRequested();
        //    var data = new TypeDictionary();
        //    foreach (var loader in LoadHandlers)
        //    {
        //        token.ThrowIfCancellationRequested();
        //        loader.Read(content, token);
        //    }
        //    return data;
        //}
    }



    /// <summary>
    /// 游戏数据初始化器(在游戏开始前进行初始化,若初始化失败意味着游戏无法开始);
    /// </summary>
    [DisallowMultipleComponent]
    public class ResourceInitializer : MonoBehaviour
    {
        private static readonly GlobalSingleton<ResourceInitializer> singleton = new GlobalSingleton<ResourceInitializer>();
        public static ResourceInitializer Instance => singleton.GetInstance();

        private const string InitializerName = "游戏数据初始化";

    }
}
