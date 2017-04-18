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
        static readonly KeyFunction[] functionArray = Enum.GetValues(typeof(KeyFunction)).
            Cast<KeyFunction>().ToArray();

        public static IEnumerable<KeyFunction> FunctionKeys
        {
            get { return functionArray; }
        }

        static readonly Dictionary<int, KeyCode> keyMap = 
            functionArray.ToDictionary(function => new KeyValuePair<int, KeyCode>((int)function, KeyCode.None));


        /// <summary>
        /// 获取到对应的 Unity.KeyCode;
        /// </summary>
        public static KeyCode GetKey(KeyFunction function)
        {
            KeyCode keycode = keyMap[(int)function];
            return keycode;
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


        /// <summary>
        /// 更新按键映射;
        /// </summary>
        static void UpdateKey(IEnumerable<CustomKey> customKeys)
        {
            foreach (var key in customKeys)
            {
                UpdateKey(key);
            }
        }

        /// <summary>
        /// 更新按键映射;
        /// </summary>
        public static void UpdateKey(CustomKey key)
        {
            UpdateKey(key.function, key.keyCode);
        }

        /// <summary>
        /// 更新按键映射;
        /// </summary>
        public static void UpdateKey(KeyFunction function, KeyCode key)
        {
            keyMap[(int)function] = key;
        }



        /// <summary>
        /// 获取到所有对应的按键信息;
        /// </summary>
        public static IEnumerable<CustomKey> GetKeys()
        {
            return keyMap.Select(pair => PairToCustomKey(pair));
        }

        static CustomKey PairToCustomKey(KeyValuePair<int, KeyCode> pair)
        {
            KeyFunction function = (KeyFunction)pair.Key;
            KeyCode key = pair.Value;
            return new CustomKey(function, key);
        }

        /// <summary>
        /// 获取到所有按键值为空的功能键;
        /// </summary>
        public static IEnumerable<KeyFunction> GetEmptyKeys()
        {
            foreach (var key in keyMap)
            {
                if (key.Value == KeyCode.None)
                {
                    KeyFunction function = (KeyFunction)key.Key;
                    yield return function;
                }
            }
        }

        /// <summary>
        /// 是为未设置 具体按键的 功能键;
        /// </summary>
        public static bool IsEmptyKey(KeyFunction function)
        {
            KeyCode keyCode = keyMap[(int)function];
            return keyCode == KeyCode.None;
        }



        internal static readonly CustomKeyReader DefaultReader = new XmlCustomKeyReader();


        public static void ReadOrDefault()
        {
            ReadOrDefault(DefaultReader);
        }

        public static void ReadOrDefault(CustomKeyReader reader)
        {
            IEnumerable<CustomKey> keys;
            try
            {
                keys = reader.ReadKeys();
            }
            catch
            {
                Debug.LogWarning("读取定义按键失败,现在使用默认按键;");
                keys = DefaultKey.DefaultKeys;
            }
            UpdateKey(keys);
        }

        public static IAsyncOperation ReadOrDefaultAsync()
        {
            return ReadOrDefaultAsync(DefaultReader);
        }

        public static IAsyncOperation ReadOrDefaultAsync(CustomKeyReader reader)
        {
            return new AsyncReadOrDefault(reader);
        }

        class AsyncReadOrDefault : ThreadOperation
        {
            public AsyncReadOrDefault(CustomKeyReader reader)
            {
                Reader = reader;
                Start();
            }

            public CustomKeyReader Reader { get; private set; }

            protected override void Operate()
            {
                ReadOrDefault(Reader);
            }
        }


        /// <summary>
        /// 从读取到所有按键信息,并且设置到按键;
        /// </summary>
        public static void Read()
        {
            Read(DefaultReader);
        }

        /// <summary>
        /// 从读取到所有按键信息,并且设置到按键,若读取失败则使用默认按键;
        /// </summary>
        public static void Read(CustomKeyReader reader)
        {
            IEnumerable<CustomKey> keys;
            keys = reader.ReadKeys();
            UpdateKey(keys);
        }

        public static IAsyncOperation ReadAsync()
        {
            return ReadAsync(DefaultReader);
        }

        public static IAsyncOperation ReadAsync(CustomKeyReader reader)
        {
            return new CustomInputAsyncReader(reader);
        }

        class CustomInputAsyncReader : ThreadOperation
        {
            public CustomInputAsyncReader(CustomKeyReader reader)
            {
                Reader = reader;
                Start();
            }

            public CustomKeyReader Reader { get; private set; }

            protected override void Operate()
            {
                Read(Reader);
            }
        }


        /// <summary>
        /// 将所有按键输出\保存;
        /// </summary>
        public static void Write()
        {
            Write(DefaultReader);
        }

        /// <summary>
        /// 将所有按键输出\保存;
        /// </summary>
        public static void Write(CustomKeyReader writer)
        {
            CustomKey[] keys = GetKeys().ToArray();
            writer.WriteKeys(keys);
        }

        public static IAsyncOperation WriteAsync()
        {
            return WriteAsync(DefaultReader);
        }

        public static IAsyncOperation WriteAsync(CustomKeyReader writer)
        {
            return new CustomInputAsyncWriter(writer);
        }

        class CustomInputAsyncWriter : ThreadOperation
        {
            public CustomInputAsyncWriter(CustomKeyReader writer)
            {
                Writer = writer;
                Start();
            }

            public CustomKeyReader Writer { get; private set; }

            protected override void Operate()
            {
                CustomInput.Write(Writer);
            }
        }

    }

}
