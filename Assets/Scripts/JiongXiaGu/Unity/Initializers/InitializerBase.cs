using JiongXiaGu.Concurrent;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace JiongXiaGu.Unity.Initializers
{

    /// <summary>
    /// 抽象类 内容初始化器;
    /// </summary>
    public abstract class InitializerBase<TSelf> : SceneSington<TSelf>, IOperationState
        where TSelf : SceneSington<TSelf>
    {
        /// <summary>
        /// 初始化异步操作;
        /// </summary>
        public Task initializeTask { get; set; }

        /// <summary>
        /// 初始化取消器;
        /// </summary>
        protected CancellationTokenSource initializeCancellation { get; set; }

        /// <summary>
        /// 初始化组件名;
        /// </summary>
        protected virtual string InitializerName
        {
            get { return "内容初始化"; }
        }

        /// <summary>
        /// 是否正在进行初始化?
        /// </summary>
        public bool IsRunning
        {
            get { return initializeTask != null && !initializeTask.IsCompleted; }
        }

        /// <summary>
        /// 是否已经进行了初始化?
        /// </summary>
        public bool IsInitialized
        {
            get { return initializeTask != null; }
        }

        /// <summary>
        /// 是否已经初始化完成?
        /// </summary>
        public bool IsCompleted
        {
            get { return initializeTask != null ? initializeTask.IsCompleted : false; }
        }

        /// <summary>
        /// 是否在初始化时遇到错误;
        /// </summary>
        public bool IsFaulted
        {
            get { return initializeTask != null ? initializeTask.IsFaulted : false; }
        }

        /// <summary>
        /// 若初始化时需要错误则不为Null;
        /// </summary>
        public AggregateException Exception
        {
            get { return initializeTask != null ? initializeTask.Exception : null; }
        }

        public bool IsCanceled
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// 取消初始化;
        /// </summary>
        private void Cancel()
        {
            initializeCancellation?.Cancel();
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            Cancel();
        }

        /// <summary>
        /// 当初始化完成时选择调用 OnCompleted() 或 OnFaulted(Exception);
        /// </summary>
        protected void OnInitializeTaskCompleted(Task task)
        {
            if (task.IsFaulted)
            {
                OnFaulted(task.Exception);
            }
            else
            {
                OnCompleted();
            }
        }

        /// <summary>
        /// 在初始化完成时调用;
        /// </summary>
        protected virtual void OnCompleted()
        {
            Debug.Log(string.Format("[{0}]完成;", InitializerName));
        }

        /// <summary>
        /// 在失败时调用;
        /// </summary>
        protected virtual void OnFaulted(Exception ex)
        {
            Debug.Log(string.Format("[{0}]运行时遇到错误:{1}", InitializerName, ex));
            Cancel();
        }

        /// <summary>
        /// 等待所有任务完成;
        /// </summary>
        protected IEnumerator WaitInitializers(Action onCompleted, params IOperationState[] initializers)
        {
            if (onCompleted == null)
                throw new ArgumentNullException(nameof(onCompleted));

            foreach (var initializer in initializers)
            {
                while (!initializer.IsCompleted)
                {
                    yield return null;
                }
            }
            onCompleted.Invoke();
        }

        /// <summary>
        /// 对所有处置器进行对应操作,并且等待完成;
        /// </summary>
        [Obsolete]
        protected static Task WhenAll<T>(IEnumerable<T> initializeHandles, Func<T, Task> func, CancellationToken cancellationToken)
        {
            List<Task> tasks = new List<Task>();
            foreach (var initializeHandle in initializeHandles)
            {
                Task task = func(initializeHandle);
                if (task != null)
                {
                    tasks.Add(task);
                }
            }
            return Task.WhenAll(tasks);
        }

        /// <summary>
        /// 对所有处置器进行对应操作,并且等待完成;
        /// </summary>
        protected static Task WhenAll<T>(IEnumerable<T> initializeHandles, Func<T, Task> func)
        {
            List<Task> tasks = new List<Task>();
            foreach (var initializeHandle in initializeHandles)
            {
                Task task = func(initializeHandle);
                if (task != null)
                {
                    tasks.Add(task);
                }
            }
            return Task.WhenAll(tasks);
        }

        //public TaskAwaiter GetAwaiter()
        //{
        //    return Task.Run(delegate ()
        //    {
        //        while (!IsCompleted)
        //        {
        //        }
        //    }).GetAwaiter();
        //}
    }
}
