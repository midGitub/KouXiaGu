using System;
using System.Collections.Generic;
using UnityEngine;
using KouXiaGu.Collections;
using UnityEngine.SceneManagement;

namespace KouXiaGu.Initialization
{

    /// <summary>
    /// 在游戏一开始时初始化;
    /// </summary>
    [DisallowMultipleComponent]
    public class WaitPreparation : MonoBehaviour
    {

        [SerializeField]
        string nextSceneName = "Start";

        LinkedList<Preparation> observers;

        void Awake()
        {
            observers = new LinkedList<Preparation>();
        }

        public IDisposable Subscribe(Preparation item)
        {
            var node = observers.AddLast(item);
            return new Unsubscriber(this, observers, node);
        }

        void OnComplete()
        {
            if (observers.Count == 0)
            {
                SceneManager.LoadScene(nextSceneName);
            }
        }

        class Unsubscriber : LinkedListUnsubscriber<Preparation>
        {
            public Unsubscriber(WaitPreparation init, LinkedList<Preparation> observers, LinkedListNode<Preparation> observer) : base(observers, observer)
            {
                this.init = init;
            }

            WaitPreparation init;

            public override void Dispose()
            {
                base.Dispose();
                init.OnComplete();
            }

        }

    }

}
