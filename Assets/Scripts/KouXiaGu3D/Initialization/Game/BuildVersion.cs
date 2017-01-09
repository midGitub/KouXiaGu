using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace KouXiaGu.Initialization
{


    /// <summary>
    /// 版本控制;
    /// </summary>
    public class BuildVersion
    {

        public string Version
        {
            get { return Application.version; }
        }

    }

}
