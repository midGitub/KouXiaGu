using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JiongXiaGu.Unity.Resources
{

    /// <summary>
    /// 可读取的游戏内容;
    /// </summary>
    public interface ILoadableResource
    {
        /// <summary>
        /// 优先权,数值越小优先级越高;
        /// </summary>
        int Priority { get; }

        /// <summary>
        /// 内容名;
        /// </summary>
        string Name { get; }

        /// <summary>
        /// 资源目录;
        /// </summary>
        DirectoryInfo DirectoryInfo { get; }
    }
}
