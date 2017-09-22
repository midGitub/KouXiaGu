using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine.UI;

namespace JiongXiaGu.UI
{

    /// <summary>
    /// 对UGUI方法的拓展;
    /// </summary>
    public static class uguiExtension
    {

        /// <summary>
        /// 设置变量,并且调用对应事件;
        /// </summary>
        public static void SetValue(this Toggle toggle, bool isOn)
        {
            toggle.isOn = isOn;
            toggle.onValueChanged.Invoke(isOn);
        }

        /// <summary>
        /// 设置变量,并且调用对应事件;
        /// </summary>
        public static void SetValue(this InputField inputField, string text)
        {
            inputField.text = text;
            inputField.onValueChanged.Invoke(text);
        }
    }
}
