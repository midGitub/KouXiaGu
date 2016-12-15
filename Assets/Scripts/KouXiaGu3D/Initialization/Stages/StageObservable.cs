using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KouXiaGu.Initialization
{


    /// <summary>
    /// 存在进入和离开的阶段;
    /// </summary>
    public abstract class StageObservable<T> : IPeriod
    {
        const bool INSTANT = false;

        readonly HashSet<IStageObserver<T>> observerSet = new HashSet<IStageObserver<T>>();
        readonly Queue<IEnumerator> coroutineQueue = new Queue<IEnumerator>();

        protected abstract T Resource { get; }
        protected abstract Stages Deputy { get; }
        protected abstract bool Premise(Stages current);

        /// <summary>
        /// 当成功进入到这个状态时调用;
        /// </summary>
        protected abstract void LastEnter();

        Stages IPeriod.Deputy
        {
            get { return Deputy; }
        }

        bool IPeriod.Instant
        {
            get { return INSTANT; }
        }

        bool IPeriod.Premise(Stages current)
        {
            return Premise(current);
        }

        IEnumerator IPeriod.OnEnter()
        {
            IEnumerator coroutine;
            Queue<IEnumerator> coroutineQueue = GetEnterCoroutineQueue();

            while (coroutineQueue.Count != 0)
            {
                coroutine = coroutineQueue.Dequeue();

                while (coroutine.MoveNext())
                {
                    yield return null;
                }
            }

            LastEnter();
            yield break;
        }

        Queue<IEnumerator> GetEnterCoroutineQueue()
        {
            IEnumerator coroutine;
            coroutineQueue.Clear();

            foreach (var observer in observerSet)
            {
                coroutine = observer.OnEnter(Resource);
                coroutineQueue.Enqueue(coroutine);
            }

            return coroutineQueue;
        }


        IEnumerator IPeriod.OnLeave()
        {
            IEnumerator coroutine;
            Queue<IEnumerator> coroutineQueue = GetLeaveCoroutineQueue();

            while (coroutineQueue.Count != 0)
            {
                coroutine = coroutineQueue.Dequeue();

                while (coroutine.MoveNext())
                {
                    yield return null;
                }
            }

            yield break;
        }

        Queue<IEnumerator> GetLeaveCoroutineQueue()
        {
            IEnumerator coroutine;
            coroutineQueue.Clear();

            foreach (var observer in observerSet)
            {
                coroutine = observer.OnLeave(Resource);
                coroutineQueue.Enqueue(coroutine);
            }

            return coroutineQueue;
        }


        public bool Subscribe(IStageObserver<T> observer)
        {
            return observerSet.Add(observer);
        }

        public bool Unsubscribe(IStageObserver<T> observer)
        {
            return observerSet.Remove(observer);
        }

    }

}
