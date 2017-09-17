using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace KouXiaGu
{

    public abstract class Initializer<T> : MonoBehaviour
    {
        List<Task> tasks;
        T[] initializers;
        internal Task InitializeTask { get; private set; }

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

        public bool IsRunning { get; private set; }
        protected CancellationTokenSource TokenSource { get; private set; }

        protected virtual void Awake()
        {
            tasks = new List<Task>();
            initializers = GetComponentsInChildren<T>();
            TokenSource = new CancellationTokenSource();
        }

        protected virtual void Start()
        {
            StartInitialize();
        }

        protected virtual void OnDestroy()
        {
            TokenSource.Cancel();
        }

        /// <summary>
        /// 初始化组件名;
        /// </summary>
        protected abstract string InitializerName { get; }

        /// <summary>
        /// 获取到
        /// </summary>
        protected abstract Task GetTask(T initializer);

        async void StartInitialize()
        {
            if (IsCompleted)
            {
                return;
            }
            IsRunning = true;

            try
            {
                while (!Prepare())
                {
                    await Task.Delay(500, TokenSource.Token);
                }

                Debug.Log(InitializerName + "开始初始化;");
                foreach (var initializer in initializers)
                {
                    Task task = GetTask(initializer);
                    if (task != null)
                    {
                        tasks.Add(task);
                    }
                }

                InitializeTask = Task.WhenAll(tasks);
                await InitializeTask;
                OnCompleted();
            }
            catch(Exception ex)
            {
                OnFaulted(ex);
            }
            finally
            {
                IsRunning = false;
            }
        }

        /// <summary>
        /// 进行准备,若为准备好初始化者返回false,准备开始初始化返回true,出现异常则抛出异常;
        /// </summary>
        protected virtual bool Prepare()
        {
            return true;
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
    }
}
