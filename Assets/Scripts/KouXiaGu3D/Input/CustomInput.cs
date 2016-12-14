using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using System.Xml.Serialization;
using System.IO;

namespace KouXiaGu
{

    /// <summary>
    /// 游戏内定义的
    /// </summary>
    public enum Function
    {

    }

    /// <summary>
    /// 对 UnityEngine.Input 进行包装;
    /// </summary>
    [DisallowMultipleComponent]
    public class CustomInput : MonoBehaviour
    {

        internal static readonly Function[] Functions = Enum.GetValues(typeof(Function)).Cast<Function>().ToArray();

        /// <summary>
        /// 按键映射表;
        /// </summary>
        internal static readonly Dictionary<Function, KeyCode> keyMap = new Dictionary<Function, KeyCode>(Functions.Length);


        #region 功能按键;

        /// <summary>
        /// 用户有按着 相关按键 时一直返回true;
        /// </summary>
        public static bool GetKey(Function function)
        {
            KeyCode keycode = Convert(function);
            return Input.GetKey(keycode);
        }

        /// <summary>
        /// 用户开始按下 相关按键 关键帧时返回true。
        /// </summary>
        public static bool GetKeyDown(Function function)
        {
            KeyCode keycode = Convert(function);
            return Input.GetKeyDown(keycode);
        }

        /// <summary>
        /// 用户释放 相关按键 的关键帧时返回true。
        /// </summary>
        public static bool GetKeyUp(Function function)
        {
            KeyCode keycode = Convert(function);
            return Input.GetKeyUp(keycode);
        }

        /// <summary>
        /// 对按键进行转换,若无法转换则返回异常 KeyNotFoundException;
        /// </summary>
        public static KeyCode Convert(Function function)
        {
            KeyCode keycode = keyMap[function];
            return keycode;
        }

        #endregion


        #region 按键映射储存为XML

        const string CUSTIM_KEY_FILE_NAME = "Input\\Keys.xml";

        static readonly XmlSerializer customKeyListSerializer = new XmlSerializer(typeof(List<CustomKey>));

        /// <summary>
        /// 将按键信息已XML格式,保存到预定义的目录下;
        /// </summary>
        public static void Save()
        {
            string filePath = GetDefaultCustomKeyFilePath();

            if (!Directory.Exists(filePath))
                Directory.CreateDirectory(filePath);

            Save(filePath);
        }

        /// <summary>
        /// 从默认的路径获取到自定义Key,并且从新构建按键映射字典;
        /// </summary>
        public static void Load()
        {
            string filePath = GetDefaultCustomKeyFilePath();
            Load(filePath);
        }

        /// <summary>
        /// 获取到默认存放到的文件路径;
        /// </summary>
        static string GetDefaultCustomKeyFilePath()
        {
            return ResourcePath.CombineConfiguration(CUSTIM_KEY_FILE_NAME);
        }

        /// <summary>
        /// 将按键信息已XML格式保存到此路径下;
        /// </summary>
        public static void Save(string filePath)
        {
            List<CustomKey> customKeyList = GetCustomKeyList();
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
        /// 获取到自定义Key,并且从新构建按键映射字典;
        /// </summary>
        public static void Load(string filePath)
        {
            var customKeys = (List<CustomKey>)customKeyListSerializer.Deserialize(filePath);
            RebuildKeyMap(customKeys);
        }

        /// <summary>
        /// 重设按键映射字典,并且加入新的映射;
        /// </summary>
        static void RebuildKeyMap(IEnumerable<CustomKey> customKeys)
        {
            keyMap.Clear();
            foreach (var item in customKeys)
            {
                keyMap.Add(item.function, item.keyCode);
            }
        }

        [XmlType("Key")]
        struct CustomKey
        {
            public CustomKey(Function function, KeyCode keyCode)
            {
                this.function = function;
                this.keyCode = keyCode;
            }

            [XmlAttribute("Function")]
            public Function function { get; private set; }
            [XmlAttribute("Key")]
            public KeyCode keyCode { get; private set; }
        }

        #endregion


        #region 实例部分;

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
            }
        }

        #endregion

    }

}
