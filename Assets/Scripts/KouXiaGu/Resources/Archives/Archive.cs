using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using UnityEngine;

namespace KouXiaGu.Resources.Archives
{

    /// <summary>
    /// 存档;
    /// </summary>
    public class Archive
    {
        /// <summary>
        /// 创建一个新的存档;
        /// </summary>
        public Archive(ArchiveInfo info)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        public Archive(string archiveDirectory, ArchiveInfo info)
        {
            Directory = archiveDirectory;
            Info = info;
        }

        public string Directory { get; private set; }
        public ArchiveInfo Info { get; private set; }

        /// <summary>
        /// 更新Info信息;
        /// </summary>
        public void UpdateInfo()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 迭代获取到所有存档文件;
        /// </summary>
        public static IEnumerable<Archive> EnumerateArchives()
        {
            throw new NotImplementedException();
        }
    }
}
