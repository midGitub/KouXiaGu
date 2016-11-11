using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UniRx;
using UnityEngine;

namespace KouXiaGu
{

    [Serializable]
    internal class CreateControl : IResourceInitialize<ICreateGameResource>
    {


        #region IResourceInitialize;

        [SerializeField]
        private CreateInitialize initialize;

        public bool IsDisposed
        {
            get { return ((IResourceInitialize<ICreateGameResource>)this.initialize).IsDisposed;}
        }

        public void Dispose()
        {
            ((IResourceInitialize<ICreateGameResource>)this.initialize).Dispose();
        }

        public IEnumerator Start(ICreateGameResource item, Action<Exception> onError, Action onInitialized, Action<Exception> onFail)
        {
            return ((IResourceInitialize<ICreateGameResource>)this.initialize).Start(item, onError, onInitialized, onFail);
        }

        #endregion

    }

    /// <summary>
    /// 在协程中准备游戏资源;
    /// </summary>
    public interface ICreateInCoroutine : ICoroutineInitialize<ICreateGameResource>
    {

    }

    /// <summary>
    /// 多线程准备游戏资源;
    /// </summary>
    public interface ICreateInThread : IThreadInitialize<ICreateGameResource>
    {
        
    }


    [Serializable]
    internal class CreateInitialize : ResourceInitialize<ICreateInCoroutine, ICreateInThread, ICreateGameResource>,
        IResourceInitialize<ICreateGameResource>
    {
        public CreateInitialize() : base()
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
