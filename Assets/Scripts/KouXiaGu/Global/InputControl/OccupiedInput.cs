using System;
using System.Collections.Generic;

namespace KouXiaGu.InputControl
{

    public class OccupiedInput
    {
        public OccupiedInput()
        {
            subscriberStack = new LinkedList<object>();
        }

        readonly LinkedList<object> subscriberStack;

        public IKeyInput Subscribe(object observer)
        {
            if (observer == null)
                throw new ArgumentNullException("observer");
            if (subscriberStack.Contains(observer))
                throw new ArgumentException("重复的订阅!");

            var node = subscriberStack.AddLast(observer);
            return new Subscriber(this, node);
        }

        void Remove(LinkedListNode<object> observer)
        {
            subscriberStack.Remove(observer);
        }

        bool IsActivated(Subscriber subscriber)
        {
            if (subscriberStack.Last.Value == subscriber.Observer)
            {
                return true;
            }
            return false;
        }

        class Subscriber : IKeyInput, IDisposable
        {
            public Subscriber(OccupiedInput parent, LinkedListNode<object> observer)
            {
                Parent = parent;
                Observer = observer;
                IsDisposabled = false;
            }

            public OccupiedInput Parent { get; private set; }
            public LinkedListNode<object> Observer { get; private set; }
            public bool IsDisposabled { get; private set; }

            public bool GetKeyDown(KeyFunction function)
            {
                Validate();
                return Parent.IsActivated(this) && KeyInput.GetKeyDown(function);
            }

            public bool GetKeyHold(KeyFunction function)
            {
                Validate();
                return Parent.IsActivated(this) && KeyInput.GetKeyHold(function);
            }

            public bool GetKeyUp(KeyFunction function)
            {
                Validate();
                return Parent.IsActivated(this) && KeyInput.GetKeyUp(function);
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
                    Parent.Remove(Observer);
                    Parent = null;
                    Observer = null;
                    IsDisposabled = true;
                }
            }
        }
    }
}
