using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace KouXiaGu
{

    [Serializable]
    public class ArchiveControl : IResourceInitialize<IArchived>
    {

        public ArchiveControl()
        {

        }

        [Header("存档信息"), SerializeField, Tooltip("保存到的文件夹")]
        private string archivedsDirectory;

        [SerializeField, Tooltip("预读取的存档名")]
        private string prefetchArchivedName;

        [SerializeField, Tooltip("存档名")]
        private string archivedName;

        [SerializeField, Tooltip("当前使用的存档版本")]
        private int version;

        ///// <summary>
        ///// 有效的存档版本;
        ///// </summary>
        //public int[] ValidVersion
        //{
        //    get { return validVersion; }
        //}

        public string GetArchivedsPath()
        {
            string archivedsPath = Path.Combine(Application.persistentDataPath, archivedsDirectory);
            return archivedsPath;
        }

        private string GetSmallArchivedFilePath(string archivedPath)
        {
            string mallArchivedFilePath = Path.Combine(archivedPath, prefetchArchivedName);
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


        public IEnumerable<ISmallArchived> GetSmallArchiveds()
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

        public IEnumerable<string> GetAllArchivedPath()
        {
            string archivedFilePath;
            string[] archivedsPath = Directory.GetDirectories(GetArchivedsPath());

            foreach (var archivedPath in archivedsPath)
            {
                if (TryGetArchivedFilePath(archivedPath, out archivedFilePath))
                    yield return archivedPath;
            }
        }

        private bool TryGetSmallArchived(string archivedPath, out SmallArchivedGroup smallArchived)
        {
            string smallArchivedFilePath;

            if (TryGetSmallArchivedFilePath(archivedPath, out smallArchivedFilePath))
            {
                SmallArchived archived = smallArchivedFilePath.Deserialize_ProtoBuf<SmallArchived>();
                smallArchived = new SmallArchivedGroup(archived, archivedPath);
                return true;
            }
            smallArchived = default(SmallArchivedGroup);
            return false;
        }

        private bool TryGetArchived(string archivedPath, out ArchivedGroup archived)
        {
            string archivedFilePath;

            if (TryGetSmallArchivedFilePath(archivedPath, out archivedFilePath))
            {
                ArchivedExpand archivedExpand = archivedFilePath.Deserialize_ProtoBuf<ArchivedExpand>();
                archived = new ArchivedGroup(archivedExpand, archivedPath);
                return true;
            }
            archived = default(ArchivedGroup);
            return false;
        }

        public IArchived SmallArchivedTransfrom(ISmallArchived smallArchived)
        {
            ArchivedGroup archivedGroup;
            if (smallArchived is IArchived)
            {
                return smallArchived as IArchived;
            }
            else if (TryGetArchived(smallArchived.ArchivedPath, out archivedGroup))
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
        public IArchived GetNewArchived()
        {
            ArchivedExpand archivedExpand = new ArchivedExpand();
            string archivedDirectoryPath = GetNewArchivedDirectoryPath();
            ArchivedGroup archivedGroup = new ArchivedGroup(archivedExpand, archivedDirectoryPath);
            return archivedGroup;
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
        /// 将这个存档保存到硬盘上(持久化保存);
        /// </summary>
        /// <param name="archived"></param>
        public void SaveInDisk(IArchived archived)
        {
            Directory.CreateDirectory(archived.ArchivedPath);

            string archivedFilePath = GetArchivedFilePath(archived.ArchivedPath);
            string smallArchivedFilePath = GetSmallArchivedFilePath(archived.ArchivedPath);

            SetArchivedInfo(archived.Archived);

            archived.Archived.Serialize_ProtoBuf(archivedFilePath);
            ((SmallArchived)archived.Archived).Serialize_ProtoBuf(smallArchivedFilePath);
        }

        /// <summary>
        /// 设置这个存档的存档信息部分;
        /// </summary>
        private void SetArchivedInfo(ArchivedExpand archived)
        {
            archived.SavedTime = DateTime.Now.Ticks;
            archived.Version = version;
        }

        #region IResourceInitialize<T>;

        [SerializeField]
        private ArchiveInitialize initialize;

        public bool IsDisposed
        {
            get { return this.initialize.IsDisposed; }
        }

        public void Dispose()
        {
            this.initialize.Dispose();
        }

        public IEnumerator Start(IArchived item, Action<Exception> onError, Action onInitialized, Action<Exception> onFail)
        {
            onInitialized = () => OnInitialized(item, onError, onInitialized, onFail);
            return this.initialize.Start(item, onError, onInitialized, onFail);
        }

        private void OnInitialized(IArchived item, Action<Exception> onError, Action onInitialized, Action<Exception> onFail)
        {
            try
            {
                SaveInDisk(item);
                onInitialized();
            }
            catch (Exception e)
            {
                onFail(e);
            }
        }

        #endregion


        private struct SmallArchivedGroup : ISmallArchived
        {
            public SmallArchivedGroup(SmallArchived archived, string archivedPath)
            {
                this.SmallArchived = archived;
                this.ArchivedPath = archivedPath;
            }
            public SmallArchived SmallArchived { get; private set; }
            public string ArchivedPath { get; private set; }
        }

        private struct ArchivedGroup : IArchived, ISmallArchived
        {
            public ArchivedGroup(ArchivedExpand archived, string archivedPath)
            {
                this.Archived = archived;
                this.ArchivedPath = archivedPath;
            }
            public ArchivedExpand Archived { get; private set; }
            public SmallArchived SmallArchived { get { return Archived; } }
            public string ArchivedPath { get; private set; }
        }

    }

    public interface ISmallArchived
    {
        SmallArchived SmallArchived { get; }
        string ArchivedPath { get; }
    }

    /// <summary>
    /// 存储游戏接口;
    /// </summary>
    public interface IArchived
    {
        ArchivedExpand Archived { get; }
        string ArchivedPath { get; }
    }

    [Serializable]
    internal sealed class ArchiveInitialize : ResourceInitialize<IArchiveInCoroutine, IArchiveInThread, IArchived>
    {
        public ArchiveInitialize() : base()
        {

        }

        [SerializeField]
        private GameObject BaseComponents;

        protected override IEnumerable<IArchiveInCoroutine> LoadInCoroutineComponents
        {
            get { return BaseComponents.GetComponentsInChildren<IArchiveInCoroutine>(); }
        }

        protected override IEnumerable<IArchiveInThread> LoadInThreadComponents
        {
            get { return BaseComponents.GetComponentsInChildren<IArchiveInThread>(); }
        }

    }

    public interface IArchiveInCoroutine : ICoroutineInitialize<IArchived> { }
    public interface IArchiveInThread : IThreadInitialize<IArchived> { }

}
