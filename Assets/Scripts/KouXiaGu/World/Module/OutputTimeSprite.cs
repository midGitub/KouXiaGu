using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KouXiaGu.Rx;
using UnityEngine;
using UnityEngine.UI;

namespace KouXiaGu.World
{

    /// <summary>
    /// 在控制台输出时间信息;
    /// </summary>
    [DisallowMultipleComponent]
    class OutputTimeSprite : MonoBehaviour, IObserver<DateTime>
    {

        OutputTimeSprite()
        {
        }

        [SerializeField]
        Text textObject;

        [SerializeField]
        TimeManager timer;

        IDisposable unsubscribe;

        void Start()
        {
            unsubscribe = timer.Subscribe(this);
        }

        public void OnNext(DateTime item)
        {
            textObject.text = item.ToString();
        }

        public void OnError(Exception error)
        {
            return;
        }

        public void OnCompleted()
        {
            unsubscribe.Dispose();
        }

    }

}
