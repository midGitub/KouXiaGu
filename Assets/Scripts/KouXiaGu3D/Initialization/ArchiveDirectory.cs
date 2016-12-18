﻿using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace KouXiaGu.Initialization
{

    /// <summary>
    /// 游戏存档管理;
    /// </summary>
    public class ArchiveDirectory
    {

        /// <summary>
        /// 存档保存到的文件夹;
        /// </summary>
        const string ARCHIVES_DIRECTORY_NAME = "Saves";

        /// <summary>
        /// 获取到保存所有存档的文件夹路径;
        /// </summary>
        static string ArchivesDirectory
        {
            get { return Path.Combine(Application.persistentDataPath, ARCHIVES_DIRECTORY_NAME); }
        }

        /// <summary>
        /// 获取到所有存档文件夹下的所有文件夹路径;
        /// </summary>
        static IEnumerable<string> GetArchivedPaths()
        {
            string[] archivedsPath = Directory.GetDirectories(ArchivesDirectory);
            return archivedsPath;
        }

        /// <summary>
        /// 获取到一个随机的文件名;
        /// </summary>
        static string GetRandomDirectoryName()
        {
            return Path.GetRandomFileName();
        }

        /// <summary>
        /// 获取到所有的存档;
        /// </summary>
        public static IEnumerable<ArchiveDirectory> GetArchives()
        {
            foreach (var path in GetArchivedPaths())
            {
                yield return new ArchiveDirectory(path, true);
            }
        }

        #region 实例部分;


        /// <summary>
        /// 存档文件夹路径;
        /// </summary>
        public string DirectoryPath { get; private set; }

        /// <summary>
        /// 是否允许编辑?
        /// </summary>
        public bool AllowEdit { get; private set; }


        /// <summary>
        /// 创建一个新的存档,自行调用 Create() 创建文件夹;
        /// </summary>
        public ArchiveDirectory()
        {
            DirectoryPath = Path.Combine(ArchivesDirectory, GetRandomDirectoryName());
            AllowEdit = true;
        }

        /// <summary>
        /// 指定目录创建一个允许编辑的存档,自动调用 Create();
        /// </summary>
        /// <param name="directoryPath"></param>
        public ArchiveDirectory(string directoryPath)
        {
            this.DirectoryPath = directoryPath;
            AllowEdit = true;
            Create();
        }

        /// <summary>
        /// 指定目录创建一个存档,自行调用 Create() 创建文件夹;
        /// </summary>
        /// <param name="directoryPath">存档路径</param>
        /// <param name="allowEdit">是否允许编辑?</param>
        public ArchiveDirectory(string directoryPath, bool allowEdit)
        {
            this.DirectoryPath = directoryPath;
            this.AllowEdit = allowEdit;
        }


        /// <summary>
        /// 创建存档目录到磁盘;
        /// </summary>
        public void Create()
        {
            AuthorizationCheck();

            if (!Directory.Exists(DirectoryPath))
                Directory.CreateDirectory(DirectoryPath);
        }

        /// <summary>
        /// 销毁磁盘上的存档内容;
        /// </summary>
        public void Destroy()
        {
            AuthorizationCheck();

            Directory.Delete(DirectoryPath, true);
        }

        /// <summary>
        /// 当保存完成时调用;
        /// </summary>
        public void OnComplete()
        {
            AuthorizationCheck();

            Debug.Log("保存完成;路径:" + DirectoryPath);
            return;
        }

        /// <summary>
        /// 若为不允许编辑的存档则返回异常 UnauthorizedAccessException;
        /// </summary>
        void AuthorizationCheck()
        {
            if (!AllowEdit)
                throw new UnauthorizedAccessException("不允许编辑的存档内容;");
        }

        #endregion

    }

}