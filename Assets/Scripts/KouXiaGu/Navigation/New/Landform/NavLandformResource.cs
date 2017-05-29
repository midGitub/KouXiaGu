using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KouXiaGu.World;
using KouXiaGu.Resources;

namespace KouXiaGu.Navigation
{

    /// <summary>
    /// 导航信息资源;
    /// </summary>
    public class NavLandformResource
    {
        public Dictionary<int, NavLandform> infos { get; internal set; }
    }
}
