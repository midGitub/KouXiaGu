using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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

        bool IEnumerator.MoveNext()
        {
            Next();
            return IsCompleted;
        }

        void IEnumerator.Reset()
        {
            throw new NotImplementedException();
        }

        public void Next()
        {
            if (coroutineStack.Count != 0)
            {
                IEnumerator request = coroutineStack.Peek();
                bool moveNext = false;

                try
                {
                    moveNext = request.MoveNext();
                }
                catch (Exception e)
                {
                    OnFaulted(e);
                    coroutineStack.Clear();
                }

                if (!moveNext)
                {
                    coroutineStack.Pop();
                    if (coroutineStack.Count == 0)
                    {
                        coroutineStack.Clear();
                        OnCompleted();
                    }
                    return;
                }

                var newCoroutine = request.Current as IEnumerator;
                if (newCoroutine != null)
                {
                    coroutineStack.Push(newCoroutine);
                }
            }
        }

        public void Cancele()
        {
            OnCanceled();
        }
    }

}
