using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using KouXiaGu.KeyInput;

namespace KouXiaGu.UI
{

    /// <summary>
    /// 独立窗口,需要独享 返回 和 确定 功能键,
    /// 在显示后确保是在其它 Canvas 之上的;
    /// </summary>
    [DisallowMultipleComponent, RequireComponent(typeof(Canvas))]
    public class StandaloneUI : MonoBehaviour
    {

        [CustomSortingLayer]
        public const string SortingLayerName = "StandaloneUI";

        static readonly LinkedList<StandaloneUI> state = new LinkedList<StandaloneUI>();

        /// <summary>
        /// 当前在最顶部的UI;
        /// </summary>
        public static StandaloneUI Activate
        {
            get { return state.Count != 0 ? state.Last.Value : null; }
        }

        static int SortingOrder
        {
            get { return state.Count; }
        }



        protected StandaloneUI() { }

        Canvas canvas;

        EscapeKeyObserver escapeKeyObserver;

        EnterKeyObserver enterKeyObserver;

        Canvas Canvas
        {
            get { return canvas ?? (canvas = GetComponent<Canvas>()); }
        }

        EscapeKeyObserver EscapeKeyObserver
        {
            get { return escapeKeyObserver ?? (escapeKeyObserver = new EscapeKeyObserver(OnEscapeKeyDown)); }
        }

        EnterKeyObserver EnterKeyObserver
        {
            get { return enterKeyObserver ?? (enterKeyObserver = new EnterKeyObserver(OnEnterKeyDown)); }
        }

        public void Display()
        {
            if (IsDisplay())
                return;

            state.AddLast(this);
            SetSortingLayer();
            Subscribe();

            DisplayAction();
        }

        void SetSortingLayer()
        {
#if UNITY_EDITOR
            if (Application.isPlaying)
            {
#endif

                Canvas.sortingLayerName = SortingLayerName;
                Canvas.sortingOrder = SortingOrder;

#if UNITY_EDITOR
            }
#endif
        }

        void Subscribe()
        {
            EscapeKeyObserver.Subscribe();
            EnterKeyObserver.Subscribe();
        }


        public void Conceal()
        {
            if (!IsDisplay())
                return;
            if (Activate != this)
                throw new PremiseNotInvalidException("不为最前的UI;");

            state.RemoveLast();
            Unsubscribe();

            ConcealAction();
        }

        public void ConcealImmediate()
        {
            if (!IsDisplay())
                return;

            state.Remove(this);
            Unsubscribe();

            ConcealAction();
        }

        void Unsubscribe()
        {
            EscapeKeyObserver.Unsubscribe();
            EnterKeyObserver.Unsubscribe();
        }


        public virtual bool IsDisplay()
        {
            return gameObject.activeSelf == true;
        }

        protected virtual void DisplayAction()
        {
            gameObject.SetActive(true);
        }

        protected virtual void ConcealAction()
        {
            gameObject.SetActive(false);
        }

        protected virtual void OnEscapeKeyDown()
        {
            Conceal();
        }

        protected virtual void OnEnterKeyDown()
        {
            Conceal();
        }

    }

}
