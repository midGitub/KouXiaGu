using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KouXiaGu.Terrain3D
{

    [Flags]
    public enum CollectionOperation
    {
        Unknown = 0,
        Add = 1,
        Remove = 2,
        Update = 4,
    }

}
