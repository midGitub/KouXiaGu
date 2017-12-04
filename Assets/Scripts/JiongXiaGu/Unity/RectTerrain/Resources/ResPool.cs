using System.Collections.Generic;
using System.Threading.Tasks;

namespace JiongXiaGu.Unity.RectTerrain
{

    public abstract class ResPool<T>
        where T : class
    {
        private readonly Dictionary<string, Task<T>> infos;

        public ResPool()
        {
            infos = new Dictionary<string, Task<T>>();
        }

        protected abstract Task<T> Create(string key);
        protected abstract void Release(string key, Task<T> info);

        /// <summary>
        /// 获取到对应资源,若未加载,则开始加载并返回;
        /// </summary>
        public Task<T> GetOrLoad(string key)
        {
            Task<T> info;
            if (infos.TryGetValue(key, out info))
            {
                return info;
            }
            else
            {
                info = Create(key);
                if (info != null)
                {
                    infos.Add(key, info);
                }
                return info;
            }
        }
    }

}
