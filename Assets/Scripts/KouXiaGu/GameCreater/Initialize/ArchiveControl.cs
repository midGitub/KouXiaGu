using System;
using System.Collections.Generic;
using UnityEngine;

namespace KouXiaGu
{

    /// <summary>
    /// 在协程中保存游戏状态;
    /// </summary>
    public interface IArchiveInCoroutine : ICoroutineInitialize<ISaveRes>
    {
       
    }

    /// <summary>
    /// 在线程中保存游戏状态;
    /// </summary>
    public interface IArchiveInThread : IThreadInitialize<ISaveRes>
    {

    }

    [Serializable]
    internal class ArchiveControl : InitializeBase<IArchiveInCoroutine, IArchiveInThread, ISaveRes>
    {
        public ArchiveControl() : base()
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

}
