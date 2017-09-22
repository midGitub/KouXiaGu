using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace JiongXiaGu.Initialization
{


    /// <summary>
    /// 版本控制;
    /// </summary>
    public class BuildVersion
    {

        public static string Version
        {
            get { return Application.version; }
        }

    }

}
