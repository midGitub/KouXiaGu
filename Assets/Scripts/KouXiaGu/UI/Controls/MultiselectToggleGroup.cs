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

        //void Awake()
        //{
        //    onValueChanged += item => Debug.Log(item.ToLog());
        //}

        public void Create(T value, bool isSelect = true)
        {
            TogglePrefab.PrefabCreate(value, this, transform, isSelect);
        }

        public IDisposable RegisterToggle(MultiselectToggle<T> toggle, object value)
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
            ICollection<T> values = new List<T>();
            foreach (var pair in Toggles)
            {
                if (pair.ToggleObject.isOn)
                {
                    values.Add(pair.Value);
                }
            }
            onValueChanged.Invoke(values);
        }

        /// <summary>
        /// 全选;
        /// </summary>
        [ContextMenu("全选")]
        public void SelectAll()
        {
            foreach (var toggle in Toggles)
            {
                toggle.ToggleObject.isOn = true;
            }
        }

        /// <summary>
        /// 清除所有选择;
        /// </summary>
        [ContextMenu("清除")]
        public void SelectClear()
        {
            foreach (var toggle in Toggles)
            {
                toggle.ToggleObject.isOn = false;
            }
        }

        /// <summary>
        /// 反选;
        /// </summary>
        [ContextMenu("反选")]
        public void SelectReverse()
        {
            foreach (var toggle in Toggles)
            {
                toggle.ToggleObject.isOn = !toggle.ToggleObject.isOn;
            }
        }
    }
}
