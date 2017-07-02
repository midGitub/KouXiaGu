using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using System.Diagnostics;

namespace KouXiaGu.Diagnostics
{

    /// <summary>
    /// 包装 UnityEngine.Debug,并且支持命令条目;
    /// </summary>
    public static class XiaGuConsole
    {
        static XiaGuConsole()
        {
            commandCollection = new CommandCollection();
        }

        static readonly CommandCollection commandCollection;

        /// <summary>
        /// 是否为开发者模式;
        /// </summary>
        public static bool IsDeveloperMode
        {
            get { return XiaGu.IsDeveloperMode; }
        }

        /// <summary>
        /// 初始化;
        /// </summary>
        internal static void Initialize()
        {
            ReflectionCommands.SearchMethod(commandCollection);
        }

        /// <summary>
        /// 设置激活状态;
        /// </summary>
        public static void SetActivate(bool activate)
        {
            throw new NotImplementedException();
        }
    }
}
