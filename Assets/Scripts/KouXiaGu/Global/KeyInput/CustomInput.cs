using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System.Xml.Serialization;
using System.IO;
using KouXiaGu.Collections;

namespace KouXiaGu.KeyInput
{

    /// <summary>
    /// 对 UnityEngine.Input 进行包装,加入自定义按键;
    /// </summary>
    public static class CustomInput
    {
        /// <summary>
        /// 当前使用的按键映射;
        /// </summary>
        public static CustomKeyMap KeyMap { get; private set; }

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

        public static void SetKeyMap(CustomKeyMap keyMap)
        {
            KeyMap = keyMap;
        }


        /// <summary>
        /// 获取到对应的 Unity.KeyCode;
        /// </summary>
        static KeyCode GetKey(KeyFunction function)
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
        public static bool GetKeyHoldDown(KeyFunction function)
        {
            KeyCode keycode = GetKey(function);
            return Input.GetKey(keycode);
        }

        /// <summary>
        /// 用户开始按下 相关按键 关键帧时返回true。
        /// </summary>
        public static bool GetKeyDown(KeyFunction function)
        {
            KeyCode keycode = GetKey(function);
            return Input.GetKeyDown(keycode);
        }

        /// <summary>
        /// 用户释放 相关按键 的关键帧时返回true。
        /// </summary>
        public static bool GetKeyUp(KeyFunction function)
        {
            KeyCode keycode = GetKey(function);
            return Input.GetKeyUp(keycode);
        }
    }
}
