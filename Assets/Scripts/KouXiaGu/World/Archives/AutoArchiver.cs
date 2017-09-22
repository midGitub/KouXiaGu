using KouXiaGu.Resources.Archives;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using System.Threading;
using KouXiaGu.Resources;
using System.IO;

namespace KouXiaGu.World.Archives
{

    /// <summary>
    /// 自动存档管理;
    /// </summary>
    [DisallowMultipleComponent]
    [RequireComponent(typeof(SceneArchiveController))]
    public class AutoArchiver : MonoBehaviour, IDataInitializeHandle, IWorldCompletedHandle
    {
        /// <summary>
        /// 是否启用自动存档?
        /// </summary>
        [SerializeField]
        bool isActivate = true;

        /// <summary>
        /// 自动保存间隔(单位秒);
        /// </summary>
        [SerializeField]
        float autoSaveInterval = 360;

        /// <summary>
        /// 上次自动保存的时间;
        /// </summary>
        float lastAutoSaveTime;

        /// <summary>
        /// 自动存档合集;
        /// </summary>
        List<Archive> autoArchives;

        /// <summary>
        /// 场景存档控制器;
        /// </summary>
        SceneArchiveController archiveController;

        /// <summary>
        /// 是否启用自动存档?
        /// </summary>
        public bool IsActivate
        {
            get { return isActivate; }
        }

        /// <summary>
        /// 自动保存间隔(单位秒);
        /// </summary>
        public float AutoSaveInterval
        {
            get { return autoSaveInterval; }
        }

        void Awake()
        {
            archiveController = GetComponent<SceneArchiveController>();
        }

        Task IDataInitializeHandle.StartInitialize(Archive archive, CancellationToken token)
        {
            return Task.Run(delegate ()
            {
                autoArchives = new List<Archive>(EnumerateAutoArchives());
            });
        }

        /// <summary>
        /// 获取到所有自动存档;
        /// </summary>
        IEnumerable<Archive> EnumerateAutoArchives()
        {
            const int maxArchiveCount = 3;
            for (int i = 1; i <= maxArchiveCount; i++)
            {
                string archiveName = "AutoSave" + i;
                string archivePath = Path.Combine(Resource.ArchivesDirectoryPath, archiveName);
                ArchiveInfo info = new ArchiveInfo(archiveName, true);
                Archive archive = Archive.ReadOrCreate(archivePath, info);
                yield return archive;
            }
        }

        void IWorldCompletedHandle.OnWorldCompleted()
        {
            StartCoroutine(AutoArchiveCoroutine());
        }

        IEnumerator AutoArchiveCoroutine()
        {
            while (true)
            {
                yield return new WaitForSecondsRealtime(autoSaveInterval);

                while (archiveController.IsWriting)
                {
                    yield return new WaitForSecondsRealtime(10);
                }

                autoArchives.Sort(new Archive.OrderByTimeAscendingComparer());
                Archive archive = autoArchives[0];
                lastAutoSaveTime = Time.realtimeSinceStartup;
                archiveController.WriteArchive(archive);
            }
        }

        /// <summary>
        /// 设置自动更新间隔,
        /// </summary>
        public void SetAutoSaveInterval(float second)
        {

        }
    }
}
