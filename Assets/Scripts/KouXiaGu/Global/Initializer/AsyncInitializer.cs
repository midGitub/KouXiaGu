﻿using System;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

namespace KouXiaGu
{

    public abstract class AsyncInitializer : AsyncOperation
    {
        protected const string InitializationCompletedStr = "初始化完毕;";

        public abstract string Prefix { get; }

        string _prefix
        {
            get { return "------" + Prefix; }
        }

        protected void StartInitialize()
        {
            Debug.Log(_prefix + "  开始初始化;");
        }

        protected override void OnCompleted()
        {
            base.OnCompleted();
            Debug.Log(_prefix + "  所有内容初始化完毕;");
        }

        protected void OnCompleted(IList<IAsyncOperation> operations)
        {
            OnCompleted();
        }

        protected override void OnFaulted(Exception ex)
        {
            base.OnFaulted(ex);
            Debug.LogError(_prefix + "  初始化失败;" + ex);
        }

        protected void OnFaulted(IList<IAsyncOperation> operations)
        {
            AggregateException ex = operations.ToAggregateException();
            OnFaulted(ex);
        }

        protected void OnFaulted(IAsyncOperation operation)
        {
            Debug.LogError(_prefix + "  初始化时遇到错误:" + operation.Exception);
        }
    }


    public abstract class AsyncInitializer<T> : AsyncOperation<T>
    {
        protected const string InitializationCompletedStr = "初始化完毕;";

        public abstract string Prefix { get; }

        string _prefix
        {
            get { return "------" + Prefix; }
        }

        protected void StartInitialize()
        {
            Debug.Log(_prefix + "  开始初始化;");
        }

        protected override void OnCompleted()
        {
            base.OnCompleted();
            Debug.Log(_prefix + "  所有内容初始化完毕;");
        }

        protected void OnCompleted(IList<IAsyncOperation> operations, T result)
        {
            OnCompleted(result);
        }

        protected override void OnFaulted(Exception ex)
        {
            base.OnFaulted(ex);
            Debug.LogError(_prefix + "  初始化失败;" + ex);
        }

        protected void OnFaulted(IList<IAsyncOperation> operations)
        {
            AggregateException ex = operations.ToAggregateException();
            OnFaulted(ex);
        }

        protected void OnFaulted(IAsyncOperation operation)
        {
            Debug.LogError(_prefix + "  初始化时遇到错误:" + operation.Exception);
        }
    }

}