using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JiongXiaGu.Unity.Archives
{

    /// <summary>
    /// 表示一个进行压缩的存档文件;
    /// </summary>
    public class ArchiveZipInfo
    {
        public ArchiveDescription Description { get; private set; }
        public FileInfo FileInfo { get; private set; }


    }
}
