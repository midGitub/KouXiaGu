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
        public static KeyMap GlobalKeyMap { get; set; }



        public static bool IsKeyDown(string name)
        {
            CombinationKey combinationKey;
            if (GlobalKeyMap != null && GlobalKeyMap.Dictionary.TryGetValue(name, out combinationKey))
            {
                return IsKeyDown(combinationKey);
            }
            else
            {
                throw new KeyNotFoundException(string.Format("未找到对应按键映射[{0}]", name));
            }
        }

        public static bool IsKeyUp(string name)
        {
            CombinationKey combinationKey;
            if (GlobalKeyMap != null && GlobalKeyMap.Dictionary.TryGetValue(name, out combinationKey))
            {
                return IsKeyUp(combinationKey);
            }
            else
            {
                throw new KeyNotFoundException(string.Format("未找到对应按键映射[{0}]", name));
            }
        }

        public static bool IsKeyHold(string name)
        {
            CombinationKey combinationKey;
            if (GlobalKeyMap != null && GlobalKeyMap.Dictionary.TryGetValue(name, out combinationKey))
            {
                return IsKeyHold(combinationKey);
            }
            else
            {
                throw new KeyNotFoundException(string.Format("未找到对应按键映射[{0}]", name));
            }
        }


        /// <summary>
        /// 这个组合按键当前是否启用了?
        /// </summary>
        public static bool IsKeyDown(CombinationKey combinationKey)
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


        ///// <summary>
        ///// 获取到按着的按键;若不存在则返回空合集;
        ///// </summary>
        //public static List<KeyCode> GetHoldKeys(int maxKey)
        //{
        //    if (maxKey <= 0)
        //        throw new ArgumentOutOfRangeException(nameof(maxKey));
        //    if (!Input.anyKey)
        //        return new List<KeyCode>();

        //    List<KeyCode> holdKeys = new List<KeyCode>();

        //    foreach (var keyCode in EnumerateKeyCode)
        //    {
        //        if (Input.GetKey(keyCode))
        //        {
        //            holdKeys.Add(keyCode);
        //            if (holdKeys.Count >= maxKey)
        //            {
        //                break;
        //            }
        //        }
        //    }

        //    return holdKeys;
        //}

        ///// <summary>
        ///// 将所有按着的按键加入到 holdKeys 合集;
        ///// </summary>
        ///// <param name="maxKey">指定获取到的最大按键数;</param>
        //public static void GetHoldKeys(ICollection<KeyCode> holdKeys, int maxKey = int.MaxValue)
        //{
        //    if (holdKeys == null)
        //        throw new ArgumentNullException("holdKeys");
        //    if (maxKey <= 0)
        //        throw new ArgumentOutOfRangeException("maxKey");
        //    if (holdKeys.Count >= maxKey)
        //        throw new ArgumentOutOfRangeException("holdKeys合集已经为最大值");
        //    if (!Input.anyKey)
        //        return;

        //    GetHoldKeys_internal(holdKeys, maxKey);
        //}

        ///// <summary>
        ///// 将所有按着的按键加入到 holdKeys 合集;
        ///// </summary>
        ///// <param name="maxKey">指定获取到的最大按键数;</param>
        //private static void GetHoldKeys_internal(ICollection<KeyCode> holdKeys, int maxKey)
        //{
        //    foreach (var keyCode in EnumerateKeyCode)
        //    {
        //        if (Input.GetKey(keyCode) && !holdKeys.Contains(keyCode))
        //        {
        //            holdKeys.Add(keyCode);
        //            if (holdKeys.Count >= maxKey)
        //            {
        //                break;
        //            }
        //        }
        //    }
        //}
    }
}
