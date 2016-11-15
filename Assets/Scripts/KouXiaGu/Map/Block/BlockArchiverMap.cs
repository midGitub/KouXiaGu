using System;
using System.Collections.Generic;
using System.IO;
using UniRx;
using UnityEngine;

namespace KouXiaGu.Map
{

    /// <summary>
    /// 地图初始化和归档方法;
    /// </summary>
    public class BlockArchiverMap<T> : MapBlockIO<T>, IBuildGameInThread, IArchiveInThread, IQuitInThread
    {
        public BlockArchiverMap(MapBlockIOInfo mapBlockIOInfo, BlocksMapInfo blocksMapInfo) : base(blocksMapInfo)
        {
            this.fullArchiveTempDirectoryPath = GetFullArchiveTempDirectoryPath(mapBlockIOInfo.archiveTempDirectoryPath);
            this.addressPrefix = mapBlockIOInfo.addressPrefix;
            this.archivedDirectoryPath = mapBlockIOInfo.archivedDirectoryPath;
        }

        /// <summary>
        /// 完整的预制地图文件夹路径(存档提供);
        /// </summary>
        private string fullprefabMapDirectoryPath;
        /// <summary>
        /// 零时存放归档地图路径(预定义);
        /// </summary>
        private string fullArchiveTempDirectoryPath;
        /// <summary>
        /// 地图块前缀(预定义);
        /// </summary>
        private string addressPrefix;
        /// <summary>
        /// 保存到存档的位置(预定义);
        /// </summary>
        private string archivedDirectoryPath;


        protected string ArchivedSearchPattern
        {
            get{ return addressPrefix + "*"; }
        }


        /// <summary>
        /// 获取到完整的预制地图文件夹路径;
        /// </summary>
        protected string GetFullPrefabMapDirectoryPath()
        {
            return this.fullprefabMapDirectoryPath;
        }

        /// <summary>
        /// 获取到完整的预制地图文件路径;
        /// </summary>
        protected override string GetFullPrefabMapFilePath(ShortVector2 address)
        {
            string fullPrefabMapDirectoryPath = GetFullPrefabMapDirectoryPath();
            string blockName = GetBlockName(address);
            string fullPrefabMapFilePath = Path.Combine(fullPrefabMapDirectoryPath, blockName);
            return fullPrefabMapFilePath;
        }

        /// <summary>
        /// 获取到完整的存档缓存地图文件夹路径;
        /// </summary>
        protected string GetFullArchiveTempDirectoryPath()
        {
            return this.fullArchiveTempDirectoryPath;
        }

        /// <summary>
        /// 获取到完整的存档缓存地图文件夹路径;
        /// </summary>
        protected string GetFullArchiveTempDirectoryPath(string archiveTempDirectoryPath)
        {
            string fullArchiveTempirectoryPath = Path.Combine(Application.dataPath, archiveTempDirectoryPath);
            return fullArchiveTempirectoryPath;
        }

        /// <summary>
        /// 获取到完整的存档缓存地图文件路径;
        /// </summary>
        protected override string GetFullArchiveTempFilePath(ShortVector2 address)
        {
            string fullArchiveTempirectoryPath = GetFullArchiveTempDirectoryPath();
            string blockName = GetBlockName(address);
            string fullArchiveTempFilePath = Path.Combine(fullArchiveTempirectoryPath, blockName);
            return fullArchiveTempFilePath;
        }

        /// <summary>
        /// 获取到地图块名称;
        /// </summary>
        protected string GetBlockName(ShortVector2 address)
        {
            return addressPrefix + address.GetHashCode();
        }

        /// <summary>
        /// 获取到完整的归档地图文件夹路径;
        /// </summary>
        protected string GetFullArchivedDirectoryPath(ArchivedGroup item)
        {
            string fullArchivedDirectoryPath = Path.Combine(item.ArchivedPath, archivedDirectoryPath);
            return fullArchivedDirectoryPath;
        }


        void IThreadInitialize<BuildGameData>.Initialize(
            BuildGameData item, ICancelable cancelable, Action<Exception> onError, Action runningDoneCallBreak)
        {
            try
            {
                Debug.Log(this + "开始初始化!");
                RecoveryFile(item, cancelable);
                RecoveryData(item, cancelable);
                RecoveryMap(item, cancelable);
            }
            catch (Exception e)
            {
                onError(e);
            }
            runningDoneCallBreak();
        }

        /// <summary>
        /// 将存档的归档地图拷贝到缓存地图文件夹下;
        /// </summary>
        private void RecoveryFile(ArchivedGroup item, ICancelable cancelable)
        {
            if (item.FromFile)
            {
                string fullArchivedDirectoryPath = GetFullArchivedDirectoryPath(item);
                string fullTempArchiveDirectoryPath = GetFullArchiveTempDirectoryPath();

                CopyDirectory(cancelable, fullArchivedDirectoryPath, fullTempArchiveDirectoryPath, ArchivedSearchPattern);
            }
        }

        /// <summary>
        /// 获取到保存预制地图路径;
        /// </summary>
        private void RecoveryData(ArchivedGroup item, ICancelable cancelable)
        {
            try
            {
                this.fullprefabMapDirectoryPath = item.Archived.Map.PathPrefabMapDirectory;
            }
            catch (KeyNotFoundException e)
            {
                throw new KeyNotFoundException("未定义地图!", e);
            }
        }

        /// <summary>
        /// 开始更新地图,初始化第一个点;
        /// </summary>
        private void RecoveryMap(ArchivedGroup item, ICancelable cancelable)
        {
            IntVector2 protagonistMapPosition = item.Archived.GetProtagonistMapPosition();
            UpdateMapData(protagonistMapPosition, false);
        }

        void IThreadInitialize<ArchivedGroup>.Initialize(
            ArchivedGroup item, ICancelable cancelable, Action<Exception> onError, Action runningDoneCallBreak)
        {
            try
            {
                Debug.Log(this + "开始归档到!");
                ArchiveMap(item, cancelable);
                ArchiveFile(item, cancelable);
                ArchiveData(item, cancelable);
            }
            catch (Exception e)
            {
                onError(e);
            }
            runningDoneCallBreak();
        }

        private void ArchiveMap(ArchivedGroup item, ICancelable cancelable)
        {
            SaveBlocks();
        }

        private void ArchiveFile(ArchivedGroup item, ICancelable cancelable)
        {
            string fullArchivedDirectoryPath = GetFullArchivedDirectoryPath(item);
            string fullTempArchiveDirectoryPath = GetFullArchiveTempDirectoryPath();

            CopyDirectory(cancelable, fullTempArchiveDirectoryPath, fullArchivedDirectoryPath, ArchivedSearchPattern);
        }

        private void ArchiveData(ArchivedGroup item, ICancelable cancelable)
        {
            item.Archived.Map.PathPrefabMapDirectory = this.fullprefabMapDirectoryPath;
        }


        void IThreadInitialize<Unit>.Initialize(
            Unit item, ICancelable cancelable, Action<Exception> onError, Action runningDoneCallBreak)
        {
            try
            {
                Debug.Log(this + "开始移除!");
                Clear();
            }
            catch (Exception e)
            {
                onError(e);
            }
            runningDoneCallBreak();
        }


        private void CopyDirectory(ICancelable cancelable, string sourceDirectoryName, string destDirectoryName, 
            string fileSearchPattern)
        {
            if (!Directory.Exists(sourceDirectoryName))
                return;
            if (!Directory.Exists(destDirectoryName))
                Directory.CreateDirectory(destDirectoryName);

            string[] filePaths = Directory.GetFileSystemEntries(sourceDirectoryName, fileSearchPattern);

            foreach (var filePath in filePaths)
            {
                if (cancelable.IsDisposed)
                    return;

                string fileName = Path.GetFileName(filePath);
                string sourceFilePath = Path.Combine(sourceDirectoryName, fileName);
                string destFilePath = Path.Combine(destDirectoryName, fileName);

                File.Copy(sourceFilePath, destFilePath, true);
            }
        }

    }

}
