﻿//using JiongXiaGu.Concurrent;
//using JiongXiaGu.Unity;
//using JiongXiaGu.Unity.Initializers;
//using JiongXiaGu.Unity.Resources;
//using System.Collections;
//using System.Collections.Generic;
//using System.IO;
//using System.Threading;
//using System.Threading.Tasks;
//using UnityEngine;

//namespace JiongXiaGu.Unity.Archives
//{

//    /// <summary>
//    /// 自动存档管理(仅Unity线程访问);
//    /// </summary>
//    [DisallowMultipleComponent]
//    [RequireComponent(typeof(SceneArchiveController))]
//    public class AutoArchiver : MonoBehaviour
//    {
//        /// <summary>
//        /// 是否启用自动存档?
//        /// </summary>
//        [SerializeField]
//        bool isActivate = true;

//        /// <summary>
//        /// 自动保存间隔(单位秒);
//        /// </summary>
//        [SerializeField]
//        float autoSaveInterval = 360;

//        /// <summary>
//        /// 上次自动保存的时间;
//        /// </summary>
//        float lastAutoSaveTime;

//        /// <summary>
//        /// 自动存档合集;
//        /// </summary>
//        List<ArchiveInfo> autoArchives;

//        /// <summary>
//        /// 场景存档控制器;
//        /// </summary>
//        SceneArchiveController archiveController;

//        /// <summary>
//        /// 计时器运行的协程;
//        /// </summary>
//        UnityEngine.Coroutine autoArchiveCoroutine;

//        /// <summary>
//        /// 是否启用自动存档?
//        /// </summary>
//        public bool IsActivate
//        {
//            get { return isActivate; }
//        }

//        /// <summary>
//        /// 自动保存间隔(单位秒);
//        /// </summary>
//        public float AutoSaveInterval
//        {
//            get { return autoSaveInterval; }
//        }

//        void Awake()
//        {
//            archiveController = GetComponent<SceneArchiveController>();
//        }

//        //Task ISceneComponentInitializeHandle.Initialize(CancellationToken token)
//        //{
//        //    return Task.Run(delegate ()
//        //    {
//        //        token.ThrowIfCancellationRequested();
//        //        autoArchives = new List<ArchiveInfo>(EnumerateAutoArchives());
//        //    });
//        //}

//        /// <summary>
//        /// 获取到所有自动存档;
//        /// </summary>
//        IEnumerable<ArchiveInfo> EnumerateAutoArchives()
//        {
//            throw new System.NotImplementedException();

//            //const int maxArchiveCount = 3;
//            //for (int i = 1; i <= maxArchiveCount; i++)
//            //{
//            //    string archiveName = "AutoSave" + i;
//            //    string archivePath = Path.Combine(Resource.ArchiveDirectory, archiveName);
//            //    ArchiveDescription description = new ArchiveDescription(archiveName, true);
//            //    ArchiveInfo archive = new ArchiveInfo(description, archivePath);
//            //    yield return archive;
//            //}
//        }

//        //void ISceneCompletedHandle.OnSceneCompleted()
//        //{
//        //    if (isActivate)
//        //    {
//        //        StartAutoArchive();
//        //    }
//        //}

//        /// <summary>
//        /// 设置是否启用自动存档;
//        /// </summary>
//        public void SetActivate(bool isActivate)
//        {
//            this.isActivate = isActivate;
//            if (isActivate)
//            {
//                StartAutoArchive();
//            }
//            else
//            {
//                StopAutoArchive();
//            }
//        }

//        /// <summary>
//        /// 开始自动存档;
//        /// </summary>
//        void StartAutoArchive()
//        {
//            StartAutoArchive(autoSaveInterval);
//        }

//        /// <summary>
//        /// 开始自动存档;
//        /// </summary>
//        void StartAutoArchive(float firstWaitSeconds)
//        {
//            if (autoArchiveCoroutine == null)
//            {
//                autoArchiveCoroutine = StartCoroutine(AutoArchiveCoroutine(firstWaitSeconds));
//            }
//        }

//        /// <summary>
//        /// 停止自动存档;
//        /// </summary>
//        void StopAutoArchive()
//        {
//            if (autoArchiveCoroutine != null)
//            {
//                StopCoroutine(autoArchiveCoroutine);
//                autoArchiveCoroutine = null;
//            }
//        }

//        /// <summary>
//        /// 自动存档计时执行器;
//        /// </summary>
//        /// <param name="firstWaitSeconds">设置距离第一次保存等待多少秒</param>
//        IEnumerator AutoArchiveCoroutine(float firstWaitSeconds)
//        {
//            float waitSeconds = firstWaitSeconds;
//            while (true)
//            {
//                yield return new WaitForSecondsRealtime(waitSeconds);
//                yield return new WaitUntil(() => !archiveController.IsWriting);
//                yield return WriteArchive().WaitComplete();
//                waitSeconds = autoSaveInterval;
//            }
//        }

//        /// <summary>
//        /// 输出自动存档;
//        /// </summary>
//        Task WriteArchive()
//        {
//            autoArchives.Sort(new ArchiveInfo.OrderByTimeAscendingComparer());
//            ArchiveInfo archive = autoArchives[0];
//            lastAutoSaveTime = Time.realtimeSinceStartup;
//            return archiveController.WriteArchive(archive);
//        }

//        /// <summary>
//        /// 设置自动更新间隔,
//        /// </summary>
//        public void SetAutoSaveInterval(float second)
//        {
//            if(autoSaveInterval != second)
//            {
//                StopAutoArchive();
//                autoSaveInterval = second;
//                float firstWaitSeconds = lastAutoSaveTime - Time.realtimeSinceStartup + autoSaveInterval;
//                StartAutoArchive(firstWaitSeconds);
//            }
//        }
//    }
//}
