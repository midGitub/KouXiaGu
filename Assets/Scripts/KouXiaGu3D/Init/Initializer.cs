using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

namespace KouXiaGu
{

    /// <summary>
    /// 进行的阶段;
    /// </summary>
    public interface IPeriod
    {
        /// <summary>
        /// 代表当前游戏进行的阶段;
        /// </summary>
        GameStages Deputy { get; }

        /// <summary>
        /// 一个瞬时的状态,如保存游戏,和读取游戏;
        /// 值执行 OnEnter() ,执行完毕后恢复之前的状态;
        /// </summary>
        bool Instant { get; }

        /// <summary>
        /// 是否允许进入当前阶段?允许返回true;
        /// </summary>
        bool Premise();

        /// <summary>
        /// 当进入状态栈时调用;
        /// </summary>
        IEnumerator OnEnter();

        /// <summary>
        /// 当弹出状态栈时调用;
        /// </summary>
        IEnumerator OnLeave();
    }


    /// <summary>
    /// 游戏阶段初始化器;
    /// </summary>
    public class Initializer
    {

        /// <summary>
        /// 存储游戏进行阶段的栈;
        /// </summary>
        static readonly Stack<IPeriod> stageStack = new Stack<IPeriod>();

        static GameStages stages = GameStages.Empty;

        /// <summary>
        /// 正在进行的状态;
        /// </summary>
        public static GameStages Stages
        {
            get { return stages; }
            private set { stages = value; }
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
            if (ContainsStage(GameStages.Running))
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
            if (ContainsStage(GameStages.Running))
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
            AddStage(GameStages.Running);
            AddStage(item.Deputy);

            Action<Exception> onError = e => {
                OnError(item, e);
                RemoveStage(GameStages.Running);
            };
            Action onCompleted = () => {
                RemoveStage(GameStages.Running);
                RemoveStage(item.Deputy);
            };

            Observable.FromMicroCoroutine(item.OnEnter).
                Subscribe(OnNext, onError, onCompleted);
        }

        /// <summary>
        /// 执行中为 Running ,执行完毕后移除 Running, 加入 其状态;
        /// </summary>
        static void Enter(IPeriod item)
        {
            AddStage(GameStages.Running);

            Action<Exception> onError = e => {
                OnError(item, e);
                RemoveStage(GameStages.Running);
            };
            Action onCompleted = () => {
                RemoveStage(GameStages.Running);
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
            RemoveStage(GameStages.Running);

            Action<Exception> onError = e => {
                OnError(item, e);
                RemoveStage(GameStages.Running);
            };
            Action onCompleted = () => {
                RemoveStage(GameStages.Running);
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
            AddStage(item.Deputy);
        }

        /// <summary>
        /// 弹出栈定元素;
        /// </summary>
        static void Pop(IPeriod item)
        {
            var value = stageStack.Pop();
            RemoveStage(item.Deputy);
        }


        static void AddStage(GameStages stage)
        {
            Stages |= stage;
        }

        static void RemoveStage(GameStages stage)
        {
            Stages &= ~stage;
        }

        static bool ContainsStage(GameStages stage)
        {
            bool contains = (Stages & stage) != GameStages.Empty;
            return contains;
        }

    }

}
