using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace KouXiaGu
{

    /// <summary>
    /// 游戏归档文件管理;
    /// </summary>
    [Serializable]
    public sealed class ArchiveData
    {
        private ArchiveData() { }

        [Header("存档信息"), SerializeField, Tooltip("保存到的文件夹")]
        private string archivedsDirectory;

        [SerializeField, Tooltip("预读取的存档名")]
        private string smallArchivedName;

        [SerializeField, Tooltip("存档名")]
        private string archivedName;

        [SerializeField, Tooltip("当前使用的存档版本")]
        private float version;

        /// <summary>
        /// 获取到所有存档的预读信息;
        /// </summary>
        /// <returns></returns>
        public IEnumerable<ArchivedHeadGroup> GetSmallArchiveds()
        {
            ArchivedHeadGroup smallArchived;
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
        /// 将存档预读信息转换成存档文件;
        /// </summary>
        /// <param name="smallArchived"></param>
        /// <returns></returns>
        public ArchivedGroup SmallArchivedTransfrom(ArchivedHeadGroup smallArchived)
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
        /// <returns></returns>
        public ArchivedGroup CreateArchived()
        {
            ArchivedExpand archivedExpand = new ArchivedExpand();
            string archivedDirectoryPath = GetNewArchivedDirectoryPath();
            ArchivedGroup archivedGroup = new ArchivedGroup(archivedExpand, archivedDirectoryPath);
            return archivedGroup;
        }

        /// <summary>
        /// 将这个存档保存到硬盘上(持久化保存);
        /// </summary>
        /// <param name="archived"></param>
        public void SaveInDisk(ArchivedGroup archived)
        {
            Directory.CreateDirectory(archived.ArchivedPath);

            string archivedFilePath = GetArchivedFilePath(archived.ArchivedPath);
            string smallArchivedFilePath = GetSmallArchivedFilePath(archived.ArchivedPath);

            SetArchivedInfo(archived.Archived);

            SerializeHelper.Serialize_ProtoBuf(archivedFilePath, (ArchivedExpand)archived.Archived);
            SerializeHelper.Serialize_ProtoBuf(smallArchivedFilePath, (ArchivedHead)archived.Archived);
        }

        /// <summary>
        /// 获取到所有存档文件夹下的所有文件夹路径,不做检查;
        /// </summary>
        /// <returns></returns>
        private IEnumerable<string> GetAllArchivedPath()
        {
            string[] archivedsPath = Directory.GetDirectories(GetArchivedsPath());

            foreach (var archivedPath in archivedsPath)
            {
                yield return archivedPath;
            }
        }

        private bool TryGetSmallArchived(string archivedPath, out ArchivedHeadGroup smallArchived)
        {
            string smallArchivedFilePath;

            if (TryGetSmallArchivedFilePath(archivedPath, out smallArchivedFilePath))
            {
                ArchivedHead archived = SerializeHelper.Deserialize_ProtoBuf<ArchivedHead>(smallArchivedFilePath);
                smallArchived = new ArchivedHeadGroup(archived, archivedPath);
                return true;
            }
            smallArchived = default(ArchivedHeadGroup);
            return false;
        }

        private bool TryGetArchived(string archivedPath, out ArchivedGroup archived)
        {
            string archivedFilePath;

            if (TryGetSmallArchivedFilePath(archivedPath, out archivedFilePath))
            {
                ArchivedExpand archivedExpand = SerializeHelper.Deserialize_ProtoBuf<ArchivedExpand>(archivedFilePath);
                archived = new ArchivedGroup(archivedExpand, archivedPath);
                return true;
            }
            archived = default(ArchivedGroup);
            return false;
        }


        /// <summary>
        /// 获取到一个新的存档存放文件夹路径;
        /// </summary>
        /// <returns></returns>
        private string GetNewArchivedDirectoryPath()
        {
            string archivedDirectoryPath = Path.Combine(GetArchivedsPath(), DateTime.Now.Ticks.ToString());
            return archivedDirectoryPath;
        }

        /// <summary>
        /// 设置这个存档的存档信息部分;
        /// </summary>
        private void SetArchivedInfo(ArchivedExpand archived)
        {
            archived.SavedTime = DateTime.Now.Ticks;
            archived.Version = version;
        }

        private string GetArchivedsPath()
        {
            string archivedsPath = Path.Combine(Application.persistentDataPath, archivedsDirectory);
            return archivedsPath;
        }

        private string GetSmallArchivedFilePath(string archivedPath)
        {
            string mallArchivedFilePath = Path.Combine(archivedPath, smallArchivedName);
            return mallArchivedFilePath;
        }

        private string GetArchivedFilePath(string archivedPath)
        {
            string archivedFilePath = Path.Combine(archivedPath, archivedName);
            return archivedFilePath;
        }

        private bool TryGetSmallArchivedFilePath(string archivedPath, out string smallArchivedFilePath)
        {
            smallArchivedFilePath = GetSmallArchivedFilePath(archivedPath);
            return File.Exists(smallArchivedFilePath);
        }

        private bool TryGetArchivedFilePath(string archivedPath, out string archivedFilePath)
        {
            archivedFilePath = GetArchivedFilePath(archivedPath);
            return File.Exists(archivedFilePath);
        }

    }

    public struct ArchivedHeadGroup
    {
        public ArchivedHeadGroup(ArchivedHead archivedHead, string archivedPath)
        {
            this.ArchivedHead = archivedHead;
            this.ArchivedPath = archivedPath;
        }
        public ArchivedHead ArchivedHead { get; private set; }
        public string ArchivedPath { get; private set; }
    }

    public struct ArchivedGroup 
    {
        public ArchivedGroup(ArchivedExpand archived, string archivedPath)
        {
            this.Archived = archived;
            this.ArchivedPath = archivedPath;
        }
        public ArchivedExpand Archived { get; private set; }
        public ArchivedHead SmallArchived { get { return Archived; } }
        public string ArchivedPath { get; private set; }

        public static implicit operator ArchivedHeadGroup(ArchivedGroup archived)
        {
            return new ArchivedHeadGroup(archived.Archived, archived.ArchivedPath);
        }
    }

}
