using JiongXiaGu.Unity.UI;
using UnityEngine;

namespace JiongXiaGu.Unity.GameConsoles.UI
{

    /// <summary>
    /// 负责对控制台显示隐藏按键响应;
    /// </summary>
    [DisallowMultipleComponent]
    public sealed class UIGameConsole : AnimatorDisplaySwitcher
    {
        [SerializeField]
        private KeyCode displaySwitcherKey = KeyCode.BackQuote;

        [SerializeField]
        private UIGameConsoleInput uiGameConsoleInput;

        private void Update()
        {
            if (Input.GetKeyDown(displaySwitcherKey))
            {
                SwitchDisplay();
            }
        }

        public override void Display()
        {
            base.Display();
            uiGameConsoleInput.ActivateInputField();
        }

        public override void Hide()
        {
            base.Hide();
            uiGameConsoleInput.ClearInputField();
        }
    }
}
