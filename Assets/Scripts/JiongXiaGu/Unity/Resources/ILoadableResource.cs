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
        /// 类型;
        /// </summary>
        ResourceType Type { get; }

        /// <summary>
        /// 全称;
        /// </summary>
        string Name { get; }

        /// <summary>
        /// 存放目录;
        /// </summary>
        DirectoryInfo DirectoryInfo { get; }
    }
}
