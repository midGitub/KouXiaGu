using System;
using System.Collections.Generic;
using UnityEngine;

namespace KouXiaGu
{

    public abstract class AsyncInitializer : AsyncOperation
    {
        public abstract string Prefix { get; }

        string _prefix
        {
            get { return "------" + Prefix; }
        }

        protected void StartInitialize()
        {
            Debug.Log(_prefix + "  开始初始化;");
        }

        protected void OnCompleted(IList<IAsyncOperation> operations)
        {
            OnCompleted();
            Debug.Log(_prefix + "  所有内容初始化完毕;");
        }

        protected void OnFaulted(IList<IAsyncOperation> operations)
        {
            AggregateException ex = operations.ToAggregateException();
            OnFaulted(ex);
            Debug.LogError(_prefix + "  初始化失败;");
        }

        protected void OnFaulted(IAsyncOperation operation)
        {
            Debug.LogError(_prefix + "  初始化时遇到错误:" + operation.Exception);
        }

    }

    public abstract class AsyncInitializer<T> : AsyncOperation<T>
    {
        public abstract string Prefix { get; }

        string _prefix
        {
            get { return "------" + Prefix; }
        }

        public void StartInitialize()
        {
            Debug.Log(_prefix + "  开始初始化;");
        }

        protected void OnCompleted(IList<IAsyncOperation> operations, T result)
        {
            OnCompleted(result);
            Debug.Log(_prefix + "  所以内容初始化完毕;");
        }

        protected void OnFaulted(IList<IAsyncOperation> operations)
        {
            AggregateException ex = operations.ToAggregateException();
            OnFaulted(ex);
            Debug.LogError(_prefix + "  初始化失败;");
        }

        protected void OnFaulted(IAsyncOperation operation)
        {
            Debug.LogError(_prefix + "  初始化时遇到错误:" + operation.Exception);
        }
    }

}
