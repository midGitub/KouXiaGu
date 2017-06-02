using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace KouXiaGu.Concurrent
{

    /// <summary>
    /// 表示异步请求;
    /// </summary>
    public interface IAsyncRequest
    {
        void AddQueue();
        IEnumerator Operate();
        void OutQueue();
    }
}
