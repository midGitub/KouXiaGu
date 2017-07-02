using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KouXiaGu.Diagnostics
{

    /// <summary>
    /// 命令条目接口;
    /// </summary>
    public interface ICommandItem
    {
        /// <summary>
        /// 是否为开发模式的方法?
        /// </summary>
        bool IsDeveloperMethod { get; }

        /// <summary>
        /// 参数数量;
        /// </summary>
        int ParameterNumber { get; }

        /// <summary>
        /// 预留消息;
        /// </summary>
        string Message { get; }

        /// <summary>
        /// 对应的操作;
        /// </summary>
        void Operate(string[] parameters);
    }
}
