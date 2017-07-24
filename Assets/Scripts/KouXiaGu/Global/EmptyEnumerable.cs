using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KouXiaGu
{

    /// <summary>
    /// 不进行迭代的结构;
    /// </summary>
    public class EmptyEnumerable<T> : IEnumerable<T>, IEnumerator<T>
    {
        static EmptyEnumerable()
        {
            Default = new EmptyEnumerable<T>();
        }

        public static EmptyEnumerable<T> Default { get; private set; }

        public T Current
        {
            get { return default(T); }
        }

        object IEnumerator.Current
        {
            get { return Current; }
        }

        public void Dispose()
        {
            return;
        }

        public bool MoveNext()
        {
            return false;
        }

        public void Reset()
        {
            return;
        }

        public IEnumerator<T> GetEnumerator()
        {
            return this;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this;
        }
    }
}
