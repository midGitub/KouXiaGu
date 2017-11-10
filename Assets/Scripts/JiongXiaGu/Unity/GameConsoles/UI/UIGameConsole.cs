using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace JiongXiaGu.Unity.GameConsoles.UI
{

    /// <summary>
    /// 负责对控制台显示隐藏按键响应;
    /// </summary>
    [DisallowMultipleComponent]
    public sealed class UIGameConsole : MonoBehaviour
    {
        private UIGameConsole()
        {
        }

        [SerializeField]
        private RectTransform gameConsoleWindow;

        [SerializeField]
        private UIGameConsoleInput uIGameConsoleInput;

        /// <summary>
        /// 是否显示中?
        /// </summary>
        private bool IsDisplay
        {
            get { return gameConsoleWindow.gameObject.activeSelf; }
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.BackQuote))
            {
                SwitchDisplayConsoleWindow();
            }
        }

        /// <summary>
        /// 切换显示控制台窗口;
        /// </summary>
        [ContextMenu("SwitchDisplay")]
        private void SwitchDisplayConsoleWindow()
        {
            if (IsDisplay)
            {
                HideConsoleWindow();
            }
            else
            {
                DisplayConsoleWindow();
            }
        }

        /// <summary>
        /// 显示控制台窗口;
        /// </summary>
        private void DisplayConsoleWindow()
        {
            gameConsoleWindow.gameObject.SetActive(true);
            uIGameConsoleInput.ActivateInputField();
        }

        /// <summary>
        /// 隐藏控制台窗口;
        /// </summary>
        private void HideConsoleWindow()
        {
            gameConsoleWindow.gameObject.SetActive(false);
            uIGameConsoleInput.ClearInputField();
        }
    }
}
