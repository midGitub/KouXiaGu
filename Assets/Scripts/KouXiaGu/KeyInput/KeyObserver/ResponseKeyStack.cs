using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KouXiaGu.Collections;
using KouXiaGu.Rx;
using UnityEngine;

namespace KouXiaGu.KeyInput
{

    /// <summary>
    /// 响应最后加入的按键;
    /// </summary>
    [Serializable]
    public class ResponseKeyStack
    {

        public ResponseKeyStack(KeyCode key)
        {
            this.key = key;
        }

        [SerializeField]
        KeyCode key;

        readonly LinkedList<IKeyResponse> observers = new LinkedList<IKeyResponse>();

        public KeyCode Key
        {
            get { return key; }
        }

        public bool IsEmpty
        {
            get { return observers.Count == 0; }
        }

        public int Count
        {
            get { return observers.Count; }
        }

        public IKeyResponse Activate
        {
            get { return IsEmpty ? null : observers.Last.Value; }
        }

        public void OnUpdate()
        {
            if (!IsEmpty)
            {
                if (Input.GetKeyDown(Key))
                {
                    Activate.OnKeyDown();
                }
            }
        }

        public IDisposable Subscribe(IKeyResponse observer)
        {
            var node = observers.AddLast(observer);
            return new LinkedListUnsubscriber<IKeyResponse>(observers, node);
        }

        public override bool Equals(object obj)
        {
            if (!(obj is ResponseKeyStack))
                return false;
            return ((ResponseKeyStack)obj).Key == Key;
        }

        public override int GetHashCode()
        {
            return Key.GetHashCode();
        }

        public override string ToString()
        {
            return "[Key:" + Key + ",Observer:" + Count + "]";
        }

    }

}
