using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using UnityEngine;

namespace KouXiaGu.KeyInput
{

    /// <summary>
    /// 默认按键;
    /// </summary>
    public class DefaultKey
    {
        static DefaultKey()
        {
            DefaultKeys = new ReadOnlyCollection<CustomKey>(defaultKeys);
        }

        /// <summary>
        /// 只读的默认按键合集;
        /// </summary>
        public static ReadOnlyCollection<CustomKey> DefaultKeys { get; private set; }


        static CustomKey[] defaultKeys = new CustomKey[]
            {
                new CustomKey(KeyFunction.Console_DisplayOrHide, KeyCode.BackQuote),

            };

    }

}
