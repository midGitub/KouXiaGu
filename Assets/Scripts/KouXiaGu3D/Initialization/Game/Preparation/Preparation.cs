using System;
using UnityEngine;

namespace KouXiaGu.Initialization
{

    
    public abstract class Preparation : MonoBehaviour
    {

        IDisposable unsubscriber;

        protected virtual void Start()
        {
            var waitPreparation = GetComponentInParent<WaitPreparation>();
            if (waitPreparation == null)
                Debug.LogError("Not Found [IWaitPreparation]");

            unsubscriber = waitPreparation.Subscribe(this);
        }

        /// <summary>
        /// 当完成时调用;
        /// </summary>
        protected void OnComplete()
        {
            unsubscriber.Dispose();
        }

    }

}
