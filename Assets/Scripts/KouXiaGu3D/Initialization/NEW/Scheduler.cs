using System;
using System.Collections.Generic;
using UnityEngine;

namespace KouXiaGu.Initialization
{

    /// <summary>
    /// 监视所有下级节点的 IOperationAsync 的进度;
    /// </summary>
    [DisallowMultipleComponent]
    public abstract class Scheduler : MonoBehaviour
    {
        protected Scheduler() { }


        Queue<IOperateAsync> waitOperaters;
        IOperateAsync[] operaters;

        /// <summary>
        /// 正在等待的,若不存在则为 null;
        /// </summary>
        public IOperateAsync current { get; private set; }


        /// <summary>
        /// 是否完成?
        /// </summary>
        public bool IsComplete
        {
            get { return Remainder == 0; }
        }

        /// <summary>
        /// 执行程序的总数,若还未初始化则返回 -1;
        /// </summary>
        public int Total
        {
            get { return operaters == null ? -1 : operaters.Length; }
        }

        /// <summary>
        /// 等待中的执行程序数目;若还未初始化则返回 -1;
        /// </summary>
        public int Remainder
        {
            get { return waitOperaters == null ? -1 : waitOperaters.Count; }
        }


        void Awake()
        {
            operaters = GetComponents<IOperateAsync>();
            waitOperaters = ToQueue(operaters);

            if (IsComplete)
                OnCompleteAll();
            else
                current = waitOperaters.Dequeue();
        }

        Queue<T> ToQueue<T>(T[] items)
        {
            Queue<T> queue = new Queue<T>(items.Length);
            foreach (var item in items)
            {
                queue.Enqueue(item);
            }
            return queue;
        }

        void Update()
        {
            if (current != null && current.IsCompleted)
            {
                if (current.IsFaulted)
                {
                    OnFail(current);
                }
                else
                {
                    OnComplete(current);
                }

                if (waitOperaters.Count > 0)
                {
                    current = waitOperaters.Dequeue();
                }
                else
                {
                    current = null;
                    OnCompleteAll();
                }
            }
        }


        /// <summary>
        /// 当所有操作都完成时调用;
        /// </summary>
        protected abstract void OnCompleteAll();

        /// <summary>
        /// 当存在完成时调用,提供剩余数目;
        /// </summary>
        protected abstract void OnComplete(IOperateAsync operater);

        /// <summary>
        /// 当存在一个任务失败时调用;
        /// </summary>
        protected abstract void OnFail(IOperateAsync operater);

    }

}
