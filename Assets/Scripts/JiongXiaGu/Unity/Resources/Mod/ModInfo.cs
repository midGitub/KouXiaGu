using System;
using System.IO;

namespace JiongXiaGu.Unity.Resources
{

    /// <summary>
    /// 模组信息;
    /// </summary>
    public class ModInfo
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
        /// 数据唯一名称;
        /// </summary>
        public string ModName
        {
            get { return Description.Name; }
        }

        /// <summary>
        /// 资源目录;
        /// </summary>
        public string FullPath
        {
            get { return DirectoryInfo.FullName; }
        }

        /// <summary>
        /// 构造模组数据信息;
        /// </summary>
        internal ModInfo(DirectoryInfo directoryInfo, ModDescription description) 
        {
            if (directoryInfo == null)
                throw new ArgumentNullException(nameof(directoryInfo));

            DirectoryInfo = directoryInfo;
            Description = description;
        }
    }
}
