using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;

namespace JiongXiaGu.Unity.UI
{

    /// <summary>
    /// 提供预制的UI组件;
    /// </summary>
    public sealed class PrefabUI : MonoBehaviour
    {
        private PrefabUI()
        {
        }

        private static readonly GlobalSingleton<PrefabUI> singleton = new GlobalSingleton<PrefabUI>();
        public static PrefabUI Instance => singleton.GetInstance();

        [SerializeField]
        private UIModificationController uIModificationControllerPrefab;
        public UIModificationController UIModificationControllerPrefab => uIModificationControllerPrefab;

        [SerializeField]
        private PrefabMessageWindow uiMessageWindowPrefab;
        public PrefabMessageWindow UIMessageWindowPrefab => uiMessageWindowPrefab;

        [SerializeField]
        private PrefabButton buttonPrefab;
        public PrefabButton ButtonPrefab => buttonPrefab;

        /// <summary>
        /// 创建一个错误消息窗口;
        /// </summary>
        /// <param name="onConfirm">当点击确认时的操作,若为Null则无操作</param>
        public PrefabMessageWindow CreateErrorInfoWindow(Transform parent, Exception ex, UnityAction onConfirm)
        {
            if (parent == null)
                throw new ArgumentNullException(nameof(parent));
            if (ex == null)
                throw new ArgumentNullException(nameof(ex));

            PrefabMessageWindow prefab = uiMessageWindowPrefab;
            var instance = GameObject.Instantiate(prefab, parent);

            instance.TitleMessageText.text = "Error";
            instance.MessageText.text = ex.ToString();
            instance.MultipleChoices.Clear();

            var confirmButton = GameObject.Instantiate(buttonPrefab, instance.MultipleChoices.Transform);
            confirmButton.TextObject.text = "Confirm";
            confirmButton.ButtonObject.onClick.AddListener(() => GameObject.Destroy(instance.gameObject));
            if (onConfirm != null)
            {
                confirmButton.ButtonObject.onClick.AddListener(onConfirm);
            }

            return instance;
        }

        /// <summary>
        /// 创建一个消息窗口;
        /// </summary>
        /// <param name="onConfirm">当点击确认时的操作,若为Null则无操作</param>
        public PrefabMessageWindow CreateInfoWindow(Transform parent, string titleMessage, string message, params ButtonInfo[] buttonInfos)
        {
            if (parent == null)
                throw new ArgumentNullException(nameof(parent));

            PrefabMessageWindow prefab = uiMessageWindowPrefab;
            var instance = GameObject.Instantiate(prefab, parent);

            instance.TitleMessageText.text = titleMessage;
            instance.MessageText.text = message;

            instance.MultipleChoices.Clear();
            CreateButtons(instance.MultipleChoices.Transform, buttonInfos);

            return instance;
        }

        /// <summary>
        /// 创建多个按钮;
        /// </summary>
        public void CreateButtons(Transform parent, ButtonInfo[] buttonInfos)
        {
            if (buttonInfos != null || buttonInfos.Length != 0)
            {
                foreach (var buttonInfo in buttonInfos)
                {
                    var button = GameObject.Instantiate(buttonPrefab, parent);
                    button.TextObject.text = buttonInfo.Name;
                    if (buttonInfo.Action != null)
                    {
                        button.ButtonObject.onClick.AddListener(buttonInfo.Action);
                    }
                }
            }
        }



        [SerializeField]
        private Transform transform1;

        [ContextMenu("Test")]
        private void Test()
        {
            CreateErrorInfoWindow(transform1, new ArgumentNullException("234234"), null);
        }
    }
}
