using System.Collections.Generic;
using JiongXiaGu.Collections;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace JiongXiaGu.Unity.Resources
{
    /// <summary>
    /// 数据整合处理接口;
    /// </summary>
    public interface IResourceIntegrateHandle
    {
        /// <summary>
        /// 读取到对应内容;
        /// </summary>
        void Read(LoadableContent content, ITypeDictionary data, CancellationToken token);

        /// <summary>
        /// 输出对应内容;
        /// </summary>
        void Write(LoadableContent content, ITypeDictionary data, CancellationToken token);

        /// <summary>
        /// 设置新的数据数据;
        /// </summary>
        void SetNew(ITypeDictionary[] data, CancellationToken token);

        /// <summary>
        /// 清空所有数据;
        /// </summary>
        void Clear();
    }

    /// <summary>
    /// 组件资源读取操作;
    /// </summary>
    internal class ResourceLoadHandler
    {
        private ITypeDictionary coreData;
        private List<ContentData> otherData;
        private LoadOrder LoadOrder { get; set; }
        public IResourceIntegrateHandle[] IntegrateHandlers { get; set; }

        private Task worker;
        private CancellationTokenSource workerCancellationTokenSource;
        public bool IsRunning => worker != null && worker.Status != TaskStatus.RanToCompletion;
        public bool IsCompleted => worker != null && worker.IsCompleted;

        public ResourceLoadHandler()
        {
            otherData = new List<ContentData>();
        }

        /// <summary>
        /// 取消读取;
        /// </summary>
        private void Cancel()
        {
            workerCancellationTokenSource?.Cancel();
        }

        /// <summary>
        /// 异步读取所有数据并且应用到;
        /// </summary>
        public Task LoadAsync()
        {
            if (LoadOrder == null)
                throw new ArgumentNullException(nameof(LoadOrder));
            if (IntegrateHandlers == null)
                throw new ArgumentNullException(nameof(IntegrateHandlers));

            if (worker == null)
            {
                workerCancellationTokenSource = new CancellationTokenSource();
                var cancellationToken = workerCancellationTokenSource.Token;
                return worker = Task.Run(() => LoadInternal(cancellationToken), cancellationToken);
            }
            else
            {
                workerCancellationTokenSource.Cancel();
                workerCancellationTokenSource = new CancellationTokenSource();
                var cancellationToken = workerCancellationTokenSource.Token;
                return worker = worker.ContinueWith(_ => LoadInternal(cancellationToken), cancellationToken);
            }
        }

        /// <summary>
        /// 读取所有数据并且应用到;
        /// </summary>
        public void Load()
        {
            if (LoadOrder == null)
                throw new ArgumentNullException(nameof(LoadOrder));
            if (IntegrateHandlers == null)
                throw new ArgumentNullException(nameof(IntegrateHandlers));

            LoadInternal(default(CancellationToken));
        }

        /// <summary>
        /// 读取所有数据;
        /// </summary>
        private void LoadInternal(CancellationToken token)
        {
            token.ThrowIfCancellationRequested();

            //读取主要数据;
            if (coreData == null)
            {
                coreData = LoadDataInternal(LoadableResource.Core.Value, token);
            }

            //读取附加的数据;
            LoadDataInternal(LoadOrder, otherData, token);

            //进行整合;
            IntegrateData(token);
        }

        /// <summary>
        /// 整合数据;
        /// </summary>
        private void IntegrateData(CancellationToken token)
        {
            int i = 0;
            ITypeDictionary[] datas = new ITypeDictionary[LoadOrder.Count];
            foreach (var content in LoadOrder)
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

            foreach (var integrateHandler in IntegrateHandlers)
            {
                integrateHandler.SetNew(datas, token);
            }
        }

        /// <summary>
        /// 读取到数据,若数据已经存在则跳过;
        /// </summary>
        private void LoadDataInternal(IEnumerable<LoadableContent> contents, IList<ContentData> datas, CancellationToken token)
        {
            token.ThrowIfCancellationRequested();
            ThrowIfCollectionhasNull(contents);

            foreach (var content in contents)
            {
                token.ThrowIfCancellationRequested();
                if (FindIndex(datas, content) >= 0)
                {
                    token.ThrowIfCancellationRequested();
                    var data = LoadDataInternal(content, token);
                    var value = new ContentData(content, data);
                    datas.Add(value);
                }
            }
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
        /// 读取所有数据,并且返回;
        /// </summary>
        private ITypeDictionary LoadDataInternal(LoadableContent content, CancellationToken token)
        {
            token.ThrowIfCancellationRequested();

            var data = new TypeDictionary();
            foreach (var handler in IntegrateHandlers)
            {
                token.ThrowIfCancellationRequested();
                handler.Read(content, data, token);
            }
            return data;
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
