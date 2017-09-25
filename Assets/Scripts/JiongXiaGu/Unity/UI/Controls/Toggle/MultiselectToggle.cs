using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace JiongXiaGu.UI
{

    /// <summary>
    /// 抽象类 切换标签;
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [RequireComponent(typeof(Toggle))]
    [DisallowMultipleComponent]
    public abstract class MultiselectToggle<T> : MonoBehaviour
    {

        Toggle toggleObject;
        protected abstract MultiselectToggleGroup<T> Group { get; set; }
        public abstract T Value { get; protected set; }
        IDisposable groupDisposer;

        public Toggle ToggleObject
        {
            get { return toggleObject ?? (toggleObject = GetComponent<Toggle>()); }
        }

        protected virtual void Awake()
        {
            if (Group != null)
            {
                groupDisposer = Group.RegisterToggle(this);
            }
        }

        public virtual MultiselectToggle<T> PrefabCreate(T value, MultiselectToggleGroup<T> group, Transform parent, bool isSelect)
        {
            Group = group;
            Value = value;
            var ins = Instantiate(this, parent);
            ins.ToggleObject.isOn = isSelect;
            return ins;
        }

        /// <summary>
        /// 提供Group销毁;
        /// </summary>
        internal void Destroy_internal()
        {
            Destroy(gameObject);
            groupDisposer = null;
        }
    }
}
