using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UniRx;
using UnityEngine;
using KouXiaGu.Map;

namespace KouXiaGu
{
    public class Test_Game : MonoBehaviour, IBuildGameInCoroutine, IBuildGameInThread
    {

        [SerializeField]
        private int i;

        [SerializeField]
        private int j;

        void IThreadInitialize<BuildGameData>.Initialize(BuildGameData item, ICancelable cancelable, Action<Exception> onError, Action runningDoneCallBreak)
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

        IEnumerator ICoroutineInitialize<BuildGameData>.Initialize(BuildGameData item, ICancelable cancelable, Action<Exception> onError)
        {
            while (i < j)
            {
                if (cancelable.IsDisposed)
                    yield break;

                i++;
                yield return null;
            }
        }

        [ContextMenu("Test")]
        private void Test_Log()
        {
            Debug.Log(new ShortVector2(100,200).GetHashCode().ToString() + "  " + new ShortVector2(200, 100).GetHashCode());

            Debug.Log(GetInt(100,200).ToString() + "  " + GetInt(200, 100));
        }

        [ContextMenu("Test2")]
        private void Test_Log2()
        {
            int x1 = 100000;
            int y2 = x1;

            HashSet<int> set = new HashSet<int>();

            for (int x = -x1; x < x1; x++)
            {
                for (int y = -y2; y < y2; y++)
                {
                    if (set.Add(new ShortVector2((short)x, (short)y).GetHashCode()))
                    {
                        Debug.Log("重复!" + x + "  " + y);
                    }
                }
            }
        }

        private int GetInt(int x, int y)
        {
            return x * short.MaxValue + y;
        }

    }
}
