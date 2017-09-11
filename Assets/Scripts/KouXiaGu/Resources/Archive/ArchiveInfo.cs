using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using UnityEngine;

namespace KouXiaGu.Resources.Archive
{

    /// <summary>
    /// 存档;
    /// </summary>
    [Serializable]
    public class ArchiveInfo
    {
        public ArchiveInfo(string archiveDirectory)
        {
            Directory = archiveDirectory;
        }

        [SerializeField]
        string directory;

        public string Directory
        {
            get { return directory; }
            private set { directory = value; }
        }

        public string GetFullPath(string name)
        {
            return Path.Combine(directory, name);
        }
    }
}
