using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace KouXiaGu.KeyInput
{

    /// <summary>
    /// 按最后加入顺序响应按键;
    /// </summary>
    [Serializable]
    public class ResponseKeyStack
    {

        public ResponseKeyStack(KeyCode key)
        {
            this.key = key;
            OnAwake();
        }

        [SerializeField]
        KeyCode key;

        LinkedList<IKeyResponse> observers;

        public KeyCode Key
        {
            get { return key; }
        }

        public bool IsInitialized
        {
            get { return observers != null; }
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
            get { return observers.Last.Value; }
        }

        public void OnAwake()
        {
            if (observers == null)
                observers = new LinkedList<IKeyResponse>();
        }

        public void OnUpdate()
        {
            if (!IsEmpty)
            {
                if (Input.GetKeyDown(Key))
                {
                    Activate.OnKeyDown();
                }
                else if (Input.GetKey(Key))
                {
                    Activate.OnKeyHold();
                }
            }
        }

        public IDisposable Subscribe(IKeyResponse observer)
        {
            var node = observers.AddLast(observer);
            return new Unsubscriber(observers, node);
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

        class Unsubscriber : IDisposable
        {
            public Unsubscriber(LinkedList<IKeyResponse> observers, LinkedListNode<IKeyResponse> observer)
            {
                this.observers = observers;
                this.observer = observer;
            }

            LinkedList<IKeyResponse> observers;
            LinkedListNode<IKeyResponse> observer;

            public void Dispose()
            {
                observers.Remove(observer);
            }
        }

    }

}
