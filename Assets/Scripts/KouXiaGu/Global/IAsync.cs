using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace KouXiaGu
{

    public interface IAsync<TResult> : IAsync
    {
        /// <summary>
        /// 返回的结果;
        /// </summary>
        TResult Result { get; }
    }

    public interface IAsync
    {

        /// <summary>
        /// 是否完成?
        /// </summary>
        bool IsCompleted { get; }

        /// <summary>
        /// 是否由于未经处理异常的原因而完成;
        /// </summary>
        bool IsFaulted { get; }

        /// <summary>
        /// 导致提前结束的异常;
        /// </summary>
        Exception Ex { get; }

    }

    public class AsyncOperation<TResult> : IAsync<TResult>
    {
        public bool IsCompleted { get; protected set; }
        public bool IsFaulted { get; protected set; }
        public TResult Result { get; protected set; }
        public Exception Ex { get; protected set; }

        public AsyncOperation()
        {
            IsCompleted = false;
            IsFaulted = false;
            Result = default(TResult);
            Ex = null;
        }

    }

    public class MonoAsyncOperation<TResult> : MonoBehaviour, IAsync<TResult>
    {
        public bool IsCompleted { get; protected set; }
        public bool IsFaulted { get; protected set; }
        public TResult Result { get; protected set; }
        public Exception Ex { get; protected set; }

        protected virtual void Awake()
        {
            IsCompleted = false;
            IsFaulted = false;
            Result = default(TResult);
            Ex = null;
        }

    }

}
