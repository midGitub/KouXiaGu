using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JiongXiaGu
{

    public interface IObjectPool<T>
    {
        int Count { get; }
        T Get();
        void Release(T item);
    }
}
