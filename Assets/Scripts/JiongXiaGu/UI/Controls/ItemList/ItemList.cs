using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace JiongXiaGu.UI
{

    /// <summary>
    /// 条目列表;
    /// </summary>
    public abstract class ItemList<T> : MonoBehaviour, IEnumerable<T>, IObservableCollection<T>
    {

        List<UserInterfaceItem<T>> itemList;
        Action<IEnumerable<T>> onSelectValueChanged;
        IObserverCollection<ICollectionObserver<T>> observers;
        bool initialized;

        protected abstract UserInterfaceItem<T> Prefab { get; }

        public int Count
        {
            get { return itemList != null ? itemList.Count : 0; }
        }

        protected virtual Transform parent
        {
            get { return transform; }
        }

        public event Action<IEnumerable<T>> OnSelectValueChanged
        {
            add { onSelectValueChanged += value; }
            remove { onSelectValueChanged -= value; }
        }

        void Initialize()
        {
            if (!initialized)
            {
                itemList = new List<UserInterfaceItem<T>>();
                observers = new ObserverList<ICollectionObserver<T>>();
                initialized = true;
            }
        }

        public IDisposable Subscribe(ICollectionObserver<T> observer)
        {
            Initialize();
            if (observers.Contains(observer))
                throw new ArgumentException();

            return observers.Subscribe(observer);
        }

        public void Add(T value)
        {
            if (Prefab == null)
                throw new ArgumentNullException("Prefab");

            Initialize();
            var uiItem = Prefab.Create(value, this, parent);
            itemList.Add(uiItem);
            SendAddEventToObserver(value);
            SendValueChangdeEvent();
        }

        public void Add(IEnumerable<T> values)
        {
            if (Prefab == null)
                throw new ArgumentNullException("Prefab");

            Initialize();
            foreach (var value in values)
            {
                var uiItem = Prefab.Create(value, this, parent);
                itemList.Add(uiItem);
                SendAddEventToObserver(value);
            }
            SendValueChangdeEvent();
        }

        public void Add(UserInterfaceItem<T> uiItem)
        {
            Initialize();
            if (!itemList.Contains(uiItem))
            {
                itemList.Add(uiItem);
                SendAddEventToObserver(uiItem.Value);
            }
            SendValueChangdeEvent();
        }

        public bool Remove(T item)
        {
            if (itemList != null)
            {
                int index = FindIndex(item);
                if (index >= 0)
                {
                    var uiItem = itemList[index];
                    itemList.RemoveAt(index);
                    Destroy(uiItem.gameObject);
                    SendRemoveEventToObserver(item);
                    SendValueChangdeEvent();
                    return true;
                }
            }
            return false;
        }

        public bool Remove(UserInterfaceItem<T> uiItem)
        {
            if (itemList != null)
            {
                int index = itemList.FindIndex(uiItem);
                if (index >= 0)
                {
                    itemList.RemoveAt(index);
                    Destroy(uiItem.gameObject);
                    SendRemoveEventToObserver(uiItem.Value);
                    SendValueChangdeEvent();
                    return true;
                }
            }
            return false;
        }

        int FindIndex(T value)
        {
            if (itemList != null)
            {
                return itemList.FindIndex(uiItem => uiItem.Value.Equals(value));
            }
            return -1;
        }

        /// <summary>
        /// 确认是否存在此元素;
        /// </summary>
        public bool Contains(T value)
        {
            if (itemList != null)
            {
                return itemList.Contains(item => item.Value.Equals(value));
            }
            return false;
        }

        public void Clear()
        {
            if (itemList != null)
            {
                foreach (var uiItem in itemList)
                {
                    Destroy(uiItem.gameObject);
                }
                itemList.Clear();
                SendClearEventToObserver();
                SendValueChangdeEvent();
            }
        }

        void SendAddEventToObserver(T item)
        {
            foreach (var observer in observers.EnumerateObserver())
            {
                try
                {
                    observer.OnAdded(item);
                }
                catch (Exception ex)
                {
                    Debug.LogError(ex);
                }
            }
        }

        void SendRemoveEventToObserver(T item)
        {
            foreach (var observer in observers.EnumerateObserver())
            {
                try
                {
                    observer.OnRemoved(item);
                }
                catch (Exception ex)
                {
                    Debug.LogError(ex);
                }
            }
        }

        void SendClearEventToObserver()
        {
            foreach (var observer in observers.EnumerateObserver())
            {
                try
                {
                    observer.OnCleared();
                }
                catch (Exception ex)
                {
                    Debug.LogError(ex);
                }
            }
        }

        void SendValueChangdeEvent()
        {
            if (onSelectValueChanged != null)
            {
                onSelectValueChanged.Invoke(this);
            }
        }

        public IEnumerator<T> GetEnumerator()
        {
            if (itemList != null)
            {
                return itemList.Select(item => item.Value).GetEnumerator();
            }
            else
            {
                return EmptyEnumerable<T>.Default;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
