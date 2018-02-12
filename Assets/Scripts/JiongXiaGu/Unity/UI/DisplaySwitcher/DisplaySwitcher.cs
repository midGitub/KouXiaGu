using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

namespace JiongXiaGu.Unity.UI
{


    public abstract class DisplaySwitcher : MonoBehaviour
    {
        [SerializeField]
        private UnityEvent onDisplay;
        [SerializeField]
        private UnityEvent onHide;

        public event UnityAction OnDisplay
        {
            add { onDisplay.AddListener(value); }
            remove { onDisplay.RemoveListener(value); }
        }

        public event UnityAction OnHide
        {
            add { onHide.AddListener(value); }
            remove { onHide.RemoveListener(value); }
        }

        public abstract bool IsDisplay();
        protected abstract void InternalDisplay();
        protected abstract void InternalHide();

        public virtual void Display(float seconds)
        {
            onDisplay.Invoke();
            StartCoroutine(DeferredExecution(InternalDisplay, seconds));
        }

        public virtual void Hide(float seconds)
        {
            onHide.Invoke();
            StartCoroutine(DeferredExecution(InternalHide, seconds));
        }

        private IEnumerator DeferredExecution(Action action, float seconds)
        {
            yield return new WaitForSeconds(seconds);
            action.Invoke();
        }

        [ContextMenu(nameof(Display))]
        public void Display()
        {
            onDisplay.Invoke();
            InternalDisplay();
        }

        [ContextMenu(nameof(Hide))]
        public void Hide()
        {
            onHide.Invoke();
            InternalHide();
        }

        public virtual void SwitchDisplay()
        {
            if (IsDisplay())
            {
                InternalHide();
            }
            else
            {
                InternalDisplay();
            }
        }
    }
}
