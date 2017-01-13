using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace KouXiaGu.Initialization
{

    /// <summary>
    /// 对游戏进行存档;
    /// </summary>
    [DisallowMultipleComponent]
    public class ArchiveScheduler : OperateWaiter
    {
        ArchiveScheduler() { }

        public static ArchiveDescription AutoArchived
        {
            get
            {
                return new ArchiveDescription()
                {
                    AllowEdit = true,
                    Name = "AutoSave",
                    Time = DateTime.Now.Ticks,
                    Version = BuildVersion.Version,
                    Message = string.Empty,
                };
            }
        }

        /// <summary>
        /// 覆盖保存到指定存档;
        /// </summary>
        public void Archive(ArchiveFile archive)
        {
            if (archive == null)
                throw new ArgumentNullException();
            if (IsWaiting)
                throw new PremiseNotInvalidException();

            var operaters = GetComponentsInChildren<IArchiveOperate>();
            foreach (var operater in operaters)
            {
                operater.SaveState(archive);
            }
            StartWait(operaters);
        }

        protected override void OnComplete(IOperateAsync operater)
        {
            return;
        }

        protected override void OnCompleteAll()
        {
            return;
        }

        protected override void OnFail(IOperateAsync operater)
        {
            Debug.Log(operater.Ex);
        }

    }

}
