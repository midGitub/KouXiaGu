﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KouXiaGu.WorldEvents
{

    /// <summary>
    /// 效果,对世界产生影响的效果;
    /// </summary>
    public interface IEffect
    {

        /// <summary>
        /// 启用;
        /// </summary>
        void Enable();

        /// <summary>
        /// 停用;
        /// </summary>
        void Disable();

    }

}