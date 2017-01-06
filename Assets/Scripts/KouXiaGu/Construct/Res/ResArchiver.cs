using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;

namespace KouXiaGu
{


    /// <summary>
    /// 预读存档 + 存档路径;
    /// </summary>
    public struct SmallArchivedGroup
    {
        public SmallArchivedGroup(bool fromFile, SmallArchived archivedHead, string archivedPath)
        {
            this.ArchivedHead = archivedHead;
            this.ArchivedPath = archivedPath;
            this.FromFile = fromFile;
        }
        public SmallArchived ArchivedHead { get; private set; }
        public bool FromFile { get; private set; }
        public string ArchivedPath { get; private set; }
    }

    /// <summary>
    /// 完整存档 + 存档路径;
    /// </summary>
    public struct ArchivedGroup
    {
        public ArchivedGroup(bool fromFile, ArchivedExpand archived, string archivedPath)
        {
            this.FromFile = fromFile;
            this.Archived = archived;
            this.ArchivedPath = archivedPath;
        }
        public ArchivedExpand Archived { get; private set; }
        public SmallArchived SmallArchived { get { return Archived; } }
        public bool FromFile { get; private set; }
        public string ArchivedPath { get; private set; }

        public static implicit operator SmallArchivedGroup(ArchivedGroup archived)
        {
            return new SmallArchivedGroup(archived.FromFile, archived.Archived, archived.ArchivedPath);
        }
    }


    public class ResArchiver
    {

        /// <summary>
        /// 存档保存到的文件夹;
        /// </summary>
        const string archivedsDirectory = "Saves";

        /// <summary>
        /// 预读取的存档名
        /// </summary>
        const string smallArchivedName = "Small.save";

        /// <summary>
        /// 存档名
        /// </summary>
        const string archivedName = "Save.save";

        /// <summary>
        /// 获取到保存所有存档的文件夹路径;
        /// </summary>
        public static string ArchivedsPath
        {
            get { return Path.Combine(Application.persistentDataPath, archivedsDirectory); }
        }


        /// <summary>
        /// 获取到所有存档的预读信息;
        /// </summary>
        public static IEnumerable<SmallArchivedGroup> GetSmallArchiveds()
        {
            SmallArchivedGroup smallArchived;
            ArchivedGroup archived;

            foreach (var archivedPath in GetAllArchivedPath())
            {
                if (TryGetSmallArchived(archivedPath, out smallArchived))
                {
                    yield return smallArchived;
                }
                else if (TryGetArchived(archivedPath, out archived))
                {
                    yield return archived;
                }
            }
        }

        /// <summary>
        /// 获取到最近保存的存档;
        /// 若不存在存档则返回异常 InvalidOperationException;
        /// </summary>
        public static ArchivedGroup GetRecentArchivedGroup()
        {
            SmallArchivedGroup smallArchivedGroup = GetRecentSmallArchivedGroup();
            return SmallArchivedTransfrom(smallArchivedGroup);
        }

        /// <summary>
        /// 获取到最近保存的存档;
        /// 若不存在存档则返回异常 InvalidOperationException;
        /// </summary>
        public static SmallArchivedGroup GetRecentSmallArchivedGroup()
        {
            SmallArchivedGroup smallArchivedGroup = GetSmallArchiveds().
                OrderByDescending(value => value.ArchivedHead.SavedTime).
                First();

            return smallArchivedGroup;
        }

        /// <summary>
        /// 将存档预读信息转换成存档文件;
        /// </summary>
        public static ArchivedGroup SmallArchivedTransfrom(SmallArchivedGroup smallArchived)
        {
            ArchivedGroup archivedGroup;
            if (TryGetArchived(smallArchived.ArchivedPath, out archivedGroup))
            {
                return archivedGroup;
            }
            else
            {
                throw new FileNotFoundException("存档损坏;");
            }
        }

        /// <summary>
        /// 获取到一个新的存档实例(未在磁盘上创建);
        /// </summary>
        public static ArchivedGroup CreateArchived()
        {
            ArchivedExpand archivedExpand = new ArchivedExpand();
            string archivedDirectoryPath = GetNewArchivedDirectoryPath();
            ArchivedGroup archivedGroup = new ArchivedGroup(false, archivedExpand, archivedDirectoryPath);
            return archivedGroup;
        }

        /// <summary>
        /// 将这个存档保存到硬盘上(持久化保存);
        /// </summary>
        public static void SaveInDisk(ArchivedGroup archived)
        {
            Directory.CreateDirectory(archived.ArchivedPath);

            string archivedFilePath = GetArchivedFilePath(archived.ArchivedPath);
            string smallArchivedFilePath = GetSmallArchivedFilePath(archived.ArchivedPath);

            SetArchivedInfo(archived.Archived);

            ProtoBufExtensions.SerializeProtoBuf(archivedFilePath, (ArchivedExpand)archived.Archived);
            ProtoBufExtensions.SerializeProtoBuf(smallArchivedFilePath, (SmallArchived)archived.Archived);
        }

        ///// <summary>
        ///// 获取到保存所有存档的文件夹路径;
        ///// </summary>
        //public static string GetArchivedsPath()
        //{
        //    string archivedsPath = Path.Combine(Application.persistentDataPath, archivedsDirectory);
        //    return archivedsPath;
        //}

        /// <summary>
        /// 获取到所有存档文件夹下的所有文件夹路径,不做检查;
        /// </summary>
        static IEnumerable<string> GetAllArchivedPath()
        {
            string[] archivedsPath = Directory.GetDirectories(ArchivedsPath);

            foreach (var archivedPath in archivedsPath)
            {
                yield return archivedPath;
            }
        }

        /// <summary>
        /// 尝试获取到这个存档路径内的预读存档;
        /// 若不存在完整的存档,也算不存在;
        /// </summary>
        static bool TryGetSmallArchived(string archivedPath, out SmallArchivedGroup smallArchived)
        {
            string smallArchivedFilePath;

            if (ExistArchivedFile(archivedPath) && TryGetSmallArchivedFilePath(archivedPath, out smallArchivedFilePath))
            {
                SmallArchived archived = ProtoBufExtensions.DeserializeProtoBuf<SmallArchived>(smallArchivedFilePath);
                smallArchived = new SmallArchivedGroup(true, archived, archivedPath);
                return true;
            }
            smallArchived = default(SmallArchivedGroup);
            return false;
        }

        /// <summary>
        /// 尝试获取到这个存档路径内的存档;
        /// </summary>
        static bool TryGetArchived(string archivedPath, out ArchivedGroup archived)
        {
            string archivedFilePath;

            if (TryGetSmallArchivedFilePath(archivedPath, out archivedFilePath))
            {
                ArchivedExpand archivedExpand = ProtoBufExtensions.DeserializeProtoBuf<ArchivedExpand>(archivedFilePath);
                archived = new ArchivedGroup(true, archivedExpand, archivedPath);
                return true;
            }
            archived = default(ArchivedGroup);
            return false;
        }


        /// <summary>
        /// 获取到一个新的存档存放文件夹路径;
        /// </summary>
        /// <returns></returns>
        static string GetNewArchivedDirectoryPath()
        {
            int appendNumber = 0;
            string archivedDirectoryPath = Path.Combine(ArchivedsPath, DateTime.Now.Ticks.ToString());

            while (Directory.Exists(archivedDirectoryPath))
            {
                archivedDirectoryPath += appendNumber.ToString();
            }

            return archivedDirectoryPath;
        }

        /// <summary>
        /// 设置这个存档的存档信息部分;
        /// </summary>
        static void SetArchivedInfo(ArchivedExpand archived)
        {
            archived.SavedTime = DateTime.Now.Ticks;
        }

        /// <summary>
        /// 从存档路径获取到预读的存档文件路径;
        /// </summary>
        static string GetSmallArchivedFilePath(string archivedPath)
        {
            string mallArchivedFilePath = Path.Combine(archivedPath, smallArchivedName);
            return mallArchivedFilePath;
        }

        /// <summary>
        /// 从存档路径获取到存档文件路径;
        /// </summary>
        static string GetArchivedFilePath(string archivedPath)
        {
            string archivedFilePath = Path.Combine(archivedPath, archivedName);
            return archivedFilePath;
        }

        /// <summary>
        /// 尝试从存档路径获取到预读的存档文件路径;
        /// </summary>
        static bool TryGetSmallArchivedFilePath(string archivedPath, out string smallArchivedFilePath)
        {
            smallArchivedFilePath = GetSmallArchivedFilePath(archivedPath);
            return File.Exists(smallArchivedFilePath);
        }

        /// <summary>
        /// 尝试从存档路径获取到存档文件路径;
        /// </summary>
        static bool TryGetArchivedFilePath(string archivedPath, out string archivedFilePath)
        {
            archivedFilePath = GetArchivedFilePath(archivedPath);
            return File.Exists(archivedFilePath);
        }

        /// <summary>
        /// 这个路径下是否存在完整存档文件;
        /// </summary>
        static bool ExistArchivedFile(string archivedPath)
        {
            string archivedFilePath = GetArchivedFilePath(archivedPath);
            return File.Exists(archivedFilePath);
        }

    }

}
