using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UniRx;
using UnityEngine;

namespace KouXiaGu
{
    public class Test_Game : MonoBehaviour, ICreateInCoroutine, ICreateInThread
    {

        [SerializeField]
        private int i;

        [SerializeField]
        private int j;

        void IThreadInitialize<ICreateGameResource>.Initialize(ICreateGameResource item, ICancelable cancelable, Action<Exception> onError, Action runningDoneCallBreak)
        {
            while (i < j)
            {
                if (cancelable.IsDisposed)
                {
                    runningDoneCallBreak();
                    break;
                }
            }
            runningDoneCallBreak();
        }

        IEnumerator ICoroutineInitialize<ICreateGameResource>.Initialize(ICreateGameResource item, ICancelable cancelable, Action<Exception> onError)
        {
            while (i < j)
            {
                if (cancelable.IsDisposed)
                    yield break;

                i++;
                yield return null;
            }
        }

    }
}
