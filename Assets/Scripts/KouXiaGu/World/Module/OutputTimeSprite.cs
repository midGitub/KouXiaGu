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
    class OutputTimeSprite : MonoBehaviour, IXiaGuObserver<IWorld>, IXiaGuObserver<DateTime>
    {

        OutputTimeSprite()
        {
        }

        [SerializeField]
        Text textObject;

        [SerializeField]
        WorldInitializer world;

        IDisposable unsubscribe;

        void Start()
        {
            unsubscribe = world.Subscribe(this);
        }

        void IXiaGuObserver<IWorld>.OnNext(IWorld world)
        {
            world.Time.Subscribe(this);
        }

        void IXiaGuObserver<DateTime>.OnNext(DateTime item)
        {
            string str = item.ToString();
            str += "\n" + item.GetMonthType();
            textObject.text = str;
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
