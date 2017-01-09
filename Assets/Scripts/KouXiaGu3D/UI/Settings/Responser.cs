using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KouXiaGu.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace KouXiaGu.UI
{

    /// <summary>
    /// 对子节点的所有 IResponsive 接口进行统一操作;
    /// </summary>
    [DisallowMultipleComponent]
    public class Responser : StandaloneUI
    {
        protected Responser() { }


        [SerializeField]
        Button btnApply;

        [SerializeField]
        Button btnRetrun;

        HashSet<IResponsive> observers;

        IResponsive[] responsive;


        void Awake()
        {
            observers = new HashSet<IResponsive>();
            responsive = GetComponentsInChildren<IResponsive>();

            btnApply.onClick.AddListener(ObserversApply);
            btnRetrun.onClick.AddListener(base.Conceal);

            ApplyInteractable();
        }

        void ObserversApply()
        {
            foreach (var observer in observers)
            {
                observer.OnApply();
            }
            observers.Clear();
            ApplyInteractable();
        }

        /// <summary>
        /// 订阅 应用 按钮事件;
        /// </summary>
        public void SubscribeApply(IResponsive observer)
        {
            if (observer == null)
                throw new ArgumentNullException();

            observers.Add(observer);
            ApplyInteractable();
        }

        /// <summary>
        /// 取消订阅 应用 按钮事件;
        /// </summary>
        public void UnsubscribeApply(IResponsive observer)
        {
            if (observer == null)
                throw new ArgumentNullException();

            observers.Remove(observer);
            ApplyInteractable();
        }


        /// <summary>
        /// 设置 应用 按钮是否生效;
        /// </summary>
        void ApplyInteractable()
        {
            if (observers.Count == 0)
                btnApply.interactable = false;
            else
                btnApply.interactable = true;
        }

        /// <summary>
        /// 当隐藏界面时恢复所有设置;
        /// </summary>
        protected override void DisplayAction()
        {
            base.DisplayAction();
            ResetAll();
            ApplyInteractable();
        }

        void ResetAll()
        {
            foreach (var observer in observers)
            {
                observer.OnReset();
            }
        }

    }

}
