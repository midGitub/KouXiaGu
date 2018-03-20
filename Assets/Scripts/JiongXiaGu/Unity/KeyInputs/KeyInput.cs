using JiongXiaGu.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace JiongXiaGu.Unity.KeyInputs
{

    /// <summary>
    /// 输入控制(仅限在Unity线程访问);
    /// </summary>
    public static class KeyInput
    {
        /// <summary>
        /// 枚举所有 KeyCode; 
        /// </summary>
        private static readonly IEnumerable<KeyCode> EnumerateKeyCode = Enum.GetValues(typeof(KeyCode)).OfType<KeyCode>().ToArray();

        /// <summary>
        /// 当前使用的按键映射(若未定义则返回null);
        /// </summary>
        public static IDictionary<string, KeyCode> CurrentKeyMap { get; internal set; }


        internal static void Initialize()
        {
            KeyMapFactroy factroy = new KeyMapFactroy();
            CurrentKeyMap = factroy.ReadKeyMap();
        }

        public static bool GetKeyDown(string name)
        {
            KeyCode key = CurrentKeyMap[name];
            return Input.GetKeyDown(key);
        }

        public static bool GetKeyUp(string name)
        {
            KeyCode key = CurrentKeyMap[name];
            return Input.GetKeyUp(key);
        }

        public static bool GetKey(string name)
        {
            KeyCode key = CurrentKeyMap[name];
            return Input.GetKey(key);
        }

        /// <summary>
        /// 这个组合按键当前是否启用了?
        /// </summary>
        public static bool GetKeyDown(CombinationKey combinationKey)
        {
            if (combinationKey.IsEmpty)
                return false;

            bool isKeyDown = false;

            foreach (var key in combinationKey.Keys)
            {
                if (!Input.GetKey(key))
                {
                    return false;
                }
                if (!isKeyDown && Input.GetKeyDown(key))
                {
                    isKeyDown = true;
                }
            }

            return isKeyDown;
        }

        /// <summary>
        /// 该组合按键是否抬起?
        /// </summary>
        public static bool IsKeyUp(CombinationKey combinationKey)
        {
            if (combinationKey.IsEmpty)
                return false;

            bool isKeyUp = false;

            foreach (var key in combinationKey.Keys)
            {
                if (!Input.GetKey(key))
                {
                    if (Input.GetKeyUp(key))
                    {
                        isKeyUp = true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }

            return isKeyUp;
        }

        /// <summary>
        /// 是否按住当前组合键?
        /// </summary>
        public static bool IsKeyHold(CombinationKey combinationKey)
        {
            if (combinationKey.IsEmpty)
                return false;

            foreach (var key in combinationKey.Keys)
            {
                if (!Input.GetKey(key))
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// 枚举所有按着的按键;
        /// </summary>
        public static IEnumerable<KeyCode> EnumerateHoldKeys()
        {
            if (!Input.anyKey)
                yield break;

            foreach (var keyCode in EnumerateKeyCode)
            {
                if (Input.GetKey(keyCode))
                {
                    yield return keyCode;
                }
            }
        }
    }
}
