using System;
using System.Collections.Generic;
using System.IO;
using JiongXiaGu.Unity.Resources;

namespace JiongXiaGu.Unity.Archives
{


    public interface IArchiveFileInfo
    {
        /// <summary>
        /// 存档信息;
        /// </summary>
        ArchiveDescription Description { get; }

        /// <summary>
        /// 存档目录;
        /// </summary>
        string ArchiveDirectory { get; }
    }

    /// <summary>
    /// 表示一个存档;
    /// </summary>
    public class ArchiveInfo : IArchiveFileInfo
    {
        /// <summary>
        /// 存档信息;
        /// </summary>
        public ArchiveDescription Description { get; internal set; }

        /// <summary>
        /// 存档路径的 DirectoryInfo 实例;
        /// </summary>
        public DirectoryInfo DirectoryInfo { get; internal set; }

        /// <summary>
        /// 指定存档路径,信息,但不进行创建存档;
        /// </summary>
        public ArchiveInfo(ArchiveDescription description, string directory) : this(description, new DirectoryInfo(directory))
        {
        }

        /// <summary>
        /// 指定存档路径,信息,但不进行创建存档;
        /// </summary>
        public ArchiveInfo(ArchiveDescription description, DirectoryInfo directoryInfo)
        {
            DirectoryInfo = directoryInfo;
            Description = description;
        }

        /// <summary>
        /// 存档目录;
        /// </summary>
        public string ArchiveDirectory
        {
            get { return DirectoryInfo.FullName; }
        }

        public override string ToString()
        {
            return base.ToString() + "[ArchiveDirectory:" + ArchiveDirectory + "]";
        }

        ///// <summary>
        ///// 获取到一个新的存档目录;
        ///// </summary>
        //public static string GetNewArchiveDirectory()
        //{
        //    string newDirectoryName = DateTime.Now.Ticks.ToString();
        //    string path = Path.Combine(ArchivesDirectory, newDirectoryName);
        //    for (uint i = 0; i < 500; i++)
        //    {
        //        string correctedPath = path;
        //        if (!File.Exists(correctedPath))
        //        {
        //            return correctedPath;
        //        }
        //        else
        //        {
        //            correctedPath = path + i;
        //        }
        //    }
        //    throw new IOException("无法提供随机文件目录;");
        //}

        /// <summary>
        /// 根据时间升序的对比器;存档时间由早到晚;
        /// </summary>
        public class OrderByTimeAscendingComparer : Comparer<ArchiveInfo>
        {
            public override int Compare(ArchiveInfo x, ArchiveInfo y)
            {
                return x.Description.Time.CompareTo(y.Description.Time);
            }
        }

        /// <summary>
        /// 根据时间降序的对比器;存档时间由晚到早;
        /// </summary>
        public class OrderByTimeDescendingComparer : Comparer<ArchiveInfo>
        {
            public override int Compare(ArchiveInfo x, ArchiveInfo y)
            {
                return -x.Description.Time.CompareTo(y.Description.Time);
            }
        }
    }
}
