using System.Collections.Generic;
using UnityEngine;
using KouXiaGu.KeyInput;
using UnityEngine.Events;

namespace KouXiaGu.UI
{

    /// <summary>
    /// 独立窗口,需要独享 返回 和 确定 功能键,
    /// 在显示后确保是在其它 Canvas 之上的;
    /// </summary>
    [DisallowMultipleComponent, RequireComponent(typeof(Canvas))]
    public class StandaloneUI : MonoBehaviour
    {
        protected StandaloneUI() { }


        [CustomUnityLayer]
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



        [SerializeField]
        UnityEvent OnDisplay;

        [SerializeField]
        UnityEvent OnConceal;

        Canvas canvas;

        EscapeKeyObserver escapeKeyObserver;

        EnterKeyObserver enterKeyObserver;

        protected Canvas Canvas
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


        /// <summary>
        /// 显示窗口;
        /// </summary>
        public void Display()
        {
            if (IsDisplay())
                return;

            SetSortingLayer();
            Subscribe();

            if(state.Last != null)
                state.Last.Value.OnCover();

            DisplayAction();
            OnUncover();

            state.AddLast(this);
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


        /// <summary>
        /// 隐藏窗口;
        /// </summary>
        public void Conceal()
        {
            if (!IsDisplay())
                return;
            if (Activate != this)
                throw new PremiseNotInvalidException("不为最前的UI;");

            Unsubscribe();

            if(state.Last.Previous != null)
                state.Last.Previous.Value.OnUncover();

            ConcealAction();

            state.RemoveLast();
        }

        public void ConcealImmediate()
        {
            try
            {
                Conceal();
            }
            catch (PremiseNotInvalidException)
            {
                Unsubscribe();
                ConcealAction();
                state.Remove(this);
            }
        }

        void Unsubscribe()
        {
            EscapeKeyObserver.Unsubscribe();
            EnterKeyObserver.Unsubscribe();
        }


        /// <summary>
        /// 是否为显示状态?
        /// </summary>
        public virtual bool IsDisplay()
        {
            return gameObject.activeSelf == true;
        }

        /// <summary>
        /// 请求显示这个UI;
        /// </summary>
        protected virtual void DisplayAction()
        {
            gameObject.SetActive(true);
        }

        /// <summary>
        /// 请求隐藏这个UI;
        /// </summary>
        protected virtual void ConcealAction()
        {
            gameObject.SetActive(false);
        }

        /// <summary>
        /// 不为最前的UI时;
        /// </summary>
        protected virtual void OnCover()
        {
            Canvas.renderMode = RenderMode.ScreenSpaceCamera;
            Canvas.worldCamera = Camera.main;
        }

        /// <summary>
        /// 重新成为最前的UI;
        /// </summary>
        protected virtual void OnUncover()
        {
            Canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        }

        /// <summary>
        /// 当按下返回时调用;
        /// </summary>
        protected virtual void OnEscapeKeyDown()
        {
            Conceal();
        }

        /// <summary>
        /// 当按下回车时调用;
        /// </summary>
        protected virtual void OnEnterKeyDown()
        {
            Conceal();
        }

    }

}
