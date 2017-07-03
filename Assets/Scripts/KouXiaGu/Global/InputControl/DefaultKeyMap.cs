using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KouXiaGu.InputControl
{

    /// <summary>
    /// 默认按键;
    /// </summary>
    [ConsoleMethodsClass]
    public sealed class DefaultKeyMap : CustomKeyMap, IEnumerable<KeyValuePair<int, KeyCode>>
    {
        DefaultKeyMap() : base(true)
        {
        }

        static readonly DefaultKeyMap defaultKeyMap = new DefaultKeyMap()
            {
                 { KeyFunction.Console_DisplayOrHide, KeyCode.BackQuote },

                 { KeyFunction.Camera_movement_up, KeyCode.W },
                 { KeyFunction.Camera_movement_down, KeyCode.S },
                 { KeyFunction.Camera_movement_left, KeyCode.A },
                 { KeyFunction.Camera_movement_right, KeyCode.D },
                 { KeyFunction.Camera_return, KeyCode.Space },
            };

        public void Add(KeyFunction func, KeyCode keyCode)
        {
            UpdateKey(func, keyCode);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <summary>
        /// 获取到一个默认按键映射实例;
        /// </summary>
        public static CustomKeyMap GetDefaultKeyMap()
        {
            return new CustomKeyMap(defaultKeyMap);
        }

        [ConsoleMethod("write_default_key", "将默认按键输出覆盖", IsDeveloperMethod = true)]
        public static void Write()
        {
            var serializer = new CustomKeyMapXmlSerializer();
            serializer.Write(GetDefaultKeyMap());
        }
    }
}
