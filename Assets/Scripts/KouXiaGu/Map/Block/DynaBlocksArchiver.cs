using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UniRx;
using UnityEngine;

namespace KouXiaGu.Map
{

    /// <summary>
    /// 地图块归档和初始化;
    /// </summary>
    public class DynaBlocksArchiver : IBuildGameInThread, IArchiveInThread
    {
        private DynaBlocksArchiver() { }

        public DynaBlocksArchiver(MapBlockIOInfo mapBlockIOInfo)
        {
            this.mapBlockIOInfo = mapBlockIOInfo;
        }

        private MapBlockIOInfo mapBlockIOInfo;

        /// <summary>
        /// 保存到存档的位置;
        /// </summary>
        private string archivedDirectoryPath
        {
            get { return mapBlockIOInfo.archivedDirectoryPath; }
        }

        public string ArchivedSearchPattern
        {
            get{ return mapBlockIOInfo.addressPrefix + "*"; }
        }


        private string GetFullArchiveTempDirectoryPath()
        {
            return mapBlockIOInfo.GetFullArchiveTempDirectoryPath();
        }


        void IThreadInitialize<BuildGameData>.Initialize(
            BuildGameData item, ICancelable cancelable, Action<Exception> onError, Action runningDoneCallBreak)
        {
            try
            {
                Debug.Log(this + "开始初始化!");
                Recovery(item, cancelable);
            }
            catch (Exception e)
            {
                onError(e);
            }
            runningDoneCallBreak();
        }

        private void Recovery(ArchivedGroup item, ICancelable cancelable)
        {
            if (item.FromFile)
            {
                string fullArchivedDirectoryPath = GetFullArchivedDirectoryPath(item);
                string fullTempArchiveDirectoryPath = GetFullArchiveTempDirectoryPath();

                CopyDirectory(cancelable, fullArchivedDirectoryPath, fullTempArchiveDirectoryPath, ArchivedSearchPattern);
            }
        }

        void IThreadInitialize<ArchivedGroup>.Initialize(
            ArchivedGroup item, ICancelable cancelable, Action<Exception> onError, Action runningDoneCallBreak)
        {
            try
            {
                Debug.Log(this + "开始保存到!");
                Archive(item, cancelable);
            }
            catch (Exception e)
            {
                onError(e);
            }
            runningDoneCallBreak();
        }

        private void Archive(ArchivedGroup item, ICancelable cancelable)
        {
            string fullArchivedDirectoryPath = GetFullArchivedDirectoryPath(item);
            string fullTempArchiveDirectoryPath = GetFullArchiveTempDirectoryPath();

            CopyDirectory(cancelable, fullTempArchiveDirectoryPath, fullArchivedDirectoryPath, ArchivedSearchPattern);
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

        private string GetFullArchivedDirectoryPath(ArchivedGroup item)
        {
            return mapBlockIOInfo.GetFullArchivedDirectoryPath(item);
        }

    }

}
