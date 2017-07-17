using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace KouXiaGu.UI
{

    /// <summary>
    /// 条目列表;
    /// </summary>
    public abstract class ItemList<T> : MonoBehaviour, IEnumerable<T>
    {

        List<UserInterfaceItem<T>> itemList;
        Action<IEnumerable<T>> onValueChanged;

        protected abstract UserInterfaceItem<T> Prefab { get; }

        List<UserInterfaceItem<T>> list
        {
            get { return itemList ?? (itemList = new List<UserInterfaceItem<T>>()); }
        }

        public int Count
        {
            get { return list.Count; }
        }

        protected virtual Transform parent
        {
            get { return transform; }
        }

        public event Action<IEnumerable<T>> OnValueChanged
        {
            add { onValueChanged += value; }
            remove { onValueChanged -= value; }
        }

        void SendValueChangde()
        {
            if (onValueChanged != null)
            {
                onValueChanged.Invoke(this);
            }
        }

        public void Add(T value)
        {
            if (Prefab == null)
                throw new ArgumentNullException("Prefab");

            var uiItem = Prefab.Create(value, this, parent);
            list.Add(uiItem);
            SendValueChangde();
        }

        public void Add(IEnumerable<T> values)
        {
            if (Prefab == null)
                throw new ArgumentNullException("Prefab");

            foreach (var value in values)
            {
                var uiItem = Prefab.Create(value, this, parent);
                list.Add(uiItem);
            }
            SendValueChangde();
        }

        public void Add(UserInterfaceItem<T> uiItem)
        {
            if (!list.Contains(uiItem))
            {
                list.Add(uiItem);
            }
            SendValueChangde();
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
                    SendValueChangde();
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
                    SendValueChangde();
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
            foreach (var uiItem in list)
            {
                Destroy(uiItem.gameObject);
            }
            list.Clear();
            SendValueChangde();
        }

        public IEnumerator<T> GetEnumerator()
        {
            return list.Select(item => item.Value).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
