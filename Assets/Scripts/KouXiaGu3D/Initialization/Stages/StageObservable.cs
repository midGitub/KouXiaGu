using System;
using System.Collections;
using System.Collections.Generic;

namespace KouXiaGu.Initialization
{


    public abstract class StageObservable<T> : IPeriod
    {

        readonly HashSet<IStageObserver<T>> observerSet = new HashSet<IStageObserver<T>>();
        readonly CoroutineList<IStageObserver<T>> coroutineList = new CoroutineList<IStageObserver<T>>();

        public abstract Stages Deputy { get; }
        public abstract bool Instant { get; }
        protected abstract T Resource { get; }

        public abstract bool Premise(Stages current);

        public bool Subscribe(IStageObserver<T> observer)
        {
            return observerSet.Add(observer);
        }

        public bool Unsubscribe(IStageObserver<T> observer)
        {
            return observerSet.Remove(observer);
        }

        IEnumerator IPeriod.OnEnter()
        {
            Func<IStageObserver<T>, IEnumerator> onNext = observer => observer.OnEnter(Resource);
            Func<IStageObserver<T>, IEnumerator> onRollBack = observer => observer.OnEnterRollBack(Resource);
            Action<IStageObserver<T>> completed = observer => observer.OnEnterCompleted();

            return OnNext(onNext, onRollBack, completed);
        }

        IEnumerator IPeriod.OnLeave()
        {
            Func<IStageObserver<T>, IEnumerator> onNext = observer => observer.OnLeave(Resource);
            Func<IStageObserver<T>, IEnumerator> onRollBack = observer => observer.OnLeaveRollBack(Resource);
            Action<IStageObserver<T>> completed = observer => observer.OnLeaveCompleted();

            return OnNext(onNext, onRollBack, completed);
        }

        IEnumerator OnNext(
            Func<IStageObserver<T>, IEnumerator> next,
            Func<IStageObserver<T>, IEnumerator> rollBack,
            Action<IStageObserver<T>> completed)
        {
            Exception error = null;
            coroutineList.SetCoroutines(observerSet, next);

            while (coroutineList.WaitCount > 0)
            {
                bool moveNext = false;
                try
                {
                    moveNext = coroutineList.Coroutine.MoveNext();
                }
                catch (Exception e)
                {
                    error = e;
                    var completes = coroutineList.GetCompletes();
                    coroutineList.SetCoroutines(completes, rollBack);
                }

                if (moveNext)
                {
                    yield return null;
                }
                else
                {
                    coroutineList.MoveNext();
                }
            }

            if (error != null)
            {
                throw error;
            }
            else
            {
                foreach (var observer in observerSet)
                {
                    completed(observer);
                }
            }
            yield break;
        }

    }



}
