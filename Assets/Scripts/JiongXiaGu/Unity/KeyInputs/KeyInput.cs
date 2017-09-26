using JiongXiaGu.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace JiongXiaGu.Unity.KeyInputs
{

    /// <summary>
    /// 输入控制(大部分方法仅限在Unity线程访问);
    /// </summary>
    public static class KeyInput
    {
        /// <summary>
        /// 枚举所有 KeyCode; 
        /// </summary>
        static readonly IEnumerable<KeyCode> EnumerateKeyCode = Enum.GetValues(typeof(KeyCode)).OfType<KeyCode>().ToArray();

        /// <summary>
        /// 当前使用的按键映射(若未定义则返回null);
        /// </summary>
        public static KeyMap CurrentKeyMap { get; internal set; }

        /// <summary>
        /// 若按键映射未定义则抛出异常;
        /// </summary>
        static void ThrowIfCurrentKeyMapIsNull()
        {
            if (CurrentKeyMap == null)
                throw new ArgumentNullException("[KeyInput]未定义有效的按键映射!");
        }

        /// <summary>
        /// 对应组合键是否被激活了?若按键映射未定义则抛出异常;
        /// </summary>
        public static bool IsKeyActivated(string name)
        {
            ThrowIfCurrentKeyMapIsNull();
            CombinationKey key = CurrentKeyMap[name];
            return IsKeyActivated(key);
        }

        /// <summary>
        /// 这个组合按键当前是否启用了?用户释放 相关按键 的关键帧时返回true;
        /// </summary>
        public static bool IsKeyActivated(CombinationKey combinationKey)
        {
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
        /// 获取到按着的按键;若不存在则返回空合集;
        /// </summary>
        public static List<KeyCode> GetHoldKeys(int maxKey)
        {
            if (maxKey <= 0)
                throw new ArgumentOutOfRangeException("maxKey");
            if (!Input.anyKey)
                return new List<KeyCode>();

            List<KeyCode> holdKeys = new List<KeyCode>();
            GetHoldKeys_internal(holdKeys, maxKey);
            return holdKeys;
        }

        /// <summary>
        /// 将所有按着的按键加入到 holdKeys 合集;
        /// </summary>
        /// <param name="maxKey">指定获取到的最大按键数;</param>
        public static void GetHoldKeys(ICollection<KeyCode> holdKeys, int maxKey = int.MaxValue)
        {
            if (holdKeys == null)
                throw new ArgumentNullException("holdKeys");
            if (maxKey <= 0)
                throw new ArgumentOutOfRangeException("maxKey");
            if (holdKeys.Count >= maxKey)
                throw new ArgumentOutOfRangeException("holdKeys合集已经为最大值");
            if (!Input.anyKey)
                return;

            GetHoldKeys_internal(holdKeys, maxKey);
        }

        /// <summary>
        /// 将所有按着的按键加入到 holdKeys 合集;
        /// </summary>
        /// <param name="maxKey">指定获取到的最大按键数;</param>
        static void GetHoldKeys_internal(ICollection<KeyCode> holdKeys, int maxKey)
        {
            foreach (var keyCode in EnumerateKeyCode)
            {
                if (Input.GetKey(keyCode) && !holdKeys.Contains(keyCode))
                {
                    holdKeys.Add(keyCode);
                    if (holdKeys.Count >= maxKey)
                    {
                        break;
                    }
                }
            }
        }
    }
}
