using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace KouXiaGu
{

    /// <summary>
    /// 提供按键转换功能;
    /// </summary>
    public sealed class FunctionInput : MonoBehaviour
    {
        static FunctionInput()
        {
            DefaultKeyDictionary = AttributeHelper.GetFieldInfosFormField<int, FunctionKeyAttribute>(
                typeof(FunctionInput), BindingFlags.NonPublic | BindingFlags.Instance, attribute => (int)attribute.FunctionKey);
        }

        private FunctionInput() { }

        #region 设置默认按键(添加按键仅增加这里);

        [FunctionKey(FunctionKey.Unknown)]
        private KeyCode Unknown;

        [Header("默认的按键")]
        [SerializeField, FunctionKey(FunctionKey.Mouse_Confirm)]
        private KeyCode Mouse_Confirm;

        [SerializeField, FunctionKey(FunctionKey.Mouse_Cancel)]
        private KeyCode Mouse_Cancel;

        #endregion

        /// <summary>
        /// 每个功能键存储时的前缀;
        /// </summary>
        private const string SavePrefix = "Input_";

        private static FunctionInput instance;

        private static readonly Dictionary<int, FieldInfo> DefaultKeyDictionary;

        /// <summary>
        /// 当前游戏使用的按键;
        /// </summary>
        private static Dictionary<int,KeyCode> KeyDictionary;

        /// <summary>
        /// 是否已经存在按键信息文件;
        /// </summary>
        private bool isSave { get { return PlayerPrefs.HasKey(GetSaveName(FunctionKey.Unknown)); } }
        public FunctionInput GetInstance { get { return instance; } }

        private void Awake()
        {
            SetInstance();
            InitialiseKey();
        }

        private void SetInstance()
        {
            if (instance != null)
                Debug.LogError("重复实例的按键组件!" + name);
            instance = this;
        }

        private void InitialiseKey()
        {
            KeyDictionary = new Dictionary<int, KeyCode>();
            if (isSave)
            {
                RecoveryAll();
            }
            else
            {
                RecoveryAllFromDefaltKey();
                SaveAll();
            }
        }

        /// <summary>
        /// 所有功能按键保存到文件;
        /// </summary>
        [ContextMenu("保存所有到文件")]
        public void SaveAll()
        {
            foreach (FunctionKey item in Enum.GetValues(typeof(FunctionKey)))
            {
                Save(item);
            }
        }

        /// <summary>
        /// 所有功能按键从文件恢复到按键设置;
        /// </summary>
        [ContextMenu("从按键文件恢复")]
        public void RecoveryAll()
        {
            foreach (FunctionKey item in Enum.GetValues(typeof(FunctionKey)))
            {
                Recovery(item);
            }
        }

        /// <summary>
        /// 所有功能按键恢复到默认的按键;
        /// </summary>
        [ContextMenu("从默认按键恢复")]
        public void RecoveryAllFromDefaltKey()
        {
            foreach (FunctionKey item in Enum.GetValues(typeof(FunctionKey)))
            {
                RecoveryFromDefaltKey(item);
            }
        }

        /// <summary>
        /// 将按键保存到文件;
        /// </summary>
        /// <param name="key"></param>
        public static void Save(FunctionKey key)
        {
            string saveName = GetSaveName(key);
            KeyCode keyCode = KeyDictionary[(int)key];
            PlayerPrefs.SetInt(saveName, (int)keyCode);
        }

        /// <summary>
        /// 从文件恢复到按键设置;
        /// </summary>
        /// <param name="key"></param>
        public static void Recovery(FunctionKey key)
        {
            string saveName = GetSaveName(key);
            KeyCode keyCode = (KeyCode)PlayerPrefs.GetInt(saveName);
            KeyDictionary[(int)key] = keyCode;
        }

        public static void RecoveryFromDefaltKey(FunctionKey key)
        {
            KeyCode keyCode = (KeyCode)DefaultKeyDictionary[(int)key].GetValue(instance);
            KeyDictionary[(int)key] = keyCode;
        }

        /// <summary>
        /// 返回功能键存储时的名称;
        /// </summary>
        private static string GetSaveName(FunctionKey key)
        {
            return SavePrefix + key.ToString();
        }

        /// <summary>
        /// 转换成UnityKeyCode;
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static KeyCode GetKey(FunctionKey key)
        {
            return KeyDictionary[(int)key];
        }

        /// <summary>
        /// 设置这个功能键的unityKey;
        /// </summary>
        /// <param name="key"></param>
        /// <param name="unityKey"></param>
        public static void SetKey(FunctionKey key, KeyCode unityKey)
        {
            KeyDictionary[(int)key] = unityKey;
            Save(key);
        }


        #region 鼠标位置;

        /// <summary>
        /// 鼠标在屏幕坐标中的位置;
        /// </summary>
        public static Vector3 MouseScreenPosition
        {
            get { return UnityEngine.Input.mousePosition; }
        }

        /// <summary>
        /// 鼠标位于世界坐标中的位置;返回的.z 为摄像机的位置z;
        /// </summary>
        public static Vector3 MouseWorldPosition
        {
            get { return Camera.main.ScreenToWorldPoint(UnityEngine.Input.mousePosition); }
        }

        #endregion

        public override string ToString()
        {
            string str = base.ToString() + "\n正在使用的按键:\n";
            foreach (var item in KeyDictionary)
            {
                str += string.Concat("FunctionKey:", (FunctionKey)item.Key, "  UnityKeyCode:", item.Value, "\n");
            }
            return str;
        }

#if UNITY_EDITOR

        [ContextMenu("使用的按键")]
        private void Test_ToString()
        {
            Debug.Log(this.ToString());
        }

        [ContextMenu("输出保存文件")]
        private void Test_SaveFile()
        {
            string str = "保存的按键:\n";
            foreach (FunctionKey item in Enum.GetValues(typeof(FunctionKey)))
            {
                string saveName = GetSaveName(item);
                int key = PlayerPrefs.GetInt(saveName);
                str += "FunctionKey:" + item.ToString() + "  UnityKeyCode:" + (KeyCode)key + "\n";
            }
            Debug.Log(str);
        }

#endif

        internal class FunctionKeyAttribute : Attribute
        {

            public FunctionKeyAttribute(FunctionKey functionKey)
            {
                this.FunctionKey = functionKey;
            }

            public FunctionKey FunctionKey { get; private set; }
        }

    }

}
