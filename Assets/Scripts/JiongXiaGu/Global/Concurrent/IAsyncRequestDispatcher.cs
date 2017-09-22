using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JiongXiaGu.Concurrent
{

    public interface IAsyncRequestDispatcher
    {
        int RequestCount { get; }
        void Add(IAsyncRequest request);
    }
}
