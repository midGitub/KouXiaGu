using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace KouXiaGu.World.Archives
{

    /// <summary>
    /// 自动存档管理;
    /// </summary>
    [DisallowMultipleComponent]
    [RequireComponent(typeof(SceneArchiveController))]
    public class AutoArchiver : MonoBehaviour , IWorldCompletedHandle
    {
        /// <summary>
        /// 自动保存间隔(单位秒);
        /// </summary>
        [SerializeField]
        float autoSaveInterval;

        /// <summary>
        /// 最多保存多少个自动存档;
        /// </summary>
        [SerializeField]
        int maxAutoArchive;

        SceneArchiveController archiveController;

        public float AutoSaveInterval
        {
            get { return autoSaveInterval; }
        }

        void Awake()
        {
            archiveController = GetComponent<SceneArchiveController>();
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
            }
        }
    }
}
