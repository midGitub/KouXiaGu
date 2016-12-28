using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KouXiaGu.Collections
{

    [Flags]
    public enum Operation
    {
        Unknown = 0,
        Add = 1,
        Remove = 2,
        Update = 4,
    }

}
