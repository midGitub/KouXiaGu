using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KouXiaGu
{


    public class Set<T> : ICollection<T>
    {
        public Set()
        {
            collection = new List<T>();
            comparer = EqualityComparer<T>.Default;
        }

        public Set(IEqualityComparer<T> comparer)
        {
            collection = new List<T>();
            this.comparer = comparer;
        }

        readonly List<T> collection;
        IEqualityComparer<T> comparer;

        public IEqualityComparer<T> Comparer
        {
            get { return comparer; }
        }

        public int Count
        {
            get { return collection.Count; }
        }

        bool ICollection<T>.IsReadOnly
        {
            get { return false; }
        }

        public void Add(T item)
        {
            if (collection.Contains(item, comparer))
            {
                throw new ArgumentException();
            }
            else
            {
                collection.Add(item);
            }
        }

        public bool Remove(T item)
        {
            return collection.Remove(item);
        }

        public void Clear()
        {
            collection.Clear();
        }

        public bool Contains(T item)
        {
            return collection.Contains(item);
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            collection.CopyTo(array, arrayIndex);
        }

        public IEnumerator<T> GetEnumerator()
        {
            return ((ICollection<T>)collection).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((ICollection<T>)collection).GetEnumerator();
        }
    }

}
