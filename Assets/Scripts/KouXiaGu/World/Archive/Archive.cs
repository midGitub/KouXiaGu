using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace KouXiaGu.World
{

    /// <summary>
    /// 存档;
    /// </summary>
    public class Archive
    {
        public Archive()
        {
            IsEffective = false;
            ArchiveDirectory = string.Empty;
        }

        public Archive(string archiveDirectory)
        {
            IsEffective = true;
            ArchiveDirectory = archiveDirectory;
        }

        public bool IsEffective { get; private set; }
        public string ArchiveDirectory { get; private set; }
    }

}
