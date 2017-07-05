using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using KouXiaGu.Diagnostics;

namespace KouXiaGu
{

    /// <summary>
    /// 全局的按键响应器;
    /// </summary>
    [DisallowMultipleComponent]
    class GlobalInputResponser : MonoBehaviour
    {
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.BackQuote))
            {
                ConsoleWindow.SetDisplay(true);
            }
        }
    }
}
