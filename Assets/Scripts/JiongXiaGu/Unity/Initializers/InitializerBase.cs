using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace JiongXiaGu.Unity.Initializers
{

    /// <summary>
    /// 抽象类 内容初始化器;
    /// </summary>
    public abstract class InitializerBase : MonoBehaviour
    {
        /// <summary>
        /// 当初始化完成时,调用这些初始化程序进行初始化;
        /// </summary>
        [SerializeField]
        List<InitializerBase> onCompletedCall;

        /// <summary>
        /// 当前初始化异步操作,若还未初始化则为NUll;
        /// </summary>
        internal Task InitializeTask { get; private set; }

        /// <summary>
        /// 取消器,若还未初始化则为NUll;
        /// </summary>
        internal CancellationTokenSource TokenSource { get; private set; }

        /// <summary>
        /// 是否已经初始化完成?
        /// </summary>
        public bool IsCompleted
        {
            get { return InitializeTask != null ? InitializeTask.IsCompleted : false; }
        }

        /// <summary>
        /// 是否在初始化时遇到错误;
        /// </summary>
        public bool IsFaulted
        {
            get { return InitializeTask != null ? InitializeTask.IsFaulted : false; }
        }

        /// <summary>
        /// 若初始化时需要错误则不为Null;
        /// </summary>
        public AggregateException Exception
        {
            get { return InitializeTask != null ? InitializeTask.Exception : null; }
        }

        /// <summary>
        /// 是否正在初始化?
        /// </summary>
        public bool IsRunning
        {
            get { return InitializeTask != null && !InitializeTask.IsCompleted; }
        }

        /// <summary>
        /// 初始化组件名;
        /// </summary>
        protected abstract string InitializerName { get; }

        /// <summary>
        /// 进行初始化,仅由Unity线程调用;
        /// </summary>
        public async Task Initialize()
        {
            XiaGu.ThrowIfNotUnityThread();
            if (InitializeTask != null)
                throw new InvalidOperationException("已经初始化完成,或者正在初始化;");

            try
            {
                TokenSource = new CancellationTokenSource();
                InitializeTask = Initialize_internal(TokenSource.Token);
                await InitializeTask;
                OnCompleted();
            }
            catch (Exception ex)
            {
                OnFaulted(ex);
            }
        }

        /// <summary>
        /// 进行初始化;
        /// </summary>
        protected abstract Task Initialize_internal(CancellationToken cancellationToken);

        /// <summary>
        /// 对所有处置器进行对应操作,并且等待完成;
        /// </summary>
        protected static async Task WhenAll<T>(IEnumerable<T> initializeHandles, Func<T, Task> func, CancellationToken cancellationToken)
        {
            foreach (var initializeHandle in initializeHandles)
            {
                cancellationToken.ThrowIfCancellationRequested();
                Task task = func(initializeHandle);
                if (task != null)
                {
                    await task;
                }
            }
        }

        /// <summary>
        /// 对所有处置器进行对应操作,并且等待完成;
        /// </summary>
        protected static void WaitAll<T>(IEnumerable<T> initializeHandles, Func<T, Task> func, CancellationToken cancellationToken)
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
            var taskArray = tasks.ToArray();
            Task.WaitAll(taskArray, cancellationToken);
        }

        /// <summary>
        /// 取消初始化;
        /// </summary>
        public void Cancel()
        {
            TokenSource?.Cancel();
        }

        /// <summary>
        /// 在初始化完成时调用;
        /// </summary>
        protected virtual void OnCompleted()
        {
            Debug.Log(InitializerName + "完成;");
            foreach (var initializer in onCompletedCall)
            {
                try
                {
                    initializer?.Initialize();
                }
                catch
                {
                    continue;
                }
            }
        }

        /// <summary>
        /// 在失败时调用;
        /// </summary>
        protected virtual void OnFaulted(Exception ex)
        {
            Debug.LogError(InitializerName + "运行时遇到错误:" + ex);
            TokenSource?.Cancel();
        }
    }
}
