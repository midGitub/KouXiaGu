using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KouXiaGu.Grids;
using UnityEngine;

namespace KouXiaGu.Terrain3D
{

    public class CoroutineQueue<T> : IEnumerator
        where T : IEnumerator
    {
        public CoroutineQueue(ISegmented stopwatch)
        {
            requestQueue = new Queue<T>();
            coroutineStack = new Stack<IEnumerator>();
            Stopwatch = stopwatch;
        }

        readonly Queue<T> requestQueue;
        readonly Stack<IEnumerator> coroutineStack;
        public ISegmented Stopwatch { get; set; }

        object IEnumerator.Current
        {
            get { return null; }
        }

        public void Next()
        {
            if (coroutineStack.Count != 0)
            {
                Stopwatch.Restart();
                while (!Stopwatch.Await())
                {
                    IEnumerator request = coroutineStack.Peek();

                    bool moveNext = request.MoveNext();
                    if (!moveNext)
                    {
                        coroutineStack.Pop();
                        if (coroutineStack.Count == 0)
                        {
                            requestQueue.Dequeue();
                            if (requestQueue.Count == 0)
                            {
                                break;
                            }
                            else
                            {
                                T item = requestQueue.Peek();
                                coroutineStack.Push(item);
                            }
                        }
                        continue;
                    }

                    var newCoroutine = request.Current as IEnumerator;
                    if (newCoroutine != null)
                    {
                        coroutineStack.Push(newCoroutine);
                    }
                }
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

    }

    [DisallowMultipleComponent]
    public class LandformBakeQueue : MonoBehaviour
    {
        LandformBakeQueue()
        {
        }

        [SerializeField]
        Stopwatch runtimeStopwatch;

        CoroutineQueue<IEnumerator> requestQueue;

        void Awake()
        {
            requestQueue = new CoroutineQueue<IEnumerator>(runtimeStopwatch);
            requestQueue.Add(Test1());
            requestQueue.Add(Test1());
            requestQueue.Add(Test1());
        }

        void Update()
        {
            requestQueue.Next();
        }

        IEnumerator Test1()
        {
            Debug.Log("1");
            yield return Test2();
        }

        IEnumerator Test2()
        {
            Debug.Log("2");
            yield return Test3();
        }

        IEnumerator Test3()
        {
            Debug.Log("3");
            yield break;
        }
    }

}
