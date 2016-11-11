using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UniRx;
using UnityEngine;

namespace KouXiaGu
{

    /// <summary>
    /// 在协程中准备游戏资源;
    /// </summary>
    public interface ICreateInCoroutine : ICoroutineInitialize<ILoadRes>
    {

    }

    /// <summary>
    /// 多线程准备游戏资源;
    /// </summary>
    public interface ICreateInThread : IThreadInitialize<ILoadRes>
    {
        
    }


    [Serializable]
    internal class CreateControl : InitializeBase<ICreateInCoroutine, ICreateInThread, ILoadRes>
    {
        public CreateControl() : base()
        {

        }

        [SerializeField]
        private GameObject BaseComponents;

        protected override IEnumerable<ICreateInCoroutine> LoadInCoroutineComponents
        {
            get { return BaseComponents.GetComponentsInChildren<ICreateInCoroutine>(); }
        }
        protected override IEnumerable<ICreateInThread> LoadInThreadComponents
        {
            get { return BaseComponents.GetComponentsInChildren<ICreateInThread>(); }
        }

    }

}
