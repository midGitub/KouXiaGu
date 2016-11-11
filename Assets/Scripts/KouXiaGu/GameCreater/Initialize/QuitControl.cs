using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

namespace KouXiaGu
{

    public interface IQuitInCoroutine : ICoroutineInitialize<Unit>
    {
    }

    public interface IQuitInThread : IThreadInitialize<Unit>
    {

    }

    [Serializable]
    internal class QuitControl : InitializeBase<IQuitInCoroutine, IQuitInThread, Unit>
    {
        public QuitControl() : base()
        {

        }

        [SerializeField]
        private GameObject BaseComponents;

        protected override IEnumerable<IQuitInCoroutine> LoadInCoroutineComponents
        {
            get { return BaseComponents.GetComponentsInChildren<IQuitInCoroutine>(); }
        }

        protected override IEnumerable<IQuitInThread> LoadInThreadComponents
        {
            get { return BaseComponents.GetComponentsInChildren<IQuitInThread>(); }
        }

    }

}
