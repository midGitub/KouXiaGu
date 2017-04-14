using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

namespace KouXiaGu.Initialization
{

    /// <summary>
    /// 监视指定 IOperateAsync 的进度;
    /// </summary>
    public abstract class OperateWaiter : MonoBehaviour
    {
        protected OperateWaiter() { }


        /// <summary>
        /// 当前等待的下标;
        /// </summary>
        int currentPointer = -1;

        /// <summary>
        /// 所有操作者的数组;
        /// </summary>
        IList<IAsyncOperation> operaters;


        /// <summary>
        /// 正在等待的,若不存在则返回 null;
        /// </summary>
        public IAsyncOperation current
        {
            get { return operaters == null || currentPointer < 0 || currentPointer >= operaters.Count ? null : operaters[currentPointer]; }
        }

        /// <summary>
        /// 执行程序的总数,若还未初始化则返回 -1;
        /// </summary>
        public int Total
        {
            get { return operaters == null ? -1 : operaters.Count; }
        }

        /// <summary>
        /// 等待中的执行程序数目;若还未初始化则返回 -1;
        /// </summary>
        public int Remainder
        {
            get { return operaters == null ? -1 : operaters.Count - currentPointer; }
        }

        /// <summary>
        /// 正在等待;
        /// </summary>
        public bool IsWaiting
        {
            get { return currentPointer != -1; }
        }

        /// <summary>
        /// 开始进行等待;
        /// </summary>
        protected void StartWait(IList<IAsyncOperation> operaters)
        {
            if (IsWaiting)
                throw new ArgumentException("正在等待;");

            this.operaters = operaters;
            if (operaters.Count == 0)
            {
                Complete();
            }

            currentPointer = 0;
        }

        /// <summary>
        /// 初始化完成;
        /// </summary>
        void Complete()
        {
            operaters = null;
            currentPointer = -1;

            OnCompleteAll();
        }

        /// <summary>
        /// 当所有操作都完成时调用;
        /// </summary>
        protected abstract void OnCompleteAll();


        void Update()
        {
            CheckComplete();
        }

        void CheckComplete()
        {
            if (operaters != null)
            {
                var current = operaters[currentPointer];

                while (current.IsCompleted)
                {
                    if (current.IsFaulted)
                    {
                        OnFail(current, current.Ex);
                    }
                    else
                    {
                        OnComplete(current);
                    }

                    currentPointer++;

                    if (currentPointer >= operaters.Count)
                    {
                        Complete();
                        break;
                    }

                    current = operaters[currentPointer];
                }
            }
        }

        /// <summary>
        /// 当存在完成时调用,提供剩余数目;
        /// </summary>
        protected abstract void OnComplete(IAsyncOperation operater);

        /// <summary>
        /// 当存在一个任务失败时调用;
        /// </summary>
        protected abstract void OnFail(IAsyncOperation operater, Exception e);

    }

}
