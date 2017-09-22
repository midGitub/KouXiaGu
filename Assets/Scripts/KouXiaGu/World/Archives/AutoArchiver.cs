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
                autoArchives.Sort(new OrderByTimeAscendingComparer());
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
                ArchiveInfo info;
                if (!Archive.TryReadInfo(archivePath, out info))
                {
                    info = new ArchiveInfo(archiveName, true);
                }
                Archive archive = new Archive(archivePath, info);
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
                for (int i = 0; i < autoArchives.Count; i++)
                {
                    yield return new WaitForSecondsRealtime(autoSaveInterval);

                    while (archiveController.IsWriting)
                    {
                        yield return new WaitForSecondsRealtime(10);
                    }

                    Archive archive = autoArchives[i];
                    lastAutoSaveTime = Time.realtimeSinceStartup;
                    archiveController.WriteArchive(archive);
                }
            }
        }

        /// <summary>
        /// 设置自动更新间隔,
        /// </summary>
        public void SetAutoSaveInterval(float second)
        {

        }

        /// <summary>
        /// 根据时间升序的对比器;存档时间由早到晚,未创建的存档永远在最前面;
        /// </summary>
        class OrderByTimeAscendingComparer : Comparer<Archive>
        {
            public override int Compare(Archive x, Archive y)
            {
                if (x.Exists())
                {
                    if (y.Exists())
                    {
                        int r = (int)(x.Info.Time.Ticks - y.Info.Time.Ticks);
                        return r;
                    }
                    else
                    {
                        return 1;
                    }
                }
                else
                {
                    if (y.Exists())
                    {
                        return -1;
                    }
                    else
                    {
                        return 0;
                    }
                }
            }
        }

    }
}
