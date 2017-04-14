using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System.Xml.Serialization;
using System.IO;
using KouXiaGu.Collections;

namespace KouXiaGu
{

    /// <summary>
    /// 按键读取;
    /// </summary>
    public abstract class CustomKeyReader
    {
        const string CustomKeyFileName = "Input\\Keyboard.xml";

        static string CustomKeyFilePath
        {
            get { return ResourcePath.CombineConfiguration(CustomKeyFileName); }
        }

        public abstract IEnumerable<CustomKey> ReadKeys(string filePath);
        public abstract void WriteKeys(IEnumerable<CustomKey> keys, string filePath);

        public virtual IEnumerable<CustomKey> ReadKeys()
        {
            return ReadKeys(CustomKeyFilePath);
        }

        public virtual void WriteKeys(IEnumerable<CustomKey> keys)
        {
            WriteKeys(keys, CustomKeyFilePath);
        }
    }

    /// <summary>
    /// 读取默认按键;
    /// </summary>
    public class DefaultCustomKeyReader : CustomKeyReader
    {
        const string DefaultCustomKeyFileName = "Input\\Default_Keyboard.xml";

        static string DefaultCustomKeyFilePath
        {
            get { return ResourcePath.CombineConfiguration(DefaultCustomKeyFileName); }
        }

        public DefaultCustomKeyReader(CustomKeyReader reader)
        {
            if (reader == null)
                throw new ArgumentNullException("reader");

            this.reader = reader;
        }

        CustomKeyReader reader;

        public override IEnumerable<CustomKey> ReadKeys()
        {
            return ReadKeys(DefaultCustomKeyFilePath);
        }

        public override void WriteKeys(IEnumerable<CustomKey> keys)
        {
            WriteKeys(keys, DefaultCustomKeyFilePath);
        }

        public override IEnumerable<CustomKey> ReadKeys(string filePath)
        {
            return reader.ReadKeys(filePath);
        }

        public override void WriteKeys(IEnumerable<CustomKey> keys, string filePath)
        {
            reader.WriteKeys(keys, filePath);
        }
    }

    public class XmlCustomKeyReader : CustomKeyReader
    {
        static readonly XmlSerializer keyArraySerializer = new XmlSerializer(typeof(CustomKey[]));

        public XmlCustomKeyReader()
        {
        }

        public override IEnumerable<CustomKey> ReadKeys(string filePath)
        {
            var customKeys = (CustomKey[])keyArraySerializer.DeserializeXiaGu(filePath);
            return customKeys;
        }

        public override void WriteKeys(IEnumerable<CustomKey> keys, string filePath)
        {
            var keyArray = keys as CustomKey[] ?? keys.ToArray();
            string directoryPath = Path.GetDirectoryName(filePath);

            if (!Directory.Exists(directoryPath))
                Directory.CreateDirectory(directoryPath);

            keyArraySerializer.SerializeXiaGu(filePath, keyArray, FileMode.Create);
        }

    }

    [XmlType("Key")]
    public struct CustomKey : IEquatable<CustomKey>
    {
        public CustomKey(KeyFunction function, KeyCode keyCode)
        {
            this.function = function;
            this.keyCode = keyCode;
        }

        [XmlAttribute("function")]
        public KeyFunction function { get; private set; }

        [XmlAttribute("key")]
        public KeyCode keyCode { get; private set; }

        public override bool Equals(object obj)
        {
            if (!(obj is CustomKey))
                return false;
            return Equals((CustomKey)obj);
        }

        public bool Equals(CustomKey other)
        {
            return  this.function == other.function &&
                this.keyCode == other.keyCode;
        }

        public override int GetHashCode()
        {
            return ((int)function) ^ ((int)keyCode);
        }

        public override string ToString()
        {
            return "Function:" + function.ToString() + ";Key:" + keyCode.ToString();
        }

    }

    /// <summary>
    /// 对 UnityEngine.Input 进行包装;
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



        internal static readonly CustomKeyReader defaultKeyReader = new XmlCustomKeyReader();

        /// <summary>
        /// 从读取到所有按键信息,并且设置到按键;
        /// </summary>
        public static void Read()
        {
            Read(defaultKeyReader);
        }

        /// <summary>
        /// 从读取到所有按键信息,并且设置到按键;
        /// </summary>
        public static void Read(CustomKeyReader keyReader)
        {
            var keys = keyReader.ReadKeys();
            UpdateKey(keys);
        }

        public static IAsyncOperation ReadAsync()
        {
            var item = new CustomInputAsyncReader(defaultKeyReader);
            item.Start();
            return item;
        }

        public static IAsyncOperation ReadAsync(CustomKeyReader keyReader)
        {
            var item = new CustomInputAsyncReader(keyReader);
            item.Start();
            return item;
        }

        class CustomInputAsyncReader : ThreadOperation
        {
            public CustomInputAsyncReader(CustomKeyReader keyReader)
            {
                KeyReader = keyReader;
            }

            public CustomKeyReader KeyReader { get; private set; }

            protected override void Operate()
            {
                CustomInput.Read(KeyReader);
            }
        }


        /// <summary>
        /// 将所有按键输出\保存;
        /// </summary>
        public static void Write()
        {
            Write(defaultKeyReader);
        }

        /// <summary>
        /// 将所有按键输出\保存;
        /// </summary>
        public static void Write(CustomKeyReader keyReader)
        {
            CustomKey[] keys = GetKeys().ToArray();
            keyReader.WriteKeys(keys);
        }

        public static IAsyncOperation WriteAsync()
        {
            var item = new CustomInputAsyncWriter(defaultKeyReader);
            item.Start();
            return item;
        }

        public static IAsyncOperation WriteAsync(CustomKeyReader keyReader)
        {
            var item = new CustomInputAsyncWriter(keyReader);
            item.Start();
            return item;
        }

        class CustomInputAsyncWriter : ThreadOperation
        {
            public CustomInputAsyncWriter(CustomKeyReader keyReader)
            {
                KeyReader = keyReader;
            }

            public CustomKeyReader KeyReader { get; private set; }

            protected override void Operate()
            {
                CustomInput.Write(KeyReader);
            }
        }

    }

}
