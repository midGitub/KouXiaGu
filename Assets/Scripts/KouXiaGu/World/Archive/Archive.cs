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
    public class Archive
    {
        public Archive(string archiveDirectory)
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
    }
}
