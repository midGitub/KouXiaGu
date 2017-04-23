using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using UnityEngine;

namespace KouXiaGu.World
{

    /// <summary>
    /// 存档;
    /// </summary>
    [Serializable]
    public class ArchiveFile
    {
        public ArchiveFile()
        {
            IsEffective = false;
            ArchiveDirectory = string.Empty;
        }

        public ArchiveFile(string archiveDirectory)
        {
            IsEffective = true;
            ArchiveDirectory = archiveDirectory;
        }

        [SerializeField]
        bool isEffective;

        [SerializeField]
        string archiveDirectory;

        public bool IsEffective
        {
            get { return isEffective; }
            private set { isEffective = value; }
        }

        public string ArchiveDirectory
        {
            get { return archiveDirectory; }
            private set { archiveDirectory = value; }
        }

    }

}
