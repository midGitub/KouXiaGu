﻿using System;
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
                Hide();
            }
            else
            {
                Display();
            }
        }
    }
}
