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
        IEnumerator OnSave(Archiver archive);
    }


    /// <summary>
    /// 游戏存档管理;
    /// </summary>
    public class Archiver
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
        /// 获取到所有的存档路径;
        /// </summary>
        public static IEnumerable<Archiver> GetArchives()
        {
            foreach (var path in GetArchivedPaths())
            {
                yield return new Archiver(path);
            }
        }

        #region 实例部分;

        /// <summary>
        /// 存档文件夹路径;
        /// </summary>
        public string DirectoryPath { get; private set; }

        /// <summary>
        /// 创建一个新的存档;
        /// </summary>
        public Archiver()
        {
            DirectoryPath = GetRandomDirectoryPath();
        }

        Archiver(string directoryPath)
        {
            this.DirectoryPath = directoryPath;
        }

        #endregion

    }


    public class ArchiveStages : IPeriod
    {
        ArchiveStages() { }

        const GameStages DEPUTY = GameStages.Saving;
        const bool INSTANT = true;

        static readonly ArchiveStages instance = new ArchiveStages();
        static readonly HashSet<IPreservable> observerSet = new HashSet<IPreservable>();
        static readonly Queue<IEnumerator> coroutines = new Queue<IEnumerator>();

        static Archiver archive;


        GameStages IPeriod.Deputy
        {
            get { return DEPUTY; }
        }

        bool IPeriod.Instant
        {
            get { return INSTANT; }
        }

        bool IPeriod.Premise()
        {
            return true;
            return Initializer.Stages == GameStages.Game;
        }

        IEnumerator IPeriod.OnEnter()
        {
            IEnumerator coroutine;
            Queue<IEnumerator> coroutineQueue = InitCoroutineQueue();

            while (coroutineQueue.Count != 0)
            {
                coroutine = coroutineQueue.Dequeue();

                while (coroutine.MoveNext())
                {
                    yield return null;
                }
            }

            yield break;
        }

        Queue<IEnumerator> InitCoroutineQueue()
        {
            coroutines.Clear();

            IEnumerator item;
            foreach (var source in observerSet)
            {
                item = source.OnSave(archive);
                coroutines.Enqueue(item);
            }

            return coroutines;
        }

        /// <summary>
        /// 不需要实现
        /// </summary>
        IEnumerator IPeriod.OnLeave()
        {
            throw new NotImplementedException();
        }


        /// <summary>
        /// 订阅到这个状态;
        /// </summary>
        public static bool Subscribe(IPreservable observer)
        {
            return observerSet.Add(observer);
        }

        public static bool Unsubscribe(IPreservable observer)
        {
            return observerSet.Remove(observer);
        }


        /// <summary>
        /// 保存游戏为新的存档;
        /// </summary>
        public static Archiver Save()
        {
            ArchiveStages.archive = new Archiver();
            Initializer.Add(instance);
            return archive;
        }

        /// <summary>
        /// 保存游戏到存档;
        /// </summary>
        public static void Save(Archiver archive)
        {
            ArchiveStages.archive = archive;
            Initializer.Add(instance);
        }


    }


}
