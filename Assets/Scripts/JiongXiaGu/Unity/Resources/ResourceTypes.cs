using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JiongXiaGu.Unity.Resources
{


    public enum ResourceTypes
    {
        /// <summary>
        /// 资源数据;
        /// </summary>
        Data,

        /// <summary>
        /// 用户数据;
        /// </summary>
        UserConfig,

        /// <summary>
        /// 用户存档数据;
        /// </summary>
        Archive,

        /// <summary>
        /// 资源数据目录;
        /// </summary>
        DataDirectory,

        /// <summary>
        /// 用户数据目录;
        /// </summary>
        UserConfigDirectory,

        /// <summary>
        /// 用户存档数据目录;
        /// </summary>
        ArchiveDirectory,
    }
}
