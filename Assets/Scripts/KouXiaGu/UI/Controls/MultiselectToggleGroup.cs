using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

namespace KouXiaGu.UI
{

    /// <summary>
    /// 抽象类 允许多选的切换标签;
    /// </summary>
    [DisallowMultipleComponent]
    public abstract class MultiselectToggleGroup<T> : MonoBehaviour
    {
        protected MultiselectToggleGroup()
        {
        }

        Action<ICollection<T>> onValueChanged;
        List<MultiselectToggle<T>> toggles;
        bool ignoreToggleEvent;

        List<MultiselectToggle<T>> Toggles
        {
            get { return toggles ?? (toggles = new List<MultiselectToggle<T>>()); }
        }

        /// <summary>
        /// 当值发生变化时调用,传送所有已经选中的值;
        /// </summary>
        public event Action<ICollection<T>> OnValueChanged
        {
            add { onValueChanged += value; }
            remove { onValueChanged -= value; ; }
        }

        public int Count
        {
            get { return toggles.Count; }
        }

        protected abstract MultiselectToggle<T> TogglePrefab { get; }

        void Awake()
        {
            onValueChanged += item => Debug.Log(item.ToLog());
        }

        /// <summary>
        /// 使用预制创建;
        /// </summary>
        public void Create(T value, bool isSelect = true)
        {
            if (TogglePrefab == null)
                throw new ArgumentNullException("TogglePrefab");

            TogglePrefab.PrefabCreate(value, this, transform, isSelect);
        }

        /// <summary>
        /// 使用预制创建;
        /// </summary>
        public void Create(IEnumerable<T> values, bool isSelect = true)
        {
            if (TogglePrefab == null)
                throw new ArgumentNullException("TogglePrefab");

            ignoreToggleEvent = true;
            foreach (var value in values)
            {
                TogglePrefab.PrefabCreate(value, this, transform, isSelect);
            }
            ignoreToggleEvent = false;
            SendValueChangedEvent();
        }

        public IDisposable RegisterToggle(MultiselectToggle<T> toggle)
        {
            if (Toggles.Contains(toggle))
            {
                throw new ArgumentException();
            }
            else
            {
                Toggles.Add(toggle);
                toggle.ToggleObject.onValueChanged.AddListener(OnToggleValueChanged);
                return new CollectionUnsubscriber<MultiselectToggle<T>>(toggles, toggle);
            }
        }

        void OnToggleValueChanged(bool isOn)
        {
            if (!ignoreToggleEvent)
            {
                SendValueChangedEvent();
            }
        }

        void SendValueChangedEvent()
        {
            var values = SelectValueAll();
            if (onValueChanged != null)
            {
                onValueChanged.Invoke(values);
            }
        }

        /// <summary>
        /// 获取到所有选中的值;
        /// </summary>
        public List<T> SelectValueAll()
        {
            var values = new List<T>();
            foreach (var pair in Toggles)
            {
                if (pair.ToggleObject.isOn)
                {
                    values.Add(pair.Value);
                }
            }
            return values;
        }

        /// <summary>
        /// 全选;
        /// </summary>
        [ContextMenu("全选")]
        public void SelectAll()
        {
            ignoreToggleEvent = true;
            foreach (var toggle in Toggles)
            {
                toggle.ToggleObject.isOn = true;
            }
            ignoreToggleEvent = false;
            SendValueChangedEvent();
        }

        /// <summary>
        /// 清除所有选择;
        /// </summary>
        [ContextMenu("清除")]
        public void SelectClear()
        {
            ignoreToggleEvent = true;
            foreach (var toggle in Toggles)
            {
                toggle.ToggleObject.isOn = false;
            }
            ignoreToggleEvent = false;
            SendValueChangedEvent();
        }

        /// <summary>
        /// 反选;
        /// </summary>
        [ContextMenu("反选")]
        public void SelectReverse()
        {
            ignoreToggleEvent = true;
            foreach (var toggle in Toggles)
            {
                toggle.ToggleObject.isOn = !toggle.ToggleObject.isOn;
            }
            ignoreToggleEvent = false;
            SendValueChangedEvent();
        }
    }
}
