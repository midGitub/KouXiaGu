using System.Collections.Generic;
using JiongXiaGu.Collections;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.IO;

namespace JiongXiaGu.Unity.Resources
{

    /// <summary>
    /// 组件资源读取操作;
    /// </summary>
    internal class ResourceLoadHandler
    {
        private ITypeDictionary coreData;
        private List<ContentData> otherData;
        public LoadOrder Order { get; private set; }

        private Task worker;
        private CancellationTokenSource workerCancellationTokenSource;
        public bool IsRunning => worker != null && worker.Status != TaskStatus.RanToCompletion;
        public bool IsCompleted => worker != null && worker.IsCompleted && worker.Status == TaskStatus.RanToCompletion;

        public ResourceLoadHandler()
        {
            otherData = new List<ContentData>();
        }

        /// <summary>
        /// 取消读取;
        /// </summary>
        public void Cancel()
        {
            workerCancellationTokenSource?.Cancel();
        }

        /// <summary>
        /// 异步读取所有数据并且应用到;
        /// </summary>
        public Task LoadAsync(LoadOrder order, IResourceIntegrateHandle[] integrateHandlers)
        {
            if (order != null)
                ThrowIfCollectionhasNull(order);
            if (integrateHandlers == null)
                throw new ArgumentNullException(nameof(integrateHandlers));
            ThrowIfCollectionhasNull(integrateHandlers);

            Order = order;
            if (worker == null)
            {
                workerCancellationTokenSource = new CancellationTokenSource();
                var cancellationToken = workerCancellationTokenSource.Token;
                return worker = Task.Run(() => LoadInternal(order, integrateHandlers, cancellationToken), cancellationToken);
            }
            else
            {
                workerCancellationTokenSource.Cancel();
                workerCancellationTokenSource = new CancellationTokenSource();
                var cancellationToken = workerCancellationTokenSource.Token;
                return worker = worker.ContinueWith(_ => LoadInternal(order, integrateHandlers, cancellationToken), cancellationToken);
            }
        }

        /// <summary>
        /// 读取所有数据;
        /// </summary>
        private void LoadInternal(LoadOrder order, IResourceIntegrateHandle[] integrateHandlers, CancellationToken token)
        {
            token.ThrowIfCancellationRequested();

            //读取主要数据;
            if (coreData == null)
            {
                coreData = InternalLoadData(LoadableResource.Core, integrateHandlers, token);
            }

            if (order != null)
            {
                //读取附加的数据;
                InternalLoadData(order, otherData, integrateHandlers, token);

                //进行整合;
                IntegrateData(order, integrateHandlers, token);
            }
            else
            {
                IntegrateData(coreData, integrateHandlers, token);
            }
        }

        /// <summary>
        /// 仅读取到核心数据;
        /// </summary>
        private void IntegrateData(ITypeDictionary core, IResourceIntegrateHandle[] integrateHandlers, CancellationToken token)
        {
            ITypeDictionary[] datas = new ITypeDictionary[1];
            datas[0] = core;

            foreach (var integrateHandler in integrateHandlers)
            {
                integrateHandler.SetNew(datas, token);
            }
        }

        /// <summary>
        /// 整合数据;
        /// </summary>
        private void IntegrateData(LoadOrder order, IResourceIntegrateHandle[] integrateHandlers, CancellationToken token)
        {
            int i = 1;
            ITypeDictionary[] datas = new ITypeDictionary[order.Count + 1];
            datas[0] = coreData;

            foreach (var content in order)
            {
                var dataIndex = FindIndex(otherData, content);
                if (dataIndex >= 0)
                {
                    var data = otherData[dataIndex];
                    datas[i] = data.Data;
                }
                else
                {
                    throw new ArgumentException();
                }
                i++;
            }

            foreach (var integrateHandler in integrateHandlers)
            {
                integrateHandler.SetNew(datas, token);
            }
        }

        /// <summary>
        /// 读取到数据,若数据已经存在则跳过;
        /// </summary>
        private void InternalLoadData(IEnumerable<LoadableContent> contents, IList<ContentData> datas, IResourceIntegrateHandle[] integrateHandlers, CancellationToken token)
        {
            token.ThrowIfCancellationRequested();

            foreach (var content in contents)
            {
                token.ThrowIfCancellationRequested();
                if (FindIndex(datas, content) >= 0)
                {
                    token.ThrowIfCancellationRequested();
                    var data = InternalLoadData(content, integrateHandlers, token);
                    var value = new ContentData(content, data);
                    datas.Add(value);
                }
            }
        }

        /// <summary>
        /// 读取所有数据,并且返回;
        /// </summary>
        private ITypeDictionary InternalLoadData(LoadableContent content, IResourceIntegrateHandle[] integrateHandlers, CancellationToken token)
        {
            token.ThrowIfCancellationRequested();

            var data = new TypeDictionary();
            foreach (var handler in integrateHandlers)
            {
                token.ThrowIfCancellationRequested();
                handler.Read(content, data, token);
            }
            return data;
        }

        /// <summary>
        /// 若合集或合集内容存在null元素,则返回异常;
        /// </summary>
        private void ThrowIfCollectionhasNull<T>(IEnumerable<T> collection)
            where T : class
        {
            if (collection == null)
                throw new ArgumentNullException(nameof(collection));

            foreach (var item in collection)
            {
                if (item == null)
                    throw new ArgumentNullException("合集其中元素为null;");
            }
        }

        /// <summary>
        /// 获取到对应数据下标;
        /// </summary>
        private int FindIndex(IList<ContentData> datas, LoadableContent content)
        {
            return datas.FindIndex(item => item.Content == content);
        }

        private struct ContentData
        {
            public LoadableContent Content { get; private set; }
            public ITypeDictionary Data { get; private set; }

            public ContentData(LoadableContent content, ITypeDictionary data)
            {
                Content = content;
                Data = data;
            }
        }
    }
}
