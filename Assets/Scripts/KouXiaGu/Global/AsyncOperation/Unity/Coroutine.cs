using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KouXiaGu
{

    /// <summary>
    /// 协程;
    /// </summary>
    class Coroutine : AsyncOperation, IEnumerator
    {
        public Coroutine(IEnumerator item)
        {
            coroutineStack = new Stack<IEnumerator>();
            coroutineStack.Push(item);
        }

        readonly Stack<IEnumerator> coroutineStack;

        object IEnumerator.Current
        {
            get { return null; }
        }

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
            return IsCompleted;
        }

        public void Cancele()
        {
            OnFaulted(new OperationCanceledException());
        }

        void IEnumerator.Reset()
        {
            throw new NotImplementedException();
        }
    }

}
