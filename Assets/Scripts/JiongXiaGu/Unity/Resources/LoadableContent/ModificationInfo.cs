using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JiongXiaGu.Unity.Resources
{


    public struct ModificationInfo
    {
        public IContentInfo ContentInfo { get; private set; }
        public ModificationDescription Description { get; private set; }
        public bool Exists => ContentInfo != null && ContentInfo.Exists;

        public ModificationInfo(IContentInfo contentInfo, ModificationDescription description)
        {
            ContentInfo = contentInfo;
            Description = description;
        }
    }
}
