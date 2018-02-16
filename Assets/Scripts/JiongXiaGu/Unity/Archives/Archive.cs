using JiongXiaGu.Unity.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JiongXiaGu.Unity.Archives
{

    /// <summary>
    /// 存档;
    /// </summary>
    public class Archive
    {
        public Content Content { get; private set; }
        public ArchiveDescription Description { get; private set; }

        public Archive(Content content, ArchiveDescription description)
        {
            Content = content;
            Description = description;
        }
    }

}
