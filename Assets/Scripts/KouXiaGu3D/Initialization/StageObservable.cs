using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KouXiaGu.Initialization
{

    /// <summary>
    /// 阶段切换方法抽象类;
    /// </summary>
    public abstract class StageObservable<T> : IPeriod
    {

        readonly CoroutineList<IStageObserver<T>> coroutineList = new CoroutineList<IStageObserver<T>>();

        public abstract Stages Deputy { get; }
        public abstract bool Instant { get; }
        protected abstract T Resource { get; }
        protected abstract IEnumerable<IStageObserver<T>> Observers { get; }

        public abstract bool Premise(Stages current);

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
            coroutineList.SetCoroutines(Observers, next);

            while (coroutineList.WaitCount > 0)
            {
                bool moveNext = false;
                try
                {
                    moveNext = coroutineList.Coroutine.MoveNext();
                }
                catch (Exception e)
                {
                    if (error == null)
                    {
                        error = e;
                        var completes = coroutineList.GetCompletesAndCurrent();
                        coroutineList.SetCoroutines(completes, rollBack);
                        continue;
                    }
                    else
                    {
                        Debug.LogError("回滚时出现错误,跳过这个回滚;\n" + coroutineList.Item.ToString() + "\n" + e);
                        coroutineList.MoveNext();
                    }
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
                foreach (var observer in Observers)
                {
                    completed(observer);
                }
            }
            yield break;
        }

    }

}
