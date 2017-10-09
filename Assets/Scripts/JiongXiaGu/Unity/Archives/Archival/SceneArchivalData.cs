using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using JiongXiaGu.Collections;

namespace JiongXiaGu.Unity.Archives
{

    /// <summary>
    /// 游戏场景归档数据;
    /// </summary>
    public class SceneArchivalData
    {
        readonly List<IDataArchival> archivalData;

        public SceneArchivalData()
        {
            archivalData = new List<IDataArchival>();
        }

        /// <summary>
        /// 状态合集;
        /// </summary>
        public IEnumerable<IDataArchival> ArchivalData
        {
            get { return archivalData; }
        }

        /// <summary>
        /// 添加状态信息;
        /// </summary>
        public void Add<T>(T item)
            where T : class, IDataArchival
        {
            if (!archivalData.Contains(i => i is T))
            {
                archivalData.Add(item);
            }
        }

        /// <summary>
        /// 移除指定状态信息;
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public bool Remove<T>()
            where T : class, IDataArchival
        {
            return archivalData.Remove(item => item is T);
        }

        /// <summary>
        /// 获取到指定信息,若不存在则返回NULL;
        /// </summary>
        public T Get<T>()
            where T : class, IDataArchival
        {
            return archivalData.Find(item => item is T) as T;
        }
    }
}
