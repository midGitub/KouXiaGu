using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace JiongXiaGu.Unity.RectTerrain
{

    /// <summary>
    /// 资源池,对常用资源进行缓存,对不常用的资源定时进行自动销毁;
    /// </summary>
    public abstract class ResPool<T>
        where T : class
    {
        private readonly Dictionary<string, Task<T>> infos;

        public ResPool()
        {
            infos = new Dictionary<string, Task<T>>();
        }

        /// <summary>
        /// 创建资源;
        /// </summary>
        protected abstract Task<T> Create(string key, CancellationToken token);

        /// <summary>
        /// 释放资源;
        /// </summary>
        protected abstract Task Release(T res);

        /// <summary>
        /// 获取到默认的资源;
        /// </summary>
        public abstract T Default();

        /// <summary>
        /// 获取到对应资源,若未加载,则开始加载并返回;
        /// </summary>
        public Task<T> GetOrLoad(string key, CancellationToken token = default(CancellationToken))
        {
            Task<T> info;
            if (infos.TryGetValue(key, out info))
            {
                return info.ContinueWith(task => task.Result, token);
            }
            else
            {
                info = Create(key, token);
                if (!info.IsFaulted)
                {
                    infos.Add(key, info);
                }
                return info;
            }
        }
    }
}
