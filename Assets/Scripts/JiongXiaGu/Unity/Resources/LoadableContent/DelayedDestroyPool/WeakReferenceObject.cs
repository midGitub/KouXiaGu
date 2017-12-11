using System;

namespace JiongXiaGu.Unity.Resources
{

    public abstract class WeakReferenceObject
    {
        /// <summary>
        /// 请求次数;
        /// </summary>
        public uint RequestTimes { get; internal set; }

        /// <summary>
        /// 创建的时间;
        /// </summary>
        public long CreateTime { get; internal set; }

        /// <summary>
        /// 最后请求的时间;
        /// </summary>
        public long LastRequestTime { get; internal set; }

        public WeakReferenceObject()
        {
            RequestTimes = 0;
            CreateTime = DateTime.Now.Ticks;
            LastRequestTime = CreateTime;
        }

        public WeakReferenceObject(WeakReferenceObject obj)
        {
            RequestTimes = obj.RequestTimes;
            CreateTime = obj.CreateTime;
            LastRequestTime = obj.LastRequestTime;
        }

        /// <summary>
        /// 对象引用的对象是否已被垃圾回收的指示;(不增加请求计数)
        /// </summary>
        public abstract bool IsAlive();

        /// <summary>
        /// 尝试获取到引用对象;(增加请求计数)
        /// </summary>
        public abstract bool TryGetObject(out object obj);

        protected void AddRequestTimes()
        {
            RequestTimes++;
        }
    }

    public class WeakReferenceObject<T> : WeakReferenceObject
        where T : class
    {
        private readonly WeakReference<T> weakReference;

        public WeakReferenceObject(T value)
        {
            weakReference = new WeakReference<T>(value);
        }

        public WeakReferenceObject(T value, WeakReferenceObject obj) : base(obj)
        {
            weakReference = new WeakReference<T>(value);
        }

        public override bool IsAlive()
        {
            T value;
            return weakReference.TryGetTarget(out value);
        }

        public bool TryGetTarget(out T value)
        {
            AddRequestTimes();
            return weakReference.TryGetTarget(out value);
        }

        public override bool TryGetObject(out object obj)
        {
            AddRequestTimes();

            T value;
            if (weakReference.TryGetTarget(out value))
            {
                obj = value;
                return true;
            }
            else
            {
                obj = null;
                return false;
            }
        }
    }
}
