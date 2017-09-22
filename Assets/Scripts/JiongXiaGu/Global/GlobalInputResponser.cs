using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using JiongXiaGu.Diagnostics;
using JiongXiaGu.OperationRecord;

namespace JiongXiaGu
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
                ConsoleWindow.Instance.Panel.Display();
            }


            if (Input.GetKey(KeyCode.LeftControl))
            {
                if (Input.GetKeyDown(KeyCode.Z))
                {
                    RecordManager.PerformUndo();
                }
                if (Input.GetKeyDown(KeyCode.Y))
                {
                    RecordManager.PerformRedo();
                }
            }
        }
    }
}
