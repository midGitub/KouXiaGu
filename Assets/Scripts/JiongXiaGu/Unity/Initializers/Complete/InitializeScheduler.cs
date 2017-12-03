using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace JiongXiaGu.Unity.Initializers
{


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

        public Task InitializeTask { get; private set; }
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
            CancellationTokenSource = new CancellationTokenSource();
            InitializeTask = new Task(InternalInitialize, CancellationTokenSource.Token);
        }

        protected virtual void OnDestroy()
        {
            Cancel();
        }

        /// <summary>
        /// 开始进行初始化;
        /// </summary>
        public void StartInitialize()
        {
            if ((options & InitializeOptions.MoveToUnityThread) > InitializeOptions.None)
            {
                if (XiaGu.IsUnityThread)
                {
                    InitializeTask.Start();
                }
                else
                {
                    InitializeTask.Start(XiaGu.UnityTaskScheduler);
                }
            }
            else
            {
                InitializeTask.Start();
            }

            InitializeTask.ContinueWith(delegate (Task task)
            {
                if (task.IsFaulted)
                {
                    Cancel();
                    OnFaulted(task.Exception);
                }
                else
                {
                    OnCompleted();
                }
            });
        }

        /// <summary>
        /// 取消初始化;
        /// </summary>
        public void Cancel()
        {
            CancellationTokenSource.Cancel();
        }

        private void InternalInitialize()
        {
            bool ignoreException = (Options & InitializeOptions.IgnoreException) > InitializeOptions.None; 
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
                                Task.WaitAny(waitedOnTaskArray.ToArray(), CancellationTokenSource.Token);
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
                Task.WaitAll(waitedOnTaskArray.ToArray(), CancellationTokenSource.Token);
            }
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
                Debug.LogError(task.Exception);
            }
            else
            {
                throw task.Exception;
            }
        }

        protected virtual void OnFaulted(Exception ex)
        {
        }

        protected virtual void OnCompleted()
        {
        }

        [Flags]
        public enum InitializeOptions
        {
            None = 1 << 0,

            /// <summary>
            /// 忽略初始化过程中的异常;
            /// </summary>
            IgnoreException = 1 << 1,

            /// <summary>
            /// 仅在Unity线程初始化,若不是Unity线程则转到;
            /// </summary>
            MoveToUnityThread = 1 << 2,
        }
    }
}
