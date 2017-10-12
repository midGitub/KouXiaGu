﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JiongXiaGu.Unity.Resources
{

    /// <summary>
    /// 对命名空间 System.IO 方法的拓展;
    /// </summary>
    public static class IOExtensions
    {

        /// <summary>
        /// 若目录不存在,则弹出异常 DirectoryNotFoundException;
        /// </summary>
        public static void ThrowIfDirectoryNotExisted(this DirectoryInfo directoryInfo)
        {
            if (!directoryInfo.Exists)
            {
                throw new DirectoryNotFoundException(string.Format("目录不存在[{0}]", directoryInfo.FullName));
            }
        }
    }
}
