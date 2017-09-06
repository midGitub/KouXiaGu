using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace KouXiaGu.Concurrent
{

    /// <summary>
    /// 在 Unity线程 Update内 处理请求;
    /// </summary>
    public class RequestUnityDispatcher : MonoBehaviour, IRequestDispatcher
    {
        RequestUnityDispatcher()
        {
        }

        [SerializeField]
        Stopwatch runtimeStopwatch = new Stopwatch(0.2f);
        public RequestQueue RequestQueue { get; private set; }

        public int Count
        {
            get { return RequestQueue != null ? RequestQueue.Count : 0; }
        }

        void Awake()
        {
            RequestQueue = new RequestQueue(runtimeStopwatch);
        }

        void Update()
        {
            RequestQueue.MoveNext();
        }

        public IRequest Add(IRequest request)
        {
            return RequestQueue.Add(request);
        }
    }
}
