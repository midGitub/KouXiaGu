using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace KouXiaGu
{

    /// <summary>
    /// 利用 using 和 IDisposable 特性 实现的状态锁;
    /// </summary>
    public class StateLock : MonoBehaviour, IDisposable
    {

        public bool IsLock = false;

        public IDisposable Lock()
        {
            if (IsLock)
                throw new Exception();
            else
                IsLock = true;

            return this;
        }

        public void Dispose()
        {
            IsLock = false;
        }

        [ContextMenu("进入")]
        void Enter()
        {
            using (this.Lock())
            {
                Debug.Log("进入了");
            }
        }

    }


}
