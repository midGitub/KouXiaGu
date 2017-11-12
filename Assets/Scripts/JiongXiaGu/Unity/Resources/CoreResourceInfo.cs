using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JiongXiaGu.Unity.Resources
{


    internal class CoreResourceInfo : ILoadableResource
    {
        public ResourceType Type
        {
            get { return ResourceType.Core; }
        }

        public string Name
        {
            get { return "Core"; }
        }

        public DirectoryInfo DirectoryInfo { get; private set; }

        public CoreResourceInfo(DirectoryInfo directoryInfo)
        {
            DirectoryInfo = directoryInfo;
        }
    }
}
