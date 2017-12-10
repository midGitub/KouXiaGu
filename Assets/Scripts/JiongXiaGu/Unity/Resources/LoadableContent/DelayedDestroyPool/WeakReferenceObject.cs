using System;

namespace JiongXiaGu.Unity.Resources
{

    public abstract class WeakReferenceObject
    {
        /// <summary>
        /// 该资源的请求次数;
        /// </summary>
        internal uint RequestTimes;

        /// <summary>
        /// 最后请求到资源的时间;
        /// </summary>
        internal ulong LastRequestTime;
    }

    public class WeakReferenceObject<T> : WeakReferenceObject
        where T : class
    {
        public WeakReference<T> Reference { get; private set; }

        public WeakReferenceObject(T value)
        {
            Reference = new WeakReference<T>(value);
        }
    }
}
