using System;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

namespace KouXiaGu.Initialization
{

    /// <summary>
    /// 游戏阶段初始化器;
    /// </summary>
    public static class Initializer
    {

        /// <summary>
        /// 存储游戏进行阶段的栈;
        /// </summary>
        static readonly Stack<IPeriod> stageStack = new Stack<IPeriod>();

        /// <summary>
        /// 保存在栈中的所有状态;
        /// </summary>
        static Stages stages = Stages.Empty;

        /// <summary>
        /// 保存在栈中的所有状态;
        /// </summary>
        public static Stages Stages
        {
            get { return stages; }
            private set { stages = value; }
        }

        /// <summary>
        /// 是正在进行状态切换?
        /// </summary>
        public static bool IsRunning
        {
            get { return Contains(Stages.Running); }
        }

        /// <summary>
        /// 当前进行的阶段;
        /// </summary>
        static IPeriod Activated
        {
            get { return stageStack.Peek(); }
        }

        public static void Add(IPeriod item)
        {
            if (IsRunning)
                throw new InvalidOperationException("当前状态不允许加入任何状态;");
            if (!item.Premise(Stages))
                throw new PremiseNotInvalidException(item.Deputy + "的前提不满足;");

            if (item.Instant)
            {
                EnterInstant(item);
            }
            else
            {
                Enter(item);
            }
        }

        public static void Remove(IPeriod item)
        {
            if (IsRunning)
                throw new InvalidOperationException("当前状态不允许移除任何状态;");
            if (Activated != item)
                throw new ArgumentException("与当前激活的状态不符;现有" + Activated.Deputy + "; 请求:" + item.Deputy);

            Leave(item);
        }

        /// <summary>
        /// 执行中为 Running 和 其状态 ,执行完毕后移除 Running 和 其状态;
        /// </summary>
        static void EnterInstant(IPeriod item)
        {
            Add(Stages.Running);
            Add(item.Deputy);

            Action<Exception> onError = e => {
                OnError(item, e);
                Remove(Stages.Running);
            };
            Action onCompleted = () => {
                Remove(Stages.Running);
                Remove(item.Deputy);
            };

            Observable.FromMicroCoroutine(item.OnEnter).
                Subscribe(OnNext, onError, onCompleted);
        }

        /// <summary>
        /// 执行中为 Running ,执行完毕后移除 Running, 加入 其状态;
        /// </summary>
        static void Enter(IPeriod item)
        {
            Add(Stages.Running);

            Action<Exception> onError = e => {
                OnError(item, e);
                Remove(Stages.Running);
            };
            Action onCompleted = () => {
                Remove(Stages.Running);
                Push(item);
            };

            Observable.FromMicroCoroutine(item.OnEnter).
              Subscribe(OnNext, onError, onCompleted);
        }

        /// <summary>
        /// 执行中为 Running , 且移除 其状态,执行完毕后移除 Running;
        /// </summary>
        static void Leave(IPeriod item)
        {
            Remove(Stages.Running);

            Action<Exception> onError = e => {
                OnError(item, e);
                Remove(Stages.Running);
            };
            Action onCompleted = () => {
                Remove(Stages.Running);
                Pop(item);
            };

            Observable.FromMicroCoroutine(item.OnEnter).
             Subscribe(OnNext, onError, onCompleted);
        }


        static void OnNext(Unit unit)
        {
        }

        static void OnError(IPeriod item, Exception e)
        {
            Debug.LogError("初始化失败;" + item.Deputy + "\n" + e);
        }


        /// <summary>
        /// 插入栈顶部;
        /// </summary>
        static void Push(IPeriod item)
        {
            stageStack.Push(item);
            Add(item.Deputy);
        }

        /// <summary>
        /// 弹出栈顶元素;
        /// </summary>
        static void Pop(IPeriod item)
        {
            var value = stageStack.Pop();
            Remove(item.Deputy);
        }

        public static bool Contains(IPeriod item)
        {
            return stageStack.Contains(item);
        }

        static void Add(Stages stage)
        {
            Stages |= stage;
        }

        static void Remove(Stages stage)
        {
            Stages &= ~stage;
        }

        public static bool Contains(Stages stage)
        {
            bool contains = (Stages & stage) != Stages.Empty;
            return contains;
        }

    }

}
