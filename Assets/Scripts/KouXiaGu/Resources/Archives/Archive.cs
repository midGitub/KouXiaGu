using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace KouXiaGu.Resources.Archives
{

    /// <summary>
    /// 存档目录;
    /// </summary>
    public class Archive
    {
        /// <summary>
        /// 创建一个新的存档路径,但不进行创建存档;
        /// </summary>
        public Archive(ArchiveInfo info) : this(GetNewArchiveDirectory(), info)
        {
        }

        /// <summary>
        /// 指定存档路径,但不进行创建存档;
        /// </summary>
        public Archive(string archiveDirectory)
        {
            ArchiveDirectoryInfo = new DirectoryInfo(archiveDirectory);
            ArchiveInfo info;
            TryReadInfo(archiveDirectory, out info);
            Info = info;
        }

        /// <summary>
        /// 指定存档路径,信息,但不进行创建存档;
        /// </summary>
        public Archive(string archiveDirectory, ArchiveInfo info)
        {
            ArchiveDirectoryInfo = new DirectoryInfo(archiveDirectory);
            Info = info;
        }

        /// <summary>
        /// 存档信息序列化器;
        /// </summary>
        static readonly ArchiveInfoSerializer archiveInfoSerializer = new ArchiveInfoSerializer();

        /// <summary>
        /// 存档路径的 DirectoryInfo 实例;
        /// </summary>
        public DirectoryInfo ArchiveDirectoryInfo { get; private set; }

        /// <summary>
        /// 存档信息;
        /// </summary>
        public ArchiveInfo Info { get; set; }

        /// <summary>
        /// 存档存放路径;
        /// </summary>
        public static string ArchivesDirectory
        {
            get { return Resource.ArchivesDirectoryPath; }
        }

        /// <summary>
        /// 存档目录;
        /// </summary>
        public string ArchiveDirectory
        {
            get { return ArchiveDirectoryInfo.FullName; }
        }

        /// <summary>
        /// 创建存档,若存档已经存在则更新存档信息文件;
        /// </summary>
        public void Create()
        {
            ArchiveDirectoryInfo.Create();
            UpdateInfo();
        }

        /// <summary>
        /// 该存档是否存在?
        /// </summary>
        public bool Exists()
        {
            return ArchiveDirectoryInfo.Exists
                && ExistsInfo();
        }

        /// <summary>
        /// 是否存在信息文件?
        /// </summary>
        public bool ExistsInfo()
        {
            return File.Exists(archiveInfoSerializer.GetArchiveInfoPath(ArchiveDirectory));
        }

        /// <summary>
        /// 输出更新存档信息文件;
        /// </summary>
        public void UpdateInfo()
        {
            archiveInfoSerializer.Serialize(ArchiveDirectory, Info);
        }

        /// <summary>
        /// 输出更新存档信息文件;
        /// </summary>
        public void UpdateInfo(ArchiveInfo info)
        {
            Info = info;
            UpdateInfo();
        }

        /// <summary>
        /// 删除存档;
        /// </summary>
        public void Delete()
        {
            ArchiveDirectoryInfo.Refresh();
            if (ArchiveDirectoryInfo.Exists)
            {
                ArchiveDirectoryInfo.Delete(true);
            }
        }

        /// <summary>
        /// 移动存档;
        /// </summary>
        public void MoveTo(Archive archive)
        {
            archive.Delete();
            archive.ArchiveDirectoryInfo.Parent.Create();
            ArchiveDirectoryInfo.MoveTo(archive.ArchiveDirectory);
        }

        /// <summary>
        /// 尝试读取到信息文件,若未能读取到则返回默认值;
        /// </summary>
        public static bool TryReadInfo(string archiveDirectory, out ArchiveInfo info)
        {
            try
            {
                info = archiveInfoSerializer.Deserialize(archiveDirectory);
                return true;
            }
            catch
            {
                info = default(ArchiveInfo);
                return false;
            }
        }

        /// <summary>
        /// 获取到一个新的存档目录;
        /// </summary>
        static string GetNewArchiveDirectory()
        {
            string newDirectoryName = DateTime.Now.Ticks.ToString();
            string path = Path.Combine(ArchivesDirectory, newDirectoryName);
            for (uint i = 0; i < 500; i++)
            {
                string correctedPath = path;
                if (!File.Exists(correctedPath))
                {
                    return correctedPath;
                }
                else
                {
                    correctedPath = path + i;
                }
            }
            throw new IOException("无法提供随机文件目录;");
        }

        /// <summary>
        /// 迭代获取到所有存档文件;
        /// </summary>
        public static IEnumerable<Archive> EnumerateArchives()
        {
            return EnumerateArchives(ArchivesDirectory);
        }

        /// <summary>
        /// 迭代获取到所有存档文件;
        /// </summary>
        public static IEnumerable<Archive> EnumerateArchives(string directory)
        {
            foreach (var archiveDirectory in Directory.EnumerateDirectories(directory))
            {
                ArchiveInfo archiveInfo;
                try
                {
                    archiveInfo = archiveInfoSerializer.Deserialize(archiveDirectory);
                }
                catch
                {
                    continue;
                }
                yield return new Archive(archiveDirectory, archiveInfo);
            }
        }

        public override string ToString()
        {
            return base.ToString() + "[ArchiveDirectory:" + ArchiveDirectory + "]";
        }


        /// <summary>
        /// 根据时间升序的对比器;存档时间由早到晚,未创建的存档永远在最后;
        /// </summary>
        public class OrderByTimeAscendingComparer : Comparer<Archive>
        {
            public override int Compare(Archive x, Archive y)
            {
                if (x.Exists())
                {
                    if (y.Exists())
                    {
                        return (int)(x.Info.Time.Ticks - y.Info.Time.Ticks);
                    }
                    else
                    {
                        return -1;
                    }
                }
                else
                {
                    if (y.Exists())
                    {
                        return 1;
                    }
                    else
                    {
                        return -1;
                    }
                }
            }
        }

        /// <summary>
        /// 根据时间降序的对比器;存档时间由晚到早,未创建的存档永远在最后;;
        /// </summary>
        public class OrderByTimeDescendingComparer : Comparer<Archive>
        {
            public override int Compare(Archive x, Archive y)
            {
                if (x.Exists())
                {
                    if (y.Exists())
                    {
                        return (int)(y.Info.Time.Ticks - x.Info.Time.Ticks);
                    }
                    else
                    {
                        return -1;
                    }
                }
                else
                {
                    if (y.Exists())
                    {
                        return 1;
                    }
                    else
                    {
                        return -1;
                    }
                }
            }
        }
    }
}
