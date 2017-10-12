using System;
using System.IO;

namespace JiongXiaGu.Unity.Resources
{
    /// <summary>
    /// 拓展数据信息;
    /// </summary>
    public class DataDirectoryInfo
    {
        /// <summary>
        /// 存放数据和配置文件的文件夹;
        /// </summary>
        public DirectoryInfo DirectoryInfo { get; private set; }

        /// <summary>
        /// 数据唯一名称;
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// 是否为主要数据?
        /// </summary>
        public bool IsCoreData { get; private set; }

        /// <summary>
        /// 该数据是否启用?
        /// </summary>
        public bool Enabled { get; private set; }

        public DataDirectoryInfo(DirectoryInfo directoryInfo) : this(directoryInfo, false)
        {
        }

        internal DataDirectoryInfo(DirectoryInfo directoryInfo, bool isCoreData)
        {
            if (directoryInfo == null)
                throw new ArgumentNullException(nameof(directoryInfo));

            DirectoryInfo = directoryInfo;
        }
    }
}
