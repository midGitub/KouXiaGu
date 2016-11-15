using System;
using System.IO;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

namespace KouXiaGu.Map
{

    /// <summary>
    /// 编辑预制地图;
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class BlockEditMap<T> : MapBlockEditIO<T>, IBuildGameInThread, IArchiveInThread, IQuitInThread
    {
        public BlockEditMap(string addressPrefix, BlocksMapInfo blocksMapInfo) : base(blocksMapInfo)
        {
            this.addressPrefix = addressPrefix;
        }


        /// <summary>
        /// 完整的地图文件夹路径;
        /// </summary>
        private string fullprefabMapDirectoryPath;
        private string addressPrefix;


        protected string GetBlockName(ShortVector2 address)
        {
            return addressPrefix + address.GetHashCode();
        }

        protected override string GetFullPrefabMapFilePath(ShortVector2 address)
        {
            string blockName = GetBlockName(address);
            string fullPrefabMapFilePath = Path.Combine(fullprefabMapDirectoryPath, blockName);
            return fullPrefabMapFilePath;
        }

        void IThreadInitialize<BuildGameData>.Initialize(
            BuildGameData item, ICancelable cancelable, Action<Exception> onError, Action runningDoneCallBreak)
        {
            try
            {
                Debug.Log(this + "开始初始化!");
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

    }

}
