using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KouXiaGu
{


    [Flags]
    public enum FunctionKey : int
    {
        Unknown = 0,

        Mouse_Confirm = 1,
        Mouse_Cancel = 2,

        CameraRotateLeft = 4,
        CameraRotateRigth = 8,
        CameraDrag = 16,

        ObjectRotateLeft = 32,
        ObjectRotateRight = 64,

        Esc = 128,
        Ent =256,
    }

}
