using System.Collections.Generic;
using JiongXiaGu.Collections;
using System;
using System.Threading;
using System.Threading.Tasks;

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
        void Read(LoadableContent content, ITypeDictionary data, CancellationToken token);
    }

    /// <summary>
    /// 数据整合处理接口;
    /// </summary>
    public interface IResourceIntegrateHandle
    {
        /// <summary>
        /// 设置新的数据数据;
        /// </summary>
        void SetNew(ITypeDictionary[] data);

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
        private ICollection<LoadableContent> LoadOrder { get; set; }
        public IResourceLoadHandle[] LoadHandlers { get; set; }
        public IResourceIntegrateHandle[] IntegrateHandlers { get; set; }

        private ITypeDictionary coreData;
        private List<ContentData> otherData;

        private Task worker;
        private CancellationTokenSource workerCancellationTokenSource;
        public bool IsRunning => worker != null && worker.Status != TaskStatus.RanToCompletion;
        public bool IsCompleted => worker != null && worker.IsCompleted;

        public ResourceLoadHandler()
        {
            otherData = new List<ContentData>();
        }

        /// <summary>
        /// 异步读取所有数据并且应用到;
        /// </summary>
        public Task LoadAsync(CancellationToken token)
        {
            if (LoadOrder == null)
                throw new ArgumentNullException(nameof(LoadOrder));
            if (LoadHandlers == null)
                throw new ArgumentNullException(nameof(LoadHandlers));
            if (IntegrateHandlers == null)
                throw new ArgumentNullException(nameof(IntegrateHandlers));
            token.ThrowIfCancellationRequested();

            if (worker == null)
            {
                CancellationToken cancellationToken = CreateLinkedTokenSource(token);
                return worker = Task.Run(() => LoadInternal(cancellationToken), cancellationToken);
            }
            else
            {
                workerCancellationTokenSource.Cancel();
                CancellationToken cancellationToken = CreateLinkedTokenSource(token);
                return worker = worker.ContinueWith(_ => LoadInternal(token), cancellationToken);
            }
        }

        /// <summary>
        /// 创建新的关联的取消控制器;
        /// </summary>
        private CancellationToken CreateLinkedTokenSource(CancellationToken token)
        {
            workerCancellationTokenSource = new CancellationTokenSource();
            var currentCancellationTokenSource = CancellationTokenSource.CreateLinkedTokenSource(token, workerCancellationTokenSource.Token);
            return currentCancellationTokenSource.Token;
        }

        /// <summary>
        /// 读取所有数据并且应用到;
        /// </summary>
        public void Load()
        {
            if (LoadOrder == null)
                throw new ArgumentNullException(nameof(LoadOrder));
            if (LoadHandlers == null)
                throw new ArgumentNullException(nameof(LoadHandlers));
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
            IntegrateData();
        }

        /// <summary>
        /// 整合数据;
        /// </summary>
        private void IntegrateData()
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
                integrateHandler.SetNew(datas);
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
            foreach (var loader in LoadHandlers)
            {
                token.ThrowIfCancellationRequested();
                loader.Read(content, data, token);
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
