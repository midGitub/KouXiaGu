using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JiongXiaGu.Unity
{

    /// <summary>
    /// 单元测试抽象类;
    /// </summary>
    public class NUnit
    {
        /// <summary>
        /// 用于单元测试的临时目录;
        /// </summary>
        public static string TempDirectory => @"NUnitTemp";

        /// <summary>
        /// 用于测试输出的根目录;
        /// </summary>
        public string RootDirectory { get; private set; }

        /// <summary>
        /// 通过项目名称创建;
        /// </summary>
        public NUnit(string name)
        {
            RootDirectory = Path.Combine(TempDirectory, name);
            if (Directory.Exists(RootDirectory))
            {
                Directory.Delete(RootDirectory, true);
            }
            Directory.CreateDirectory(RootDirectory);
        }
    }
}
