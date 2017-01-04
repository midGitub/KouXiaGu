using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

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


        [CustomSortingLayer]
        public const string SortingLayerName = "StandaloneUI";

        static readonly LinkedList<StandaloneUI> state = new LinkedList<StandaloneUI>();

        /// <summary>
        /// 当前在最前面的UI;
        /// </summary>
        public static StandaloneUI Activate
        {
            get { return state.Count != 0 ? state.Last.Value : null; }
        }

        static int SortingOrder
        {
            get { return state.Count; }
        }




        Canvas canvas;

        Canvas Canvas
        {
            get { return canvas ?? (canvas = GetComponent<Canvas>()); }
        }

        public void Display()
        {
            if (IsDisplay())
                return;

            state.AddLast(this);
            SetSortingLayer();

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

        public void Conceal()
        {
            if (!IsDisplay())
                return;
            if (Activate != this)
                throw new PremiseNotInvalidException("不为最前的UI;");

            state.RemoveLast();

            ConcealAction();
        }

        /// <summary>
        /// 立即隐藏;
        /// </summary>
        public void ConcealImmediate()
        {
            if (!IsDisplay())
                return;

            state.Remove(this);

            ConcealAction();
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

    }

}
