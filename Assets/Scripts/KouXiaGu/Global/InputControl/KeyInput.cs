using System;
using UnityEngine;

namespace KouXiaGu.InputControl
{

    /// <summary>
    /// 对 UnityEngine.Input 进行包装,加入自定义按键;
    /// </summary>
    public static class KeyInput
    {
        static KeyInput()
        {
            OccupiedInput = new OccupiedInput();
        }

        /// <summary>
        /// 当前使用的按键映射;
        /// </summary>
        public static CustomKeyMap KeyMap { get; private set; }
        public static OccupiedInput OccupiedInput { get; private set; }

        /// <summary>
        /// 初始化;
        /// </summary>
        public static void Initialize()
        {
            var serializer = new CustomKeyMapXmlSerializer();
            CustomKeyMap keyMap;
            try
            {
                keyMap = serializer.Read();
            }
            catch(Exception ex)
            {
                Debug.LogWarning("未能读取自定义按键;" + ex);
                keyMap = DefaultKeyMap.GetDefaultKeyMap();
            }
            SetKeyMap(keyMap);
        }

        /// <summary>
        /// 更新按键映射;
        /// </summary>
        public static void SetKeyMap(CustomKeyMap keyMap)
        {
            KeyMap = keyMap;
        }

        /// <summary>
        /// 获取到对应的 Unity.KeyCode;98
        /// </summary>
        public static KeyCode GetKey(KeyFunction function)
        {
            if (KeyMap != null)
            {
                KeyCode keycode = KeyMap[(int)function];
                return keycode;
            }
            return KeyCode.None;
        }

        /// <summary>
        /// 用户有按着 相关按键 时一直返回true;
        /// </summary>
        public static bool GetKeyHold(KeyFunction function)
        {
            return KeyMap != null && KeyMap.GetKeyHold(function);
        }

        /// <summary>
        /// 用户开始按下 相关按键 关键帧时返回true。
        /// </summary>
        public static bool GetKeyDown(KeyFunction function)
        {
            return KeyMap != null && KeyMap.GetKeyDown(function);
        }

        /// <summary>
        /// 用户释放 相关按键 的关键帧时返回true。
        /// </summary>
        public static bool GetKeyUp(KeyFunction function)
        {
            return KeyMap != null && KeyMap.GetKeyUp(function);
        }
    }
}
