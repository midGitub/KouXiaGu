using JiongXiaGu.Concurrent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace JiongXiaGu.Unity
{

    /// <summary>
    /// 初始化接口;
    /// </summary>
    public interface IGameInitializeHandle
    {
        /// <summary>
        /// 开始初始化;
        /// </summary>
        Task StartInitialize(CancellationToken token);
    }
}
