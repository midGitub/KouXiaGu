using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace JiongXiaGu.InputControl
{

    public interface IKeyInput : IDisposable
    {
        bool IsActivating { get; }

        bool GetKeyHold(KeyFunction function);
        bool GetKeyDown(KeyFunction function);
        bool GetKeyUp(KeyFunction function);

        bool GetKeyHold(KeyCode keyCode);
        bool GetKeyDown(KeyCode keyCode);
        bool GetKeyUp(KeyCode keyCode);
    }
}
