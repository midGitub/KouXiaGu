using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KouXiaGu
{

    /// <summary>
    /// 协程队列;
    /// </summary>
    public class CoroutineQueue<T> : IEnumerator, IEnumerable<T>
        where T : IEnumerator
    {
        public CoroutineQueue()
        {
            requestQueue = new Queue<T>();
            coroutineStack = new Stack<IEnumerator>();
        }

        readonly Queue<T> requestQueue;
        readonly Stack<IEnumerator> coroutineStack;

        object IEnumerator.Current
        {
            get { return null; }
        }

        public void Next()
        {
            if (coroutineStack.Count != 0)
            {
                IEnumerator request = coroutineStack.Peek();
                bool moveNext;

                try
                {
                    moveNext = request.MoveNext();
                }
                catch (Exception e)
                {
                    MoveNextCoroutine();
                    throw e;
                }

                if (!moveNext)
                {
                    coroutineStack.Pop();
                    if (coroutineStack.Count == 0)
                    {
                        MoveNextCoroutine();
                    }
                }

                var newCoroutine = request.Current as IEnumerator;
                if (newCoroutine != null)
                {
                    coroutineStack.Push(newCoroutine);
                }

            }
        }

        void MoveNextCoroutine()
        {
            coroutineStack.Clear();
            requestQueue.Dequeue();
            if (requestQueue.Count != 0)
            {
                T item = requestQueue.Peek();
                coroutineStack.Push(item);
            }
        }

        public void Add(T item)
        {
            requestQueue.Enqueue(item);
            if (coroutineStack.Count == 0)
            {
                coroutineStack.Push(item);
            }
        }

        public void Clear()
        {
            requestQueue.Clear();
            coroutineStack.Clear();
        }

        bool IEnumerator.MoveNext()
        {
            Next();
            return true;
        }

        void IEnumerator.Reset()
        {
            return;
        }

        public IEnumerator<T> GetEnumerator()
        {
            return requestQueue.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return requestQueue.GetEnumerator();
        }
    }

}
