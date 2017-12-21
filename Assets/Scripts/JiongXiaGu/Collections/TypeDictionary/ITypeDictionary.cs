using System;
using System.Collections.Generic;

namespace JiongXiaGu.Collections
{
    public interface ITypeDictionary
    {
        int Count { get; }
        IEnumerable<Type> Keys { get; }
        IEnumerable<object> Values { get; }

        T Get<T>();
        void Set<T>(T item);
        void Add<T>(T item);
        AddOrUpdateStatus AddOrUpdate<T>(T item);
        bool Contains<T>();
        T Find<T>();
        object Find<T>(Func<object, bool> predicate);
        IEnumerable<T> FindAll<T>();
        bool Remove<T>();
        bool TryGetValue<T>(out T item);
        void Clear();
    }
}