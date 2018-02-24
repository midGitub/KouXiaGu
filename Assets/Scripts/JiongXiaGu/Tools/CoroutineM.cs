using System;
using System.Collections;
using System.Collections.Generic;

namespace JiongXiaGu
{

    /// <summary>
    /// 基于 IEnumerator 接口实现的协程;
    /// </summary>
    public sealed class CoroutineM : IEnumerator
    {
        private Stack<IEnumerator> stack;
        public int Depth => stack.Count;
        public bool IsComplete => stack == null;
        public bool IsFaulted => Exception != null;
        public Exception Exception { get; private set; }
        object IEnumerator.Current => stack != null ? stack.Peek() : null;

        public CoroutineM(IEnumerator method)
        {
            if (method == null)
                throw new ArgumentNullException(nameof(method));

            stack = new Stack<IEnumerator>();
            stack.Push(method);
        }

        /// <summary>
        /// 执行一步,若成功则返回true,若完成则返回false;
        /// </summary>
        public bool MoveNext()
        {
            if (!IsComplete)
            {
                IEnumerator current = stack.Peek();

                try
                {
                    if (current.MoveNext())
                    {
                        IEnumerator next = current.Current as IEnumerator;
                        if (next != null)
                        {
                            stack.Push(next);
                        }
                        return true;
                    }
                    else
                    {
                        stack.Pop();
                        if (stack.Count == 0)
                        {
                            stack = null;
                            return false;
                        }
                        else
                        {
                            return true;
                        }
                    }
                }
                catch (Exception ex)
                {
                    Exception = ex;
                    stack = null;
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 不实现,调用时返回异常;
        /// </summary>
        /// <exception cref="InvalidOperationException"></exception>
        public void Reset()
        {
            throw new InvalidOperationException();
        }
    }
}
