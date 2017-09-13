using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace KouXiaGu.Concurrent
{


    /// <summary>
    /// 请求基类;
    /// </summary>
    public abstract class RequestBase
    {
        public RequestBase()
        {
            IsCompleted = false;
            IsFaulted = false;
            Exception = null;
        }

        public RequestBase(CancellationToken cancellationToken) : this()
        {
            if (cancellationToken == null)
                throw new ArgumentNullException("cancellationToken");

            this.cancellationToken = cancellationToken;
            cancellationToken.Register(OnCancele);
        }

        protected Action<RequestBase> onCompleteEvent;
        public bool IsCompleted { get; protected set; }
        public bool IsFaulted { get; protected set; }
        public bool IsCanceled { get; protected set; }
        public AggregateException Exception { get; protected set; }
        protected CancellationToken cancellationToken { get; private set; }

        /// <summary>
        /// 当请求完成时调用,不管成功还是失败;
        /// </summary>
        public event Action<RequestBase> OnCompleteEvent
        {
            add { onCompleteEvent += value; }
            remove { onCompleteEvent -= value; }
        }

        protected void OnComplete()
        {
            IsCompleted = true;
            onCompleteEvent?.Invoke(this);
        }

        protected void OnFault()
        {
            IsFaulted = true;
            IsCompleted = true;
            onCompleteEvent?.Invoke(this);
        }

        void OnCancele()
        {
            AddException(new OperationCanceledException());
            IsCanceled = true;
            IsCompleted = true;
            onCompleteEvent?.Invoke(this);
        }

        /// <summary>
        /// 添加错误异常;
        /// </summary>
        protected void AddException(Exception ex)
        {
            if (Exception == null)
            {
                Exception = new AggregateException(ex);
            }
            else
            {
                var aggregateException = Exception as AggregateException;
                if (aggregateException == null)
                {
                    Exception = new AggregateException(Exception, ex);
                }
                else
                {
                    var exceptionList = new List<Exception>(Exception.InnerExceptions);
                    exceptionList.Add(ex);
                    Exception = new AggregateException(exceptionList);
                }
            }
        }


        /// <summary>
        /// 等待所有请求处理完成调用;
        /// </summary>
        public static void WaitAll<T>(ICollection<T> requests)
            where T : Request
        {
            if (requests == null)
                throw new ArgumentNullException("requests");

            foreach (var request in requests)
            {
                while (!request.IsCompleted)
                {
                }
            }
        }

        /// <summary>
        /// 等待所有请求处理完成调用,并保存出现异常的请求;
        /// </summary>
        public static void WaitAll<T>(ICollection<T> requests, ICollection<T> faults)
            where T : Request
        {
            if (requests == null)
                throw new ArgumentNullException("requests");
            if (faults == null)
                throw new ArgumentNullException("faults");

            foreach (var request in requests)
            {
                while (!request.IsCompleted)
                {
                }
                if (request.IsFaulted)
                {
                    faults.Add(request);
                }
            }
        }
    }

    /// <summary>
    /// 表示一个操作请求;
    /// </summary>
    public abstract class Request : RequestBase, IRequest
    {
        public Request() : base()
        {
        }

        public Request(CancellationToken cancellationToken) : base(cancellationToken)
        {
        }

        void IRequest.Operate()
        {
            if (!IsCompleted)
            {
                try
                {
                    Operate();
                    OnComplete();
                }
                catch (Exception ex)
                {
                    AddException(ex);
                    OnFault();
                }
            }
        }

        protected abstract void Operate();
    }

    /// <summary>
    /// 表示一个带返回值的请求;
    /// </summary>
    public abstract class Request<T> : RequestBase, IRequest<T>
    {
        public Request() : base()
        {
        }

        public Request(CancellationToken cancellationToken) : base(cancellationToken)
        {
        }

        public T Result { get; protected set; }

        void IRequest.Operate()
        {
            if (!IsCompleted)
            {
                try
                {
                    Result = Operate();
                    OnComplete();
                }
                catch (Exception ex)
                {
                    AddException(ex);
                    OnFault();
                }
            }
        }

        protected abstract T Operate();
    }
}
