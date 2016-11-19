using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KouXiaGu
{

    /// <summary>
    /// 非线程安全的对象池;
    /// </summary>
    public interface IObjectPool<TKey, TValue>
    {
        /// <summary>
        /// 尝试获取到保存在对象池的实例;若加入的值为 Null 则返回false;
        /// </summary>
        bool TryGetInstance(TKey key, out TValue instance);

        /// <summary>
        /// 尝试将实例保存到对象池;
        /// </summary>
        bool TryKeepInstance(TKey key, TValue instance);

        /// <summary>
        /// 清除所有保存的实例引用;
        /// </summary>
        void Clear();
    }

    /// <summary>
    /// 加入对象池物体需要实现的接口;
    /// </summary>
    public interface IPoolObject
    {
        /// <summary>
        /// 保存在池内的最大数目;
        /// </summary>
        uint MaxCountInPool { get; }
    }

}
