using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace JiongXiaGu.Unity.Initializers
{

    /// <summary>
    /// 抽象类 初始化器;
    /// </summary>
    public abstract class InitializeScheduler : MonoBehaviour
    {
        /// <summary>
        /// 并发执行数;
        /// </summary>
        [Range(1, 10)]
        [SerializeField]
        private int concurrencyNumber = 3;

        [EnumFlags]
        [SerializeField]
        private InitializeOptions options;

        private TaskCompletionSource<object> taskCompletionSource;

        public Task InitializeTask
        {
            get { return taskCompletionSource.Task; }
        }

        public CancellationTokenSource CancellationTokenSource { get; private set; }

        public int ConcurrencyNumber
        {
            get { return concurrencyNumber; }
        }

        public InitializeOptions Options
        {
            get { return options; }
        }

        protected virtual void Awake()
        {
            taskCompletionSource = new TaskCompletionSource<object>();
            CancellationTokenSource = new CancellationTokenSource();
        }

        protected virtual void OnDestroy()
        {
            Cancel();
        }

        /// <summary>
        /// 开始进行初始化;
        /// </summary>
        public async void StartInitialize()
        {
            try
            {
                if ((options & InitializeOptions.RunInUnityThread) > InitializeOptions.None && !UnityThread.IsUnityThread)
                {
                    await TaskHelper.Run(InternalInitialize, UnityThread.TaskScheduler);
                }
                else
                {
                    await InternalInitialize();
                }
                taskCompletionSource.SetResult(null);
                OnCompleted();
            }
            catch (Exception ex)
            {
                taskCompletionSource.SetException(ex);
                CancellationTokenSource.Cancel();
                OnFaulted(ex);
            }
        }

        private async Task InternalInitialize()
        {
            await First();

            using (IEnumerator<Task> initializeHandlers = EnumerateInitializeHandler().GetEnumerator())
            {
                List<Task> waitedOnTaskArray = new List<Task>(ConcurrencyNumber);

                while (initializeHandlers.MoveNext())
                {
                    Task task = initializeHandlers.Current;
                    if (task != null)
                    {
                        if (task.IsFaulted)
                        {
                            OnFaulted(task);
                        }
                        else
                        {
                            waitedOnTaskArray.Add(task);

                            if (waitedOnTaskArray.Count == waitedOnTaskArray.Capacity)
                            {
                                //等待操作完成, 并移除完成的操作
                                await Task.WhenAny(waitedOnTaskArray);
                                waitedOnTaskArray.RemoveAll(delegate (Task item)
                                {
                                    if (item.IsCompleted)
                                    {
                                        if (task.IsFaulted)
                                        {
                                            OnFaulted(task);
                                        }
                                        return true;
                                    }
                                    return false;
                                });
                            }
                        }
                    }
                }
                await Task.WhenAll(waitedOnTaskArray);
            }
        }

        /// <summary>
        /// 取消初始化;
        /// </summary>
        public void Cancel()
        {
            CancellationTokenSource.Cancel();
            taskCompletionSource.TrySetCanceled();
        }

        protected virtual Task First()
        {
            return Task.CompletedTask;
        }

        /// <summary>
        /// 获取到初始化内容;
        /// </summary>
        protected abstract IEnumerable<Task> EnumerateInitializeHandler();

        /// <summary>
        /// 在初始化过程中出现异常;
        /// </summary>
        protected virtual void OnFaulted(Task task)
        {
            bool ignoreException = (Options & InitializeOptions.IgnoreException) > InitializeOptions.None;
            if (ignoreException)
            {
                Debug.LogWarning(task.Exception);
            }
            else
            {
                throw task.Exception;
            }
        }

        /// <summary>
        /// 当初始化完毕后遇到错误调用;
        /// </summary>
        protected virtual void OnFaulted(Exception ex)
        {
        }

        /// <summary>
        /// 当初始化完成时调用;
        /// </summary>
        protected virtual void OnCompleted()
        {
        }
    }
}
