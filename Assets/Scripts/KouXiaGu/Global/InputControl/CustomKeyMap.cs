using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System.Xml.Serialization;

namespace KouXiaGu.InputControl
{

    /// <summary>
    /// 按键映射;
    /// </summary>
    [XmlRoot("KeyMap")]
    public class CustomKeyMap
    {
        /// <summary>
        /// 提供xml序列化使用的构造函数;
        /// </summary>
        protected CustomKeyMap()
        {
        }

        /// <summary>
        /// 初始化为空的构造函数,参数不影响;
        /// </summary>
        public CustomKeyMap(bool none)
        {
            keyMap = ToDictionary();
        }

        public CustomKeyMap(IDictionary<int, KeyCode> keys)
        {
            keyMap = new Dictionary<int, KeyCode>(keys);
            Standardize(keyMap);
        }

        public CustomKeyMap(CustomKeyMap map)
        {
            keyMap = new Dictionary<int, KeyCode>(map.keyMap);
        }

        static readonly KeyFunction[] functionArray = Enum.GetValues(typeof(KeyFunction)).Cast<KeyFunction>().ToArray();
        protected Dictionary<int, KeyCode> keyMap;

        /// <summary>
        /// 提供序列化使用的Keys;
        /// </summary>
        [XmlArray("Keys")]
        [XmlArrayItem("Key")]
        public CustomKey[] _SerializableKeys
        {
            get { return ToArray(keyMap); }
            set { keyMap = ToDictionary(value); }
        }

        [XmlIgnore]
        public int Count
        {
            get { return keyMap.Count; }
        }

        public KeyCode this[int functionKey]
        {
            get { return keyMap[functionKey]; }
            set { keyMap[functionKey] = value; }
        }

        Dictionary<int, KeyCode> ToDictionary()
        {
            Dictionary<int, KeyCode> dictionary = new Dictionary<int, KeyCode>();
            foreach (var item in functionArray)
            {
                dictionary.Add((int)item, KeyCode.None);
            }
            return dictionary;
        }

        Dictionary<int, KeyCode> ToDictionary(IEnumerable<CustomKey> keys)
        {
            if (keys != null)
            {
                Dictionary<int, KeyCode> dictionary = new Dictionary<int, KeyCode>();
                foreach (var key in keys)
                {
                    if (key.function == KeyFunction.None)
                    {
                        continue;
                    }

                    int func = (int)key.function;
                    if (!dictionary.ContainsKey(func))
                    {
                        dictionary.Add(func, key.keyCode);
                    }
                    else
                    {
                        Debug.LogWarning("自定义按键重复定义键" + key.function.ToString());
                    }
                }
                Standardize(dictionary);
                return dictionary;
            }
            return ToDictionary();
        }

        /// <summary>
        /// 补全Key参数;
        /// </summary>
        protected void Standardize(Dictionary<int, KeyCode> dictionary)
        {
            foreach (var function in functionArray)
            {
                int func = (int)function;
                if (!dictionary.ContainsKey(func))
                {
                    dictionary.Add(func, KeyCode.None);
                }
            }
        }

        CustomKey[] ToArray(IDictionary<int, KeyCode> keyMap)
        {
            CustomKey[] array = new CustomKey[keyMap.Count];
            int index = 0;
            foreach (var item in keyMap)
            {
                array[index++] = new CustomKey((KeyFunction)item.Key, item.Value);
            }
            return array;
        }

        public IEnumerator<KeyValuePair<int, KeyCode>> GetEnumerator()
        {
            return keyMap.GetEnumerator();
        }

        /// <summary>
        /// 更新按键映射;
        /// </summary>
        public void UpdateKeys(IEnumerable<KeyValuePair<int, KeyCode>> customKeys)
        {
            foreach (var key in customKeys)
            {
                keyMap[key.Key] = key.Value;
            }
        }

        /// <summary>
        /// 更新按键映射;
        /// </summary>
        public void UpdateKeys(IEnumerable<CustomKey> customKeys)
        {
            foreach (var key in customKeys)
            {
                UpdateKey(key);
            }
        }

        /// <summary>
        /// 更新按键映射;
        /// </summary>
        public void UpdateKey(CustomKey key)
        {
            keyMap[(int)key.function] = key.keyCode;
        }

        /// <summary>
        /// 更新按键映射;
        /// </summary>
        public void UpdateKey(KeyFunction function, KeyCode key)
        {
            keyMap[(int)function] = key;
        }

        /// <summary>
        /// 获取到所有按键值为空的功能键;
        /// </summary>
        public IEnumerable<KeyFunction> GetEmptyKeys()
        {
            foreach (var key in keyMap)
            {
                if (key.Value == KeyCode.None && key.Key != 0)
                {
                    KeyFunction function = (KeyFunction)key.Key;
                    yield return function;
                }
            }
        }

        /// <summary>
        /// 获取到对应的 Unity.KeyCode;
        /// </summary>
        KeyCode GetKey(KeyFunction function)
        {
            KeyCode keycode = keyMap[(int)function];
            return keycode;
        }

        /// <summary>
        /// 用户有按着 相关按键 时一直返回true;
        /// </summary>
        public bool GetKeyHold(KeyFunction function)
        {
            KeyCode keycode = GetKey(function);
            return Input.GetKey(keycode);
        }

        /// <summary>
        /// 用户开始按下 相关按键 关键帧时返回true。
        /// </summary>
        public bool GetKeyDown(KeyFunction function)
        {
            KeyCode keycode = GetKey(function);
            return Input.GetKeyDown(keycode);
        }

        /// <summary>
        /// 用户释放 相关按键 的关键帧时返回true。
        /// </summary>
        public bool GetKeyUp(KeyFunction function)
        {
            KeyCode keycode = GetKey(function);
            return Input.GetKeyUp(keycode);
        }
    }
}
