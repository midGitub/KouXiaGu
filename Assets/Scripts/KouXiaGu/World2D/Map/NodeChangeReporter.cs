using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KouXiaGu.Grids;
using UniRx;

namespace KouXiaGu.World2D.Map
{

    /// <summary>
    /// 当新加入点,或者点内容发生变化时进行通知;
    /// </summary>
    class NodeChangeReporter<T> : IObservable<MapNodeState<T>>
    {
        public NodeChangeReporter() { }

        List<IObserver<MapNodeState<T>>> observers = new List<IObserver<MapNodeState<T>>>();

        public void NodeDataUpdate(ChangeType eventType, RectCoord mapPoint, T node)
        {
            if (observers.Count != 0)
            {
                MapNodeState<T> pari = new MapNodeState<T>(eventType, mapPoint, node);
                foreach (var observer in observers.ToArray())
                {
                    observer.OnNext(pari);
                }
            }
        }

        public IDisposable Subscribe(IObserver<MapNodeState<T>> observer)
        {
            if (observer == null)
                throw new NullReferenceException();

            if (!observers.Contains(observer))
                observers.Add(observer);
            return new Unsubscriber(observers, observer);
        }

        private class Unsubscriber : IDisposable
        {
            public Unsubscriber(List<IObserver<MapNodeState<T>>> observers, IObserver<MapNodeState<T>> observer)
            {
                this.observers = observers;
                this.observer = observer;
            }

            List<IObserver<MapNodeState<T>>> observers;
            IObserver<MapNodeState<T>> observer;

            public void Dispose()
            {
                observers.Remove(observer);
            }
        }

    }


}
