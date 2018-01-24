using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace JiongXiaGu.Unity.UI
{

    /// <summary>
    /// 对 Panel 激活状态进行管理;
    /// </summary>
    [ExecuteInEditMode]
    public sealed class PanelLayoutGroup : UIBehaviour, ILayoutGroup
    {
        private PanelLayoutGroup()
        {
        }

        private RectTransform rectTransform => transform as RectTransform;

        void ILayoutController.SetLayoutHorizontal()
        {
            if (IsActive())
            {
                StartCoroutine(DelayedUpdateChildrenLayout());
            }
        }

        void ILayoutController.SetLayoutVertical()
        {
            return;
        }

        private void OnTransformChildrenChanged()
        {
            if (IsActive())
            {
                UpdateChildrenLayout();
            }
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            SetDirty();
        }

        private void SetDirty()
        {
            if (IsActive())
            {
                if (!CanvasUpdateRegistry.IsRebuildingLayout())
                {
                    LayoutRebuilder.MarkLayoutForRebuild(rectTransform);
                }
            }
        }

        private IEnumerator DelayedUpdateChildrenLayout()
        {
            yield return new WaitForFixedUpdate();
            UpdateChildrenLayout();
            yield break;
        }

        /// <summary>
        /// 更新子节点的布局;
        /// </summary>
        public void UpdateChildrenLayout()
        {
            IEnumerator<Panel> panels = EnumerateReverse(transform).GetEnumerator();

            if (panels.MoveNext())
            {
                var panel = panels.Current;
                panel.OnActivate();

                while (panels.MoveNext())
                {
                    panel = panels.Current;
                    panel.OnUnactivate();
                }
            }
        }

        private IEnumerable<Panel> EnumerateReverse(Transform parent)
        {
            for (int index = parent.childCount - 1; index >= 0; index--)
            {
                var child = parent.GetChild(index);
                if (child.gameObject.activeSelf)
                {
                    var panel = child.GetComponent<Panel>();
                    if (panel != null && panel.enabled)
                    {
                        yield return panel;
                    }
                }
            }
        }
    }
}
