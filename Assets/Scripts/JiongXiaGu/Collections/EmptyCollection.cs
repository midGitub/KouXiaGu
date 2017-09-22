using System.Collections;
using System.Collections.Generic;

namespace JiongXiaGu.Collections
{

    /// <summary>
    /// 一个空的,不可编辑的合集;
    /// </summary>
    internal class EmptyCollection<T> : IEnumerable<T>
    {
        EmptyCollection() { }

        static readonly EmptyCollection<T> instance = new EmptyCollection<T>();

        public static EmptyCollection<T> GetInstance
        {
            get { return instance; }
        }


        IEnumerator IEnumerable.GetEnumerator()
        {
            yield break;
        }

        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            yield break;
        }

    }

}
