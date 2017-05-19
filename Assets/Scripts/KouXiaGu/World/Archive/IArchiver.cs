using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KouXiaGu.World
{

    public interface IArchiver
    {
        /// <summary>
        /// 准备存档信息;
        /// </summary>
        void Prepare();

        /// <summary>
        /// 将存档信息输出到文件;
        /// </summary>
        void Write(ArchiveFile file);
    }

}
