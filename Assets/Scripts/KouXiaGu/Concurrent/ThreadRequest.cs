using UnityEngine;

namespace KouXiaGu.Concurrent
{

    /// <summary>
    /// 多线程请求;
    /// </summary>
    [DisallowMultipleComponent]
    public class ThreadRequest : MonoBehaviour
    {
        private ThreadRequest() { }

        /// <summary>
        /// 每次更新解决的最大请求数;
        /// </summary>
        [SerializeField]
        uint width;

        ConcurrentQueue<IRequest> requestsQueue;

        void Awake()
        {
            requestsQueue = new ConcurrentQueue<IRequest>();
        }

        void FixedUpdate()
        {
            IRequest request;
            while (!requestsQueue.IsEmpty && width-- > uint.MinValue)
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
