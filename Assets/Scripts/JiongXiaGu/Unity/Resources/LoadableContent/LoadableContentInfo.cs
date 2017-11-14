using System;
using System.IO;

namespace JiongXiaGu.Unity.Resources
{

    /// <summary>
    /// 可加载资源信息;
    /// </summary>
    public class LoadableContentInfo
    {
        /// <summary>
        /// 模组描述;
        /// </summary>
        public LoadableContentDescription Description { get; private set; }

        /// <summary>
        /// 资源目录信息;
        /// </summary>
        public DirectoryInfo DirectoryInfo { get; private set; }

        /// <summary>
        /// 资源类型;DLC 或 MOD;
        /// </summary>
        public LoadableContentType Type { get; private set; }

        /// <summary>
        /// 该资源是否存在?
        /// </summary>
        public bool Exists
        {
            get { return DirectoryInfo != null && DirectoryInfo.Exists; }
        }

        /// <summary>
        /// 构造不存在的模组数据信息;
        /// </summary>
        internal LoadableContentInfo(LoadableContentDescription description, LoadableContentType type)
        {
            Description = description;
            Type = type;
        }

        /// <summary>
        /// 构造模组数据信息;
        /// </summary>
        internal LoadableContentInfo(DirectoryInfo directoryInfo, LoadableContentDescription description, LoadableContentType type) 
        {
            if (directoryInfo == null)
                throw new ArgumentNullException(nameof(directoryInfo));

            DirectoryInfo = directoryInfo;
            Description = description;
            Type = type;
        }
    }
}
