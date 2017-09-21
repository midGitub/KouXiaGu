using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KouXiaGu.World
{

    /// <summary>
    /// 当场景初始化完成时调用存在此接口的实例;
    /// </summary>
    public interface IWorldCompletedHandle
    {
        void OnWorldCompleted();
    }
}
