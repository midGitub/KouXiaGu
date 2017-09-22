using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JiongXiaGu
{

    /// <summary>
    /// 协程;
    /// </summary>
    class CustomCoroutine : AsyncOperation, IEnumerator
    {
        public CustomCoroutine(IEnumerator item)
        {
            coroutineStack = new Stack<IEnumerator>();
            coroutineStack.Push(item);
        }

        readonly Stack<IEnumerator> coroutineStack;

        object IEnumerator.Current
        {
            get { return null; }
        }

        /// <summary>
        /// 若还需要继续则返回true,若已经结束则返回false;
        /// </summary>
        public bool MoveNext()
        {
            if (coroutineStack.Count != 0)
            {
            Start:
                IEnumerator request = coroutineStack.Peek();
                bool moveNext = false;

                try
                {
                    moveNext = request.MoveNext();
                }
                catch (Exception ex)
                {
                    OnFaulted(ex);
                    coroutineStack.Clear();
                    throw ex;
                }

                if (!moveNext)
                {
                    coroutineStack.Pop();
                    if (coroutineStack.Count == 0)
                    {
                        coroutineStack.Clear();
                        OnCompleted();
                    }
                    return IsCompleted;
                }

                var newCoroutine = request.Current as IEnumerator;
                if (newCoroutine != null)
                {
                    coroutineStack.Push(newCoroutine);
                    goto Start;
                }
            }
            return !IsCompleted;
        }

        void IEnumerator.Reset()
        {
            throw new NotImplementedException();
        }
    }
}
