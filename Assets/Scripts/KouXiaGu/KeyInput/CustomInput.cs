using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System.Xml.Serialization;
using System.IO;
using KouXiaGu.Collections;

namespace KouXiaGu
{

    public interface ICustomKeyFiler
    {
        IEnumerable<CustomKey> ReadKeys();
        void WriteKeys(CustomKey[] keys);
    }

    public class XmlCustomKeyFiler : ICustomKeyFiler
    {
        public XmlCustomKeyFiler()
        {
        }

        const string CustomKeyFileName = "Input\\Keyboard.xml";

        static readonly XmlSerializer keyArraySerializer = new XmlSerializer(typeof(CustomKey[]));

        public IEnumerable<CustomKey> ReadKeys()
        {
            string filePath = GetFilePath();
            var customKeys = (CustomKey[])keyArraySerializer.DeserializeXiaGu(filePath);
            return customKeys;
        }

        public void WriteKeys(CustomKey[] keys)
        {
            string filePath = GetFilePath();
            string directoryPath = Path.GetDirectoryName(filePath);

            if (!Directory.Exists(directoryPath))
                Directory.CreateDirectory(directoryPath);

            keyArraySerializer.SerializeXiaGu(filePath, keys, FileMode.Create);
        }

        /// <summary>
        /// 获取到默认存放到的文件路径;
        /// </summary>
        public static string GetFilePath()
        {
            return ResourcePath.CombineConfiguration(CustomKeyFileName);
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

        internal static ICustomKeyFiler Filer { get; private set; }

        static CustomInput()
        {
            Filer = new XmlCustomKeyFiler();
        }

        internal static void Initialize()
        {
            Filer = new XmlCustomKeyFiler();
            ReadFromFile();

            var emptyKeys = GetEmptyKeys().ToList();
            if (emptyKeys.Count != 0)
            {
                Debug.LogWarning("未定义的按键:" + emptyKeys.ToLog());
            }
        }

        /// <summary>
        /// 从文件读取到所以按键信息;
        /// </summary>
        public static void ReadFromFile()
        {
            var keys = Filer.ReadKeys();
            UpdateKeyMap(keys);
        }

        static void UpdateKeyMap(IEnumerable<CustomKey> customKeys)
        {
            foreach (var key in customKeys)
            {
                SetKey(key);
            }
        }

        public static void SetKey(CustomKey key)
        {
            SetKey(key.function, key.keyCode);
        }

        public static void SetKey(KeyFunction function, KeyCode key)
        {
            keyMap[(int)function] = key;
        }

        /// <summary>
        /// 设置到按键,并且保存;
        /// </summary>
        public static void SetKeysAndWrite(IEnumerable<CustomKey> keys)
        {
            foreach (var key in keys)
            {
                SetKey(key);
            }
            WriteToFile();
        }

        /// <summary>
        /// 将所以按键输出\保存到文件;
        /// </summary>
        public static void WriteToFile()
        {
            CustomKey[] keys = GetKeys().ToArray();
            Filer.WriteKeys(keys);
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

        /// <summary>
        /// 获取到对应;
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

    }

}
