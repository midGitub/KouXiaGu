using System;
using System.Collections.Generic;
using UnityEngine;

namespace KouXiaGu.InputControl
{

    public class OccupiedInput
    {
        public OccupiedInput()
        {
            subscriberStack = new LinkedList<Subscriber>();
        }

        readonly LinkedList<Subscriber> subscriberStack;

        public IKeyInput Subscribe(object observer)
        {
            if (observer == null)
                throw new ArgumentNullException("observer");
            if (subscriberStack.Contains(item => item.Node.Value == observer))
                throw new ArgumentException("重复的订阅!");

            return Add(observer);
        }

        Subscriber Add(object observer)
        {
            var lastnode = subscriberStack.Last;
            if (lastnode != null)
            {
                lastnode.Value.IsActivating = false;
            }

            var subscriber = new Subscriber(this);
            subscriber.Node = subscriberStack.AddLast(subscriber);
            subscriber.IsActivating = true;
            return subscriber;
        }

        void Remove(LinkedListNode<Subscriber> node)
        {
            subscriberStack.Remove(node);
            var lastNode = subscriberStack.Last;
            if (lastNode != null)
            {
                lastNode.Value.IsActivating = true;
            }
        }

        class Subscriber : IKeyInput, IDisposable
        {
            public Subscriber(OccupiedInput parent)
            {
                Parent = parent;
                IsDisposabled = false;
            }

            public OccupiedInput Parent { get; private set; }
            public LinkedListNode<Subscriber> Node { get; set; }
            public bool IsDisposabled { get; private set; }
            public bool IsActivating { get; set; }

            public bool GetKeyDown(KeyFunction function)
            {
                Validate();
                return IsActivating && KeyInput.GetKeyDown(function);
            }

            public bool GetKeyHold(KeyFunction function)
            {
                Validate();
                return IsActivating && KeyInput.GetKeyHold(function);
            }

            public bool GetKeyUp(KeyFunction function)
            {
                Validate();
                return IsActivating && KeyInput.GetKeyUp(function);
            }


            public bool GetKeyHold(KeyCode keyCode)
            {
                Validate();
                return IsActivating && Input.GetKey(keyCode);
            }

            public bool GetKeyDown(KeyCode keyCode)
            {
                Validate();
                return IsActivating && Input.GetKeyDown(keyCode);
            }

            public bool GetKeyUp(KeyCode keyCode)
            {
                Validate();
                return IsActivating && Input.GetKeyUp(keyCode);
            }


            void Validate()
            {
                if (IsDisposabled)
                    throw new InvalidOperationException();
            }

            public void Dispose()
            {
                if (!IsDisposabled)
                {
                    Parent.Remove(Node);
                    Parent = null;
                    Node = null;
                    IsActivating = false;
                    IsDisposabled = true;
                }
            }
        }
    }
}
