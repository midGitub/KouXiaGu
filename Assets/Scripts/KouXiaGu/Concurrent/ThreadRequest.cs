using UnityEngine;

namespace KouXiaGu.Concurrent
{

    /// <summary>
    /// 多线程请求;
    /// </summary>
    [DisallowMultipleComponent]
    public sealed class ThreadRequest : UnitySingleton<ThreadRequest>
    {
        private ThreadRequest() { }

        /// <summary>
        /// 每次更新解决的最大请求数;
        /// </summary>
        [SerializeField]
        uint width = 200;

        ConcurrentQueue<IRequest> requestsQueue;

        /// <summary>
        /// 等待中的请求;
        /// </summary>
        [ShowOnlyProperty]
        public int WaitingRequest
        {
            get { return requestsQueue.Count; }
        }

        protected override void Awake()
        {
            base.Awake();
            requestsQueue = new ConcurrentQueue<IRequest>();
        }

        void FixedUpdate()
        {
            IRequest request;
            uint times = this.width;
            while (!requestsQueue.IsEmpty && times-- > uint.MinValue)
            {
                if (requestsQueue.TryDequeue(out request))
                {
                    request.OnQueue = false;
                    request.Execute();
                }
            }
        }

        public void AddQueue(IRequest request)
        {
            requestsQueue.Enqueue(request);
            request.OnQueue = true;
        }

    }

}
