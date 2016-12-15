using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KouXiaGu.Initialization
{


    public abstract class InstantObservable<T> : IPeriod
    {
        const bool INSTANT = true;

        readonly HashSet<IStageEnter<T>> observerSet = new HashSet<IStageEnter<T>>();
        readonly Queue<IEnumerator> coroutineQueue = new Queue<IEnumerator>();

        protected abstract T Resource { get; }
        protected abstract Stages Deputy { get; }
        protected abstract bool Premise(Stages current);

        /// <summary>
        /// 当成功进行完毕这个阶段时调用;
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
            throw new NotImplementedException("不应该调用此方法!");
        }

        public bool Subscribe(IStageEnter<T> observer)
        {
            return observerSet.Add(observer);
        }

        public bool Unsubscribe(IStageEnter<T> observer)
        {
            return observerSet.Remove(observer);
        }

    }

}
