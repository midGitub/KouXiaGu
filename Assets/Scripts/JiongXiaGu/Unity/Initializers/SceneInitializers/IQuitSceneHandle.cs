﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JiongXiaGu.Unity
{

    /// <summary>
    /// 当退出场景时进行的操作;
    /// </summary>
    public interface IQuitSceneHandle
    {
        Task OnQuitScene();
    }
}
