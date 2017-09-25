using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace JiongXiaGu
{

    public abstract class Initializer<T> : MonoBehaviour
    {

        /// <summary>
        /// 当前初始化异步操作,若还未初始化则为NUll;
        /// </summary>
        public Task InitializeTask { get; private set; }

        /// <summary>
        /// 取消器,若还未初始化则为NUll;
        /// </summary>
        public CancellationTokenSource TokenSource { get; private set; }

        /// <summary>
        /// 初始化组件名;
        /// </summary>
        protected abstract string InitializerName { get; }

        public bool IsCompleted
        {
            get { return InitializeTask != null ? InitializeTask.IsCompleted : false; }
        }

        public bool IsFaulted
        {
            get { return InitializeTask != null ? InitializeTask.IsFaulted : false; }
        }

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

        protected virtual void Start()
        {
            StartInitialize();
        }

        /// <summary>
        /// 开始进行异步的初始化;
        /// </summary>
        public Task StartInitialize()
        {
            if (InitializeTask == null)
            {
                InitializeTask = StartInitializeAsync();
            }
            return InitializeTask;
        }

        /// <summary>
        /// 进行初始化;
        /// </summary>
        async Task StartInitializeAsync()
        {
            try
            {
                TokenSource = new CancellationTokenSource();
                var waiter = WaitPrepare(TokenSource.Token);
                if (waiter != null)
                {
                    await waiter;
                }

                OnStart();
                T[] initializers = GetComponentsInChildren<T>();
                List<Task> tasks = new List<Task>();

                for (int i = 0; i < initializers.Length; i++)
                {
                    TokenSource.Token.ThrowIfCancellationRequested();
                    var initializer = initializers[i];
                    Task task = GetTask(initializer);
                    if (task != null)
                    {
                        tasks.Add(task);
                    }
                }

                await Task.WhenAll(tasks);
                OnCompleted();
            }
            catch(Exception ex)
            {
                OnFaulted(ex);
                throw ex;
            }
        }

        /// <summary>
        /// 获取到Task;
        /// </summary>
        protected abstract Task GetTask(T initializer);

        /// <summary>
        /// 进行准备,若为准备好初始化者返回false,准备开始初始化返回true,出现异常则抛出异常;
        /// </summary>
        protected virtual Task WaitPrepare(CancellationToken token)
        {
            return null;
        }

        protected virtual void OnStart()
        {
            Debug.Log(InitializerName + "开始初始化;");
        }

        /// <summary>
        /// 在初始化完成时调用;
        /// </summary>
        protected virtual void OnCompleted()
        {
            Debug.Log(InitializerName + "初始化完成;");
        }

        /// <summary>
        /// 在失败时调用;
        /// </summary>
        protected virtual void OnFaulted(Exception ex)
        {
            Debug.LogError(InitializerName + "初始化时遇到错误:" + ex);
        }

        public static Task WaitInitializer<T1>(Initializer<T1> initializer, CancellationToken token)
        {
            if (initializer.InitializeTask == null)
            {
                return Task.Run(delegate ()
                {
                    while (!initializer.IsCompleted)
                    {
                        token.ThrowIfCancellationRequested();
                    }
                    if (initializer.IsFaulted)
                    {
                        throw initializer.Exception;
                    }
                }, token);
            }
            else
            {
                return initializer.InitializeTask.ContinueWith(delegate (Task task)
                {
                    if (initializer.IsFaulted)
                    {
                        throw initializer.Exception;
                    }
                }, token);
            }
        }

        protected virtual void OnDestroy()
        {
            CancelInitialize();
        }

        /// <summary>
        /// 取消初始化;
        /// </summary>
        public virtual void CancelInitialize()
        {
            if (TokenSource != null)
            {
                TokenSource.Cancel();
            }
        }


        public TaskAwaiter GetAwaiter()
        {
            StartInitialize();
            return InitializeTask.GetAwaiter();
        }

        public struct InitializerAwaiter : INotifyCompletion
        {
            internal InitializerAwaiter(Initializer<T> initializer)
            {
                Initializer = initializer;
            }

            public Initializer<T> Initializer { get; private set; }

            public bool IsCompleted
            {
                get { return Initializer.IsCompleted; }
            }

            public void OnCompleted(Action continuation)
            {
                Initializer<T> initializer = Initializer;
                CancellationToken token = initializer.TokenSource.Token;
                initializer.InitializeTask.ContinueWith(delegate (Task task)
                {
                    if (task.IsFaulted)
                    {
                        throw task.Exception;
                    }
                    continuation();
                }, token);

                //if (initializer.InitializeTask != null)
                //{
                //    initializer.InitializeTask.ContinueWith(_ => continuation(), token);
                //}
                //else
                //{
                //    Task.Run(delegate ()
                //    {
                //        while (!initializer.IsCompleted)
                //        {
                //            token.ThrowIfCancellationRequested();
                //        }
                //        continuation();
                //    }, token);
                //}
            }

            public void GetResult()
            {
                if (Initializer.IsFaulted)
                {
                    throw Initializer.Exception;
                }
            }
        }
    }
}
