using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace JiongXiaGu.Unity.Resources
{


    /// <summary>
    /// 模组信息;
    /// </summary>
    public struct ModificationInfo
    {
        public string ModificationDirectory { get; private set; }
        public ModificationDescription Description { get; private set; }

        public ModificationInfo(string modificationDirectory, ModificationDescription description)
        {
            ModificationDirectory = modificationDirectory;
            Description = description;
        }

        public static implicit operator string(ModificationInfo info)
        {
            return info.ModificationDirectory;
        }
    }
}
