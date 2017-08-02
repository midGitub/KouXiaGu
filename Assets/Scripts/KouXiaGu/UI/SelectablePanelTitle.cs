using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace KouXiaGu.UI
{

    /// <summary>
    /// 可选择面板的标题控制;
    /// </summary>
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Image))]
    public class SelectablePanelTitle : MonoBehaviour, IPointerDownHandler
    {
        SelectablePanelTitle()
        {
        }

        public enum StyleTypes
        {
            Self,
            Global,
        }

        [SerializeField]
        StyleTypes style = StyleTypes.Global;
        [SerializeField]
        SelectablePanel parent;
        [SerializeField]
        Image titleImageObject;
        [SerializeField]
        Text titleTextObject;
        [SerializeField]
        Image closeImageObject;
        [SerializeField]
        SelectablePanelTitleStyleInfo selfStyleInfo = DefaultStyleInfo;
        UIStyleManager uiStyleManager;
        string title;
        bool isShowingWarning;
        bool isWaitOperation;
        const string waitOperationMark = "*";

        /// <summary>
        /// 是否正在显示异常?
        /// </summary>
        public bool IsShowingWarning
        {
            get { return isShowingWarning; }
        }

        /// <summary>
        /// 是否存在等待操作?在标题栏显示 * ?
        /// </summary>
        public bool IsWaitOperation
        {
            get { return isWaitOperation; }
        }

        /// <summary>
        /// 标题;
        /// </summary>
        public string Title
        {
            get { return title; }
        }

        public StyleTypes Style
        {
            get { return style; }
        }

        public SelectablePanelTitleStyleInfo StyleInfo
        {
            get { return selfStyleInfo; }
            set { selfStyleInfo = value; }
        }

        /// <summary>
        /// 默认的样式;
        /// </summary>
        public static SelectablePanelTitleStyleInfo DefaultStyleInfo
        {
            get
            {
                return new SelectablePanelTitleStyleInfo()
                {
                    OnFocusColor = ColorExtensions.New(240, 80, 80),
                    OnBlurColor = ColorExtensions.New(240, 120, 120),
                    CloseImageColor = ColorExtensions.New(25, 25, 25, 255),
                };
            }
        }

        void Awake()
        {
            if (parent == null)
            {
                Debug.LogError("未指定 SelectablePanel 脚本!");
                Destroy(this);
                return;
            }

            uiStyleManager = UIStyleManager.Instance;
            if (uiStyleManager == null)
            {
                Debug.LogWarning("未找到 UIStyleManager !");
            }

            if (titleTextObject != null)
            {
                title = titleTextObject.text;
            }

            parent.OnFocusEvent += UseOnFocusColor;
            parent.OnBlurEvent += UseOnBlurColor;
            ResetState();
        }

        void OnValidate()
        {
            ResetState();
        }

        /// <summary>
        /// 设置标题;
        /// </summary>
        public void SetTitle(string title)
        {
            this.title = title;
            if (titleTextObject != null)
            {
                if (isWaitOperation)
                {
                    title += waitOperationMark;
                }
                titleTextObject.text = title;
            }
        }

        /// <summary>
        /// 启用等待操作标识;
        /// </summary>
        [ContextMenu("AppayWaitOperation")]
        public void AppayWaitOperation()
        {
            if (titleTextObject != null && !isWaitOperation)
            {
                string title = this.title + waitOperationMark;
                titleTextObject.text = title;
                isWaitOperation = true;
            }
        }

        /// <summary>
        /// 取消等待操作标识;
        /// </summary>
        [ContextMenu("CancelWaitOperation")]
        public void CancelWaitOperation()
        {
            if (titleTextObject != null && isWaitOperation)
            {
                titleTextObject.text = title;
                isWaitOperation = false;
            }
        }

        void ResetState()
        {
            var styleInfo = GetStyleInfo();
            if (parent != null)
            {
                if (parent.IsFocus)
                {
                    UseOnFocusColor(styleInfo);
                }
                else
                {
                    UseOnBlurColor(styleInfo);
                }
            }
            if (closeImageObject != null)
            {
                closeImageObject.color = styleInfo.CloseImageColor;
            }
        }

        void UseOnFocusColor()
        {
            var styleInfo = GetStyleInfo();
            UseOnFocusColor(styleInfo);
        }

        void UseOnFocusColor(SelectablePanelTitleStyleInfo styleInfo)
        {
            if (titleImageObject != null)
            {
                titleImageObject.color = styleInfo.OnFocusColor;
            }
        }

        void UseOnBlurColor()
        {
            var styleInfo = GetStyleInfo();
            UseOnBlurColor(styleInfo);
        }

        void UseOnBlurColor(SelectablePanelTitleStyleInfo styleInfo)
        {
            if (titleImageObject != null)
            {
                titleImageObject.color = styleInfo.OnBlurColor;
            }
        }

        SelectablePanelTitleStyleInfo GetStyleInfo()
        {
            if (style == StyleTypes.Self)
            {
                return selfStyleInfo;
            }
            else
            {
                if (uiStyleManager == null)
                {
                    return selfStyleInfo;
                }
                else
                {
                    return uiStyleManager.SelectablePanelTitleStyleInfo;
                }
            }
        }

        /// <summary>
        /// 闪动显示标题栏;
        /// </summary>
        public void ApplyWarning(int time = 2)
        {
            if (!isShowingWarning)
            {
                isShowingWarning = true;
                StartCoroutine(WarningCoroutine(time));
            }
        }

        IEnumerator WarningCoroutine(int time)
        {
            const float seconds = 0.2f;
            var styleInfo = GetStyleInfo();
            for (int i = 0; i < time; i++)
            {
                UseOnFocusColor(styleInfo);
                yield return new WaitForSecondsRealtime(seconds);
                UseOnBlurColor(styleInfo);
                yield return new WaitForSecondsRealtime(seconds);
            }
            ResetState();
            isShowingWarning = false;
        }

        void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
        {
            parent.OnFocus();
        }
    }
}
