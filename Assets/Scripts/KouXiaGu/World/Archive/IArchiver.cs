using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KouXiaGu.World
{

    [Obsolete]
    public interface IArchiver
    {
        /// <summary>
        /// 将存档信息输出到文件;
        /// </summary>
        void Write(string directoryPath);
    }

}
