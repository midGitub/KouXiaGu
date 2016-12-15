using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;
using UniRx;

namespace KouXiaGu.Initialization
{

    /// <summary>
    /// 可以保存的;
    /// </summary>
    public interface IPreservable
    {
        IAsyncOperate OnSave(Archive archive);
    }

    /// <summary>
    /// 游戏存档管理;
    /// </summary>
    public class Archive
    {

        /// <summary>
        /// 存档保存到的文件夹;
        /// </summary>
        const string ARCHIVES_DIRECTORY_NAME = "Saves";

        /// <summary>
        /// 临时存档文件夹名,先将存档保存到此路径,保存完毕后再复制到真正的存档目录;
        /// </summary>
        const string TEMP_ARCHIVES_DIRECTORY_NAME = "Saves_Temp";

        /// <summary>
        /// 获取到保存所有存档的文件夹路径;
        /// </summary>
        public static string ArchivesPath
        {
            get { return Path.Combine(Application.persistentDataPath, ARCHIVES_DIRECTORY_NAME); }
        }

        /// <summary>
        /// 临时存档文件夹名,先将存档保存到此路径,保存完毕后再复制到真正的存档目录;
        /// </summary>
        public static string TempDirectoryPath
        {
            get { return Path.Combine(Application.temporaryCachePath, TEMP_ARCHIVES_DIRECTORY_NAME); }
        }

        /// <summary>
        /// 获取到所有存档文件夹下的所有文件夹路径;
        /// </summary>
        static IEnumerable<string> GetArchivedPaths()
        {
            string[] archivedsPath = Directory.GetDirectories(ArchivesPath);
            return archivedsPath;
        }

        /// <summary>
        /// 获取到随机的存档文件夹路径,未创建到磁盘;
        /// </summary>
        static string GetRandomDirectoryPath()
        {
            int appendNumber = 0;
            string archivedDirectoryPath = Path.Combine(ArchivesPath, Path.GetRandomFileName());

            while (Directory.Exists(archivedDirectoryPath))
            {
                archivedDirectoryPath += appendNumber.ToString();
            }

            return archivedDirectoryPath;
        }

        /// <summary>
        /// 获取到所有的存档;
        /// </summary>
        public static IEnumerable<Archive> GetArchives()
        {
            foreach (var path in GetArchivedPaths())
            {
                yield return new Archive(path);
            }
        }



        /// <summary>
        /// 保存游戏为新的存档;
        /// </summary>
        public static Archive Save()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 保存游戏为存档;
        /// </summary>
        public static void Save(Archive archive)
        {
            throw new NotImplementedException();
        }


        class ArchiveStages : Initializer
        {
            const GameStages DEPUTY = GameStages.Saving;
            const bool INSTANT = true;

            public GameStages Deputy
            {
                get { return DEPUTY; }
            }

            public bool Instant
            {
                get { return INSTANT; }
            }

            public bool Premise()
            {
                return Stages == GameStages.Game;
            }

            public IAsyncOperate OnEnter()
            {
                throw new NotImplementedException();
            }

            public IAsyncOperate OnLeave()
            {
                throw new NotImplementedException();
            }


            public IDisposable Subscribe(IPreservable observer)
            {
                throw new NotImplementedException();
            }

        }


        #region 实例部分;

        /// <summary>
        /// 存档文件夹路径;
        /// </summary>
        public string DirectoryPath { get; private set; }

        Archive()
        {
            DirectoryPath = GetRandomDirectoryPath();
        }

        Archive(string directoryPath)
        {
            this.DirectoryPath = directoryPath;
        }

        #endregion


    }



}
