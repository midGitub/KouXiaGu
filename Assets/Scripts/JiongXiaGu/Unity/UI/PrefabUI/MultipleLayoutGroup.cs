using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace JiongXiaGu.Unity.UI
{
    public struct MultipleLayoutGroup
    {
        [SerializeField]
        private LayoutGroup choices;

        public MultipleLayoutGroup(LayoutGroup choices)
        {
            this.choices = choices;
        }

        /// <summary>
        /// 添加新元素到目录下;
        /// </summary>
        public RectTransform Add(RectTransform item)
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item));
            if (choices == null)
                throw new ObjectDisposedException(nameof(choices));

            item.transform.SetParent(choices.transform, false);
            return item;
        }

        public bool Remove(string name)
        {
            if (choices == null)
                throw new ObjectDisposedException(nameof(choices));

            foreach (Transform child in choices.transform)
            {
                if (child.name == name)
                {
                    GameObject.Destroy(child.gameObject);
                    return true;
                }
            }
            return false;
        }

        public IEnumerable<RectTransform> Enumerate()
        {
            if (choices == null)
                throw new ObjectDisposedException(nameof(choices));

            return choices.transform.Cast<RectTransform>();
        }
    }
}
