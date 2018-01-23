using JiongXiaGu.Unity.UI;
using UnityEngine;

namespace JiongXiaGu.Unity.GameConsoles.UI
{

    /// <summary>
    /// 负责对控制台显示隐藏按键响应;
    /// </summary>
    [DisallowMultipleComponent]
    [RequireComponent(typeof(DisplaySwitcher))]
    public sealed class UIGameConsole : MonoBehaviour
    {
        [SerializeField]
        private KeyCode displaySwitcherKey = KeyCode.BackQuote;

        [SerializeField]
        private UIGameConsoleInput uiGameConsoleInput;

        private DisplaySwitcher displaySwitcher;

        private void Awake()
        {
            displaySwitcher = GetComponent<DisplaySwitcher>();

            displaySwitcher.OnDisplay += uiGameConsoleInput.ClearInputField;
            displaySwitcher.OnHide += uiGameConsoleInput.ClearInputField;
        }

        private void Update()
        {
            if (Input.GetKeyDown(displaySwitcherKey))
            {
                displaySwitcher.SwitchDisplay();
            }
        }
    }
}
