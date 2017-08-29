using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KouXiaGu.World
{

    /// <summary>
    /// 游戏世界组件初始化接口,在Unity线程调用;
    /// </summary>
    public interface IComponentInitializer
    {
        void WorldDataCompleted(IWorldData data);
    }
}
