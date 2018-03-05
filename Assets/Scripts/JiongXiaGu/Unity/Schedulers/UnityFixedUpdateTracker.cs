using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JiongXiaGu.Unity.Schedulers
{
    /// <summary>
    /// 早于 FixedUpdate() 并行执行的更新;
    /// </summary>
    public interface IBeforeFixedUpdateObserver
    {
        void BeforeFixedUpdate();
    }

    /// <summary>
    /// 晚于 FixedUpdate() 并行执行的更新;
    /// </summary>
    public interface IAfterFixedUpdateObserver
    {
        void AfterFixedUpdate();
    }

    /// <summary>
    /// 可设置在 Unity FixedUpdate() 之前和之后更新;
    /// </summary>
    public class UnityFixedUpdateTracker
    {
        /// <summary>
        /// 更新次数;
        /// </summary>
        public ulong UpdateCount { get; private set; } = 0;
        public bool IsBeforeFixedUpdating { get; private set; } = false;
        public bool IsFixedUpdating { get; private set; } = false;
        public bool IsAfterFixedUpdating { get; private set; } = false;
        public bool IsUpdating => IsBeforeFixedUpdating || IsFixedUpdating || IsAfterFixedUpdating;

        public IDisposable Subscribe(IBeforeFixedUpdateObserver observer)
        {
            throw new NotImplementedException();
        }

        public IDisposable Subscribe(IAfterFixedUpdateObserver observer)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 首次更新;
        /// </summary>
        public void SendBeforeFixedUpdate()
        {
            UnityThread.ThrowIfNotUnityThread();

            throw new NotImplementedException();
        }

        /// <summary>
        /// 最后更新;
        /// </summary>
        public void SendAfterFixedUpdate()
        {
            UnityThread.ThrowIfNotUnityThread();

            throw new NotImplementedException();
        }
    }
}
