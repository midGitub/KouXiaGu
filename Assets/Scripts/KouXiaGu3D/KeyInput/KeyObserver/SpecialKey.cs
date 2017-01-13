using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace KouXiaGu.KeyInput
{

    /// <summary>
    /// 对回车 和 Esc 等按键的特殊监视方法;
    /// </summary>
    [DisallowMultipleComponent]
    public sealed class SpecialKey : GlobalSington<SpecialKey>
    {

        static readonly ResponseKeyStack escape = new ResponseKeyStack(KeyCode.Escape);

        static readonly ResponseKeyStack enter = new ResponseKeyStack(KeyCode.Return);

        public static ResponseKeyStack Escape
        {
            get { return escape; }
        }

        public static ResponseKeyStack Enter
        {
            get { return enter; }
        }

        void Update()
        {
            escape.OnUpdate();
            enter.OnUpdate();
        }

    }

}
