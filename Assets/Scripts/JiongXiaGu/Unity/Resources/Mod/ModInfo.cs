using System;
using System.IO;

namespace JiongXiaGu.Unity.Resources
{

    /// <summary>
    /// 模组信息;
    /// </summary>
    public class ModInfo : IDisposable
    {
        /// <summary>
        /// 模组描述;
        /// </summary>
        public ModDescription Description { get; private set; }

        /// <summary>
        /// 资源目录信息;
        /// </summary>
        public DirectoryInfo DirectoryInfo { get; private set; }

        /// <summary>
        /// 是否为核心数据?
        /// </summary>
        public bool IsCoreData { get; private set; }

        /// <summary>
        /// 构造核心数据信息;
        /// </summary>
        internal ModInfo(DirectoryInfo directoryInfo)
        {
            if (directoryInfo == null)
                throw new ArgumentNullException(nameof(directoryInfo));

            DirectoryInfo = directoryInfo;
            IsCoreData = true;
        }

        /// <summary>
        /// 构造模组数据信息;
        /// </summary>
        public ModInfo(DirectoryInfo directoryInfo, ModDescription description) 
        {
            if (directoryInfo == null)
                throw new ArgumentNullException(nameof(directoryInfo));

            DirectoryInfo = directoryInfo;
            Description = description;
        }

        /// <summary>
        /// 数据唯一名称;
        /// </summary>
        public string Name
        {
            get { return Description.Name; }
        }

        /// <summary>
        /// 资源目录;
        /// </summary>
        public string Directory
        {
            get { return DirectoryInfo.FullName; }
        }

        public void Dispose()
        {
            return;
        }
    }
}
