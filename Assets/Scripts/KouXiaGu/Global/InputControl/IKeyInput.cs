using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KouXiaGu.InputControl
{

    public interface IKeyInput
    {
        bool GetKeyHold(KeyFunction function);
        bool GetKeyDown(KeyFunction function);
        bool GetKeyUp(KeyFunction function);
    }
}
