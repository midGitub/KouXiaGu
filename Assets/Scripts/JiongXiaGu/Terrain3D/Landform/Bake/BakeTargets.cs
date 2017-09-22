using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JiongXiaGu.Terrain3D
{

    [Flags]
    public enum BakeTargets
    {
        None = 0,
        All = Landform | Road,
        Landform = 1,
        Road = 2,
    }

}
