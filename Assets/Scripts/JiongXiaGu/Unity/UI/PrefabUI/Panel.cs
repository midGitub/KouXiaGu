using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace JiongXiaGu.Unity.UI
{


    [DisallowMultipleComponent]
    public abstract class Panel : MonoBehaviour
    {
        protected Panel()
        {
        }

        /// <summary>
        /// 提供内部检查是否生效标记;
        /// </summary>
        private bool isEffective;

        /// <summary>
        /// 当激活时调用;
        /// </summary>
        protected abstract void InternalOnActivate();

        /// <summary>
        /// 当取消激活时调用;
        /// </summary>
        protected abstract void InternalOnUnactivate();

        /// <summary>
        /// 当启用时,检查是否为尾部面板,设置是否激活本面板;
        /// </summary>
        private void OnEnable()
        {
            isEffective = true;

            int lastPanelIndex;
            var lastPanel = GetLastPanel(out lastPanelIndex);

            if (lastPanel == null)
            {
                throw new NotImplementedException();
            }
            else if (lastPanel == this)
            {
                lastPanel = GetLastPanel(lastPanelIndex - 1, out lastPanelIndex);
                lastPanel?.InternalOnUnactivate();
                InternalOnActivate();
            }
            else
            {
                InternalOnUnactivate();
            }
        }

        /// <summary>
        /// 当关闭时,检查当前是否为尾部面板,设置是否激活之前的面板;
        /// </summary>
        private void OnDisable()
        {
            int lastPanelIndex;
            var lastPanel = GetLastPanel(out lastPanelIndex);

            if (lastPanel == null)
            {
                throw new NotImplementedException();
            }
            else if (lastPanel == this)
            {
                InternalOnUnactivate();

                lastPanel = GetLastPanel(lastPanelIndex - 1, out lastPanelIndex);
                lastPanel?.InternalOnActivate();
            }
            isEffective = false;
        }

        private void OnBeforeTransformParentChanged()
        {
            if (this)
            {
                OnDisable();
            }
        }

        private void OnTransformParentChanged()
        {
            if (this)
            {
                OnEnable();
            }
        }

        /// <summary>
        /// 激活当前面板;
        /// </summary>
        [ContextMenu(nameof(Activate))]
        public void Activate()
        {
            int lastPanelIndex;
            var lastPanel = GetLastPanel(out lastPanelIndex);

            if (lastPanel == null)
            {
                throw new NotImplementedException();
            }
            if (lastPanel != this)
            {
                lastPanel.InternalOnUnactivate();
                transform.SetAsLastSibling();
                InternalOnActivate();
            }
        }

        /// <summary>
        /// 获取到最后一个Panel脚本和下标;
        /// </summary>
        private Panel GetLastPanel(out int index)
        {
            Transform parent = transform.parent;
            return GetLastPanel(parent.childCount - 1, out index);
        }

        /// <summary>
        /// 从坐标开始处获取到最后一个Panel脚本和下标;
        /// </summary>
        private Panel GetLastPanel(int start, out int index)
        {
            Transform parent = transform.parent;
            for (index = start; index >= 0; index--)
            {
                var child = parent.GetChild(index);
                var panel = child.GetComponent<Panel>();
                if (panel != null && panel.isEffective)
                {
                    return panel;
                }
            }
            return null;
        }
    }
}
