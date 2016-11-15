﻿using System;
using System.Collections;
using UniRx;
using UnityEngine;

namespace KouXiaGu
{

    public interface IBuildGameInCoroutine : ICoroutineInitialize<BuildGameData> { }
    public interface IBuildGameInThread : IThreadInitialize<BuildGameData> { }

    public interface IQuitInCoroutine : ICoroutineInitialize<Unit> { }
    public interface IQuitInThread : IThreadInitialize<Unit> { }

    public interface IArchiveInCoroutine : ICoroutineInitialize<ArchivedGroup> { }
    public interface IArchiveInThread : IThreadInitialize<ArchivedGroup> { }


    /// <summary>
    /// 构建游戏需要的数据;
    /// </summary>
    public struct BuildGameData
    {
        public BuildGameData(ICoreDataResource coreDataResource, ModGroup modRes, ArchivedGroup archivedGroup)
        {
            this.CoreDataResource = coreDataResource;
            this.ModRes = modRes;
            this.ArchivedGroup = archivedGroup;
        }

        public ModGroup ModRes { get; private set; }

        public ICoreDataResource CoreDataResource { get; private set; }

        public ArchivedGroup ArchivedGroup { get; private set; }


        public static implicit operator ModGroup(BuildGameData item)
        {
            return item.ModRes;
        }
        public static implicit operator ArchivedGroup(BuildGameData item)
        {
            return item.ArchivedGroup;
        }

    }


    public interface IBuildGameData
    {
        IAppendInitialize<IArchiveInCoroutine, IArchiveInThread> AppendArchiveGame { get; }
        IAppendInitialize<IBuildGameInCoroutine, IBuildGameInThread> AppendBuildGame { get; }
        IAppendInitialize<IQuitInCoroutine, IQuitInThread> AppendQuitGame { get; }
        DataArchive ArchiveData { get; }
        DataCore CoreData { get; }
        DataMod ModData { get; }

        BuildGameData GetBuildGameData();
        BuildGameData GetBuildGameData(ArchivedGroup archivedGroup);
    }


    /// <summary>
    /// 游戏创建方法;
    /// </summary>
    [Serializable]
    public class BuildGame : IBuildGameData
    {
        private BuildGame() { }

        [SerializeField]
        private FrameCountType CheckType = FrameCountType.Update;
        private readonly bool publishEveryYield = false;

        [SerializeField]
        private DataCore coreData;

        [SerializeField]
        private DataMod modData;

        [SerializeField]
        private DataArchive archiveData;


        [SerializeField]
        private GameInitialize buildGame;

        [SerializeField]
        private ArchiveInitialize archiveGame;

        [SerializeField]
        private QuitInitialize quitGame;


        public DataCore CoreData
        {
            get { return coreData; }
        }
        public DataMod ModData
        {
            get { return modData; }
        }
        public DataArchive ArchiveData
        {
            get { return archiveData; }
        }

        public IAppendInitialize<IBuildGameInCoroutine, IBuildGameInThread> AppendBuildGame
        {
            get { return buildGame; }
        }
        public IAppendInitialize<IArchiveInCoroutine, IArchiveInThread> AppendArchiveGame
        {
            get { return archiveGame; }
        }
        public IAppendInitialize<IQuitInCoroutine, IQuitInThread> AppendQuitGame
        {
            get { return quitGame; }
        }

        /// <summary>
        /// 开始创建游戏;
        /// </summary>
        public ICancelable Build(BuildGameData buildGameRes, Action onBuiltComplete, Action<Exception> onBuildingFail)
        {
            Func<IEnumerator> coroutine = () => buildGame.Start(buildGameRes, onBuiltComplete, onBuildingFail);
            Observable.FromMicroCoroutine(coroutine, publishEveryYield, CheckType).Subscribe();
            return buildGame;
        }

        public ICancelable Save(ArchivedGroup archivedGroup, Action onSavedComplete, Action<Exception> onSavingFail)
        {
            Action onComplete = () => OnSavedComplete(archivedGroup, onSavedComplete, onSavingFail);
            Func<IEnumerator> coroutine = () => archiveGame.Start(archivedGroup, onComplete, onSavingFail);
            Observable.FromMicroCoroutine(coroutine, publishEveryYield, CheckType).Subscribe();
            return archiveGame;
        }

        public ICancelable Quit(Action onQuitComplete, Action<Exception> onQuittingFail)
        {
            Func<IEnumerator> coroutine = () => quitGame.Start(Unit.Default, onQuitComplete, onQuittingFail);
            Observable.FromMicroCoroutine(coroutine, publishEveryYield, CheckType).Subscribe();
            return quitGame;
        }

        /// <summary>
        /// 根据当前状态获取到创建游戏所需的资源信息;
        /// </summary>
        public BuildGameData GetBuildGameData()
        {
            ICoreDataResource coreDataResource = coreData;
            ModGroup modRes = modData.GetModGroup();
            ArchivedGroup archivedGroup = archiveData.CreateArchived();
            BuildGameData buildGameData = new BuildGameData(coreDataResource, modRes, archivedGroup);
            return buildGameData;
        }

        /// <summary>
        /// 根据这个存档获取到创建游戏所需的资源信息;
        /// </summary>
        public BuildGameData GetBuildGameData(ArchivedGroup archivedGroup)
        {
            ICoreDataResource coreDataResource = coreData;
            ModGroup modRes = modData.GetModGroup();
            BuildGameData buildGameData = new BuildGameData(coreDataResource, modRes, archivedGroup);
            return buildGameData;
        }

        /// <summary>
        /// 若所有接口都保存完毕后,则将存档保存到磁盘;
        /// 若未保存成功,则也返回保存失败;
        /// </summary>
        private void OnSavedComplete(ArchivedGroup archivedGroup, Action onSavedComplete, Action<Exception> onSavingFail)
        {
            try
            {
                archiveData.SaveInDisk(archivedGroup);
                onSavedComplete();
            }
            catch (Exception e)
            {
                onSavingFail(e);
            }
        }


        //#region 加入到初始化事件;

        //public bool Add(IBuildGameInThread item)
        //{
        //    return this.AppendBuildGame.Add(item);
        //}

        //public bool Add(IBuildGameInCoroutine item)
        //{
        //    return this.AppendBuildGame.Add(item);
        //}

        //public bool Contains(IBuildGameInThread item)
        //{
        //    return this.AppendBuildGame.Contains(item);
        //}

        //public bool Contains(IBuildGameInCoroutine item)
        //{
        //    return this.AppendBuildGame.Contains(item);
        //}

        //public bool Remove(IBuildGameInThread item)
        //{
        //    return this.AppendBuildGame.Remove(item);
        //}

        //public bool Remove(IBuildGameInCoroutine item)
        //{
        //    return this.AppendBuildGame.Remove(item);
        //}

        //public bool Add(IArchiveInThread item)
        //{
        //    return this.AppendArchiveGame.Add(item);
        //}

        //public bool Add(IArchiveInCoroutine item)
        //{
        //    return this.AppendArchiveGame.Add(item);
        //}

        //public bool Contains(IArchiveInThread item)
        //{
        //    return this.AppendArchiveGame.Contains(item);
        //}

        //public bool Contains(IArchiveInCoroutine item)
        //{
        //    return this.AppendArchiveGame.Contains(item);
        //}

        //public bool Remove(IArchiveInThread item)
        //{
        //    return this.AppendArchiveGame.Remove(item);
        //}

        //public bool Remove(IArchiveInCoroutine item)
        //{
        //    return this.AppendArchiveGame.Remove(item);
        //}

        //public bool Add(IQuitInThread item)
        //{
        //    return this.AppendQuitGame.Add(item);
        //}

        //public bool Add(IQuitInCoroutine item)
        //{
        //    return this.AppendQuitGame.Add(item);
        //}

        //public bool Contains(IQuitInThread item)
        //{
        //    return this.AppendQuitGame.Contains(item);
        //}

        //public bool Contains(IQuitInCoroutine item)
        //{
        //    return this.AppendQuitGame.Contains(item);
        //}

        //public bool Remove(IQuitInThread item)
        //{
        //    return this.AppendQuitGame.Remove(item);
        //}

        //public bool Remove(IQuitInCoroutine item)
        //{
        //    return this.AppendQuitGame.Remove(item);
        //}

        //#endregion


        /// <summary>
        /// 加载游戏;
        /// </summary>
        [Serializable]
        private class GameInitialize : InitializeAppend<IBuildGameInCoroutine, IBuildGameInThread, BuildGameData>
        {
            private GameInitialize() : base() { }
        }

        /// <summary>
        /// 退出游戏;
        /// </summary>
        [Serializable]
        private class QuitInitialize : InitializeAppend<IQuitInCoroutine, IQuitInThread, Unit>
        {
            public QuitInitialize() : base() { }
        }

        /// <summary>
        /// 游戏归档方法;
        /// </summary>
        [Serializable]
        private sealed class ArchiveInitialize : InitializeAppend<IArchiveInCoroutine, IArchiveInThread, ArchivedGroup>
        {
            private ArchiveInitialize() : base() { }
        }

    }


}
