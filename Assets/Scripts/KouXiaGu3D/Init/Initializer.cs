using System;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

namespace KouXiaGu
{

    /// <summary>
    /// 游戏阶段初始化器;
    /// </summary>
    public class Initializer
    {

        /// <summary>
        /// 存储游戏进行阶段的栈;
        /// </summary>
        static readonly Stack<IPeriod> stageStack = new Stack<IPeriod>();

        /// <summary>
        /// 保存在栈中的所有状态;
        /// </summary>
        static GameStages stages = GameStages.Empty;

        /// <summary>
        /// 保存在栈中的所有状态;
        /// </summary>
        public static GameStages Stages
        {
            get { return stages; }
            private set { stages = value; }
        }

        /// <summary>
        /// 是正在进行状态切换?
        /// </summary>
        public static bool IsRunning
        {
            get { return Contains(GameStages.Running); }
        }

        /// <summary>
        /// 当前进行的阶段;
        /// </summary>
        static IPeriod Activated
        {
            get { return stageStack.Peek(); }
        }

        public void Add(IPeriod item)
        {
            if (IsRunning)
                throw new InvalidOperationException("当前状态不允许加入任何状态;");
            if (!item.Premise())
                throw new PremiseNotInvalidException(item.Deputy + "的前提不满足;");

            if (Activated.Instant)
            {
                EnterInstant(item);
            }
            else
            {
                Enter(item);
            }
        }

        public void Remove(IPeriod item)
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
            Add(GameStages.Running);
            Add(item.Deputy);

            Action<Exception> onError = e => {
                OnError(item, e);
                Remove(GameStages.Running);
            };
            Action onCompleted = () => {
                Remove(GameStages.Running);
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
            Add(GameStages.Running);

            Action<Exception> onError = e => {
                OnError(item, e);
                Remove(GameStages.Running);
            };
            Action onCompleted = () => {
                Remove(GameStages.Running);
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
            Remove(GameStages.Running);

            Action<Exception> onError = e => {
                OnError(item, e);
                Remove(GameStages.Running);
            };
            Action onCompleted = () => {
                Remove(GameStages.Running);
                Pop(item);
            };

            Observable.FromMicroCoroutine(item.OnLeave).
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

        static void Add(GameStages stage)
        {
            Stages |= stage;
        }

        static void Remove(GameStages stage)
        {
            Stages &= ~stage;
        }

        public static bool Contains(GameStages stage)
        {
            bool contains = (Stages & stage) != GameStages.Empty;
            return contains;
        }

    }

}
