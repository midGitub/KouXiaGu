using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System.Xml.Serialization;
using System.IO;

namespace KouXiaGu
{

    /// <summary>
    /// 对 UnityEngine.Input 进行包装;
    /// </summary>
    [DisallowMultipleComponent]
    public class CustomInput : MonoBehaviour
    {

        internal static readonly KeyFunction[] Functions = Enum.GetValues(typeof(KeyFunction)).Cast<KeyFunction>().ToArray();

        /// <summary>
        /// 按键映射表;
        /// </summary>
        static readonly Dictionary<int, KeyCode> keyMap = 
            Functions.ToDictionary(function => new KeyValuePair<int, KeyCode>((int)function, KeyCode.None));


        /// <summary>
        /// 获取到所有按键值为空的功能键;
        /// </summary>
        public static List<KeyFunction> EmptyKeys()
        {
            List<KeyFunction> noneFunctionKeys = new List<KeyFunction>();
            foreach (var key in keyMap)
            {
                KeyFunction function = (KeyFunction)key.Key;

                if (IsEmptyKes(function))
                    noneFunctionKeys.Add(function);
            }
            return noneFunctionKeys;
        }

        /// <summary>
        /// 是为未设置 具体按键的 功能键;
        /// </summary>
        public static bool IsEmptyKes(KeyFunction function)
        {
            KeyCode keyCode = keyMap[(int)function];
            return keyCode == KeyCode.None;
        }

        /// <summary>
        /// 设置按键到映射表(但是不做保存);
        /// </summary>
        public static void SetKey(KeyFunction function, KeyCode keyCode)
        {
            keyMap[(int)function] = keyCode;
        }

        /// <summary>
        /// 对按键进行转换;
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

        #region 按键映射储存为XML

        const string CUSTIM_KEY_FILE_NAME = "Input\\Keyboard.xml";

        static readonly XmlSerializer customKeyListSerializer = new XmlSerializer(typeof(List<CustomKey>));

        public static XmlSerializer CustomKeyListSerializer
        {
            get { return customKeyListSerializer; }
        }

        /// <summary>
        /// 将按键信息已XML格式,保存到预定义的目录下;
        /// </summary>
        public static void Save()
        {
            string filePath = GetDefaultFilePath();

            if (!Directory.Exists(Path.GetDirectoryName(filePath)))
                Directory.CreateDirectory(Path.GetDirectoryName(filePath));

            Save(filePath);
        }

        /// <summary>
        /// 从默认的路径获取到自定义Key,并且更新按键映射字典;
        /// </summary>
        public static void Load()
        {
            string filePath = GetDefaultFilePath();
            Load(filePath);
        }

        /// <summary>
        /// 获取到默认存放到的文件路径;
        /// </summary>
        static string GetDefaultFilePath()
        {
            return ResourcePath.CombineConfiguration(CUSTIM_KEY_FILE_NAME);
        }

        /// <summary>
        /// 将按键信息已XML格式保存到此路径下;
        /// </summary>
        public static void Save(string filePath)
        {
            var customKeyList = GetCustomKeyList();
            customKeyListSerializer.Serialize(filePath, customKeyList, FileMode.Create);
        }

        /// <summary>
        /// 获取到需要保存的自定义Key;
        /// </summary>
        static List<CustomKey> GetCustomKeyList()
        {
            List<CustomKey> customKeyList = new List<CustomKey>(keyMap.Count);
            foreach (var item in keyMap)
            {
                CustomKey customKey = new CustomKey(item.Key, item.Value);
                customKeyList.Add(customKey);
            }
            return customKeyList;
        }

        /// <summary>
        /// 获取到自定义Key,并且从更新按键映射字典;
        /// </summary>
        public static void Load(string filePath)
        {
            var customKeys = (List<CustomKey>)customKeyListSerializer.Deserialize(filePath);
            UpdateKeyMap(customKeys);
        }

        /// <summary>
        /// 更新按键映射字典;
        /// </summary>
        public static void UpdateKeyMap(IEnumerable<CustomKey> customKeys)
        {
            keyMap.Clear();
            foreach (var item in customKeys)
            {
                keyMap[item.function] = item.keyCode;
            }
        }

        [XmlType("Key")]
        public struct CustomKey
        {
            public CustomKey(int function, KeyCode keyCode)
            {
                this.function = function;
                this.keyCode = keyCode;
            }

            [XmlAttribute("function")]
            public int function { get; private set; }
            [XmlAttribute("key")]
            public KeyCode keyCode { get; private set; }

            public override string ToString()
            {
                return "Function:" + function + ";Key:" + keyCode.ToString();
            }
        }

        #endregion

        #region 实例部分;

        CustomInput() { }

        static bool initialized = false;

        void Awake()
        {
            if (initialized)
            {
                Destroy(this);
                Debug.LogWarning("重复实例的Input类;" + name);
            }
            else
            {
                initialized = true;
                initialize();
            }
        }

        void initialize()
        {
            try
            {
                Load();
                var emptyKeys = EmptyKeys();
                if (emptyKeys.Count != 0)
                {
                    Debug.LogWarning("未定义的按键:" + emptyKeys.ToEnumerableLog());
                }
                emptyKeys.Clear();
            }
            catch (Exception e)
            {
                Debug.LogWarning("按键初始化失败;" + e);
            }
        }

        #endregion

    }

}
