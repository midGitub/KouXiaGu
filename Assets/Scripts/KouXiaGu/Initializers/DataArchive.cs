using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UniRx;
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


    ///// <summary>
    ///// 存档获取方法;
    ///// </summary>
    //public interface IArchiveData
    //{
    //    /// <summary>
    //    /// 获取到保存所有存档的文件夹路径;
    //    /// </summary>
    //    string GetArchivedsPath();

    //    /// <summary>
    //    /// 创建一个空存档信息,并且返回(未持久化保存);
    //    /// </summary>
    //    ArchivedGroup CreateArchived();

    //    /// <summary>
    //    /// 获取到所有有效的预读存档信息;
    //    /// </summary>
    //    IEnumerable<SmallArchivedGroup> GetSmallArchiveds();

    //    /// <summary>
    //    /// 将这个存档保存到磁盘上(持久化保存)
    //    /// </summary>
    //    /// <param name="archived"></param>
    //    void SaveInDisk(ArchivedGroup archived);

    //    /// <summary>
    //    /// 将预读存档文件转换成完整存档;
    //    /// </summary>
    //    ArchivedGroup SmallArchivedTransfrom(SmallArchivedGroup smallArchived);
    //}

    /// <summary>
    /// 游戏归档管理;
    /// </summary>
    [Serializable]
    public sealed class DataArchive
    {
        private DataArchive() { }

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
        public IEnumerable<SmallArchivedGroup> GetSmallArchiveds()
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
        /// 将存档预读信息转换成存档文件;
        /// </summary>
        public ArchivedGroup SmallArchivedTransfrom(SmallArchivedGroup smallArchived)
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
        public ArchivedGroup CreateArchived()
        {
            ArchivedExpand archivedExpand = new ArchivedExpand();
            string archivedDirectoryPath = GetNewArchivedDirectoryPath();
            ArchivedGroup archivedGroup = new ArchivedGroup(false, archivedExpand, archivedDirectoryPath);
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
            SerializeHelper.Serialize_ProtoBuf(smallArchivedFilePath, (SmallArchived)archived.Archived);
        }

        /// <summary>
        /// 获取到保存所有存档的文件夹路径;
        /// </summary>
        public string GetArchivedsPath()
        {
            string archivedsPath = Path.Combine(Application.persistentDataPath, archivedsDirectory);
            return archivedsPath;
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

        /// <summary>
        /// 尝试获取到这个存档路径内的预读存档;
        /// 若不存在完整的存档,也算不存在;
        /// </summary>
        private bool TryGetSmallArchived(string archivedPath, out SmallArchivedGroup smallArchived)
        {
            string smallArchivedFilePath;

            if (ExistArchivedFile(archivedPath) && TryGetSmallArchivedFilePath(archivedPath, out smallArchivedFilePath))
            {
                SmallArchived archived = SerializeHelper.Deserialize_ProtoBuf<SmallArchived>(smallArchivedFilePath);
                smallArchived = new SmallArchivedGroup(true, archived, archivedPath);
                return true;
            }
            smallArchived = default(SmallArchivedGroup);
            return false;
        }

        /// <summary>
        /// 尝试获取到这个存档路径内的存档;
        /// </summary>
        private bool TryGetArchived(string archivedPath, out ArchivedGroup archived)
        {
            string archivedFilePath;

            if (TryGetSmallArchivedFilePath(archivedPath, out archivedFilePath))
            {
                ArchivedExpand archivedExpand = SerializeHelper.Deserialize_ProtoBuf<ArchivedExpand>(archivedFilePath);
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

        /// <summary>
        /// 从存档路径获取到预读的存档文件路径;
        /// </summary>
        private string GetSmallArchivedFilePath(string archivedPath)
        {
            string mallArchivedFilePath = Path.Combine(archivedPath, smallArchivedName);
            return mallArchivedFilePath;
        }

        /// <summary>
        /// 从存档路径获取到存档文件路径;
        /// </summary>
        private string GetArchivedFilePath(string archivedPath)
        {
            string archivedFilePath = Path.Combine(archivedPath, archivedName);
            return archivedFilePath;
        }

        /// <summary>
        /// 尝试从存档路径获取到预读的存档文件路径;
        /// </summary>
        private bool TryGetSmallArchivedFilePath(string archivedPath, out string smallArchivedFilePath)
        {
            smallArchivedFilePath = GetSmallArchivedFilePath(archivedPath);
            return File.Exists(smallArchivedFilePath);
        }

        /// <summary>
        /// 尝试从存档路径获取到存档文件路径;
        /// </summary>
        private bool TryGetArchivedFilePath(string archivedPath, out string archivedFilePath)
        {
            archivedFilePath = GetArchivedFilePath(archivedPath);
            return File.Exists(archivedFilePath);
        }

        /// <summary>
        /// 这个路径下是否存在完整存档文件;
        /// </summary>
        private bool ExistArchivedFile(string archivedPath)
        {
            string archivedFilePath = GetArchivedFilePath(archivedPath);
            return File.Exists(archivedFilePath);
        }

    }

}
