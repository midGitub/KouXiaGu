using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace KouXiaGu
{

    /// <summary>
    /// 对Unity.Input进行包装,加入优先级 与 功能按键;
    /// 按键根据Key枚举内的数据进行注册;
    /// </summary>
    [DisallowMultipleComponent, Obsolete]
    public class Input : MonoBehaviour
    {

        #region 静态初始化

        /// <summary>
        /// 按键对应按键响应;在脚本内初始化;
        /// </summary>
        private static readonly Dictionary<FunctionKey, KeyOverlie> _KeySet = InitializeKeyDictionary();

        /// <summary>
        /// 默认按键合集;
        /// </summary>
        private static readonly Dictionary<FunctionKey, FieldInfo> DefaultKeySet;

        /// <summary>
        /// 使用按键合集;
        /// </summary>
        private static readonly Dictionary<FunctionKey, FieldInfo> KeySet;

        static Input()
        {
            Enable = true;
            InitializeKeySet(out DefaultKeySet, out KeySet);
        }

        /// <summary>
        /// 初始化Key映射表;
        /// </summary>
        private static Dictionary<FunctionKey, KeyOverlie> InitializeKeyDictionary()
        {
            Dictionary<FunctionKey, KeyOverlie> keyDictionary = new Dictionary<FunctionKey, KeyOverlie>();
            Array keyInfos = Enum.GetValues(typeof(FunctionKey));

            foreach (FunctionKey key in keyInfos)
            {
                keyDictionary.Add(key, new KeyOverlie());
            }
            return keyDictionary;
        }

        /// <summary>
        /// 获取默认按键值合集;
        /// </summary>
        private static void InitializeKeySet(
            out Dictionary<FunctionKey, FieldInfo> defaultKeySet,
            out Dictionary<FunctionKey, FieldInfo> keySet)
        {
            defaultKeySet = new Dictionary<FunctionKey, FieldInfo>();
            keySet = new Dictionary<FunctionKey, FieldInfo>();
            Type defaultKeyType = typeof(Input);
            FieldInfo[] fieldInfos = defaultKeyType.GetFields(BindingFlags.Instance | BindingFlags.NonPublic);

            foreach (var fieldInfo in fieldInfos)
            {
                KeyAttribute keyAttribute =
                    (KeyAttribute)Attribute.GetCustomAttribute(fieldInfo, typeof(KeyAttribute));
                if (keyAttribute != null)
                {
                    if (keyAttribute.IsDefaultKey)
                    {
                        defaultKeySet.Add(keyAttribute.FunctionKey, fieldInfo);
                    }
                    else
                    {
                        keySet.Add(keyAttribute.FunctionKey, fieldInfo);
                    }
                }
            }
        }

        #endregion


        #region 按键保存,初始,设置(可在Unity设置初值);

        [Header("默认的按键")]

        [SerializeField, Key(FunctionKey.Mouse_Cancel, true)]
        private KeyCode d_Mouse_Cancel;

        [SerializeField, Key(FunctionKey.Mouse_Confirm, true)]
        private KeyCode d_Mouse_Confirm;

        [SerializeField, Key(FunctionKey.CameraRotateLeft, true)]
        private KeyCode d_CameraRotateLeft;

        [SerializeField, Key(FunctionKey.CameraRotateRigth, true)]
        private KeyCode d_CameraRotateRigth;

        [SerializeField, Key(FunctionKey.CameraDrag, true)]
        private KeyCode d_CameraDrag;

        [SerializeField, Key(FunctionKey.ObjectRotateLeft, true)]
        private KeyCode d_ObjectRotateLeft;

        [SerializeField, Key(FunctionKey.ObjectRotateRight, true)]
        private KeyCode d_ObjectRotateRight;

        [SerializeField, Key(FunctionKey.Esc, true)]
        private KeyCode d_Esc;

        [SerializeField, Key(FunctionKey.Ent, true)]
        private KeyCode d_Ent;


        [Header("当前使用的按键(修改无效)")]

        [SerializeField, Key(FunctionKey.Mouse_Cancel)]
        private KeyCode Mouse_Cancel;

        [SerializeField, Key(FunctionKey.Mouse_Confirm)]
        private KeyCode Mouse_Confirm;

        [SerializeField, Key(FunctionKey.CameraRotateLeft)]
        private KeyCode CameraRotateLeft;

        [SerializeField, Key(FunctionKey.CameraRotateRigth)]
        private KeyCode CameraRotateRigth;

        [SerializeField, Key(FunctionKey.CameraDrag)]
        private KeyCode CameraDrag;

        [SerializeField, Key(FunctionKey.ObjectRotateLeft)]
        private KeyCode ObjectRotateLeft;

        [SerializeField, Key(FunctionKey.ObjectRotateRight)]
        private KeyCode ObjectRotateRight;

        [SerializeField, Key(FunctionKey.Esc)]
        private KeyCode Esc;

        [SerializeField, Key(FunctionKey.Ent)]
        private KeyCode Ent;

        /// <summary>
        /// 初始化Key初值;在PlayerPrefs内获取;
        /// </summary>
        private void InitializeKey()
        {
            Array keyInfos = Enum.GetValues(typeof(FunctionKey));

            foreach (FunctionKey functionKey in keyInfos)
            {
                KeyCode unityKeyCode = LoadOnPlayerPrefs(functionKey);
                _KeySet[functionKey].UnityKeyCode = unityKeyCode;
            }
        }

        /// <summary>
        /// 暂时恢复所有功能键到默认值,并且重新赋值到PlayerPrefs;
        /// </summary>
        [ContextMenu("恢复所有按键到默认")]
        public void RecoveryAllKey()
        {
            IEnumerable<KeyValuePair<FunctionKey, FieldInfo>> defaultKeyEnumerable = DefaultKeySet;
            foreach (var item in defaultKeyEnumerable)
            {
                FunctionKey functionKey = item.Key;
                KeyCode unityKeyCode = (KeyCode)item.Value.GetValue(this);
                _KeySet[functionKey].UnityKeyCode = unityKeyCode;
                SaveOnPlayerPrefs(functionKey, unityKeyCode);
            }

#if UNITY_EDITOR
            ResetUseKey();
#endif
        }

#if UNITY_EDITOR

        [ContextMenu("刷新使用的按键")]
        private void ResetUseKey()
        {
            foreach (var item in KeySet)
            {
                KeyCode unityCode = LoadOnPlayerPrefs(item.Key);
                item.Value.SetValue(this, unityCode);
            }
        }

        [ContextMenu("恢复所有默认按键")]
        private void ResetDefaultKey()
        {
            foreach (var item in DefaultKeySet)
            {
                KeyCode unityCode = LoadOnPlayerPrefs(item.Key);
                item.Value.SetValue(this, unityCode);
            }
        }

#endif


        /// <summary>
        /// 是否已经向注册表内注册了默认值;返回false,为未注册;
        /// </summary>
        public bool IsRegister
        {
            get { return ContainsOnPlayerPrefs(FunctionKey.Unknown); }
        }

        /// <summary>
        /// 作为存储的PlayerPrefs的前缀;
        /// </summary>
        private const string PlayerPrefsPrefix = "Input_";

        /// <summary>
        /// 返回功能键保存到注册表的名称;
        /// </summary>
        private static string GetPlayerPrefsName(FunctionKey key)
        {
            return PlayerPrefsPrefix + key.ToString();
        }

        /// <summary>
        /// 从注册表获取此键值;
        /// </summary>
        private static KeyCode LoadOnPlayerPrefs(FunctionKey key)
        {
            int keyCode = PlayerPrefs.GetInt(GetPlayerPrefsName(key));
            return (KeyCode)keyCode;
        }

        /// <summary>
        /// 保存此键值到注册表;
        /// </summary>
        private static void SaveOnPlayerPrefs(FunctionKey key, KeyCode unityKeyCode)
        {
            PlayerPrefs.SetInt(GetPlayerPrefsName(key), (int)unityKeyCode);
        }

        /// <summary>
        /// 确认是否存在此键值;
        /// </summary>
        private static bool ContainsOnPlayerPrefs(FunctionKey key)
        {
            return PlayerPrefs.HasKey(GetPlayerPrefsName(key));
        }

        /// <summary>
        /// 根据按键映射表获取到生效的KeyCode;
        /// </summary>
        public static KeyCode GetUnityKey(FunctionKey key)
        {
            KeyOverlie keyOverlie = GetKeyOverlie(key);
            return keyOverlie.UnityKeyCode;
        }

        /// <summary>
        /// 设置到 按键映射表 和 注册表;
        /// </summary>
        public void SetUnityKey(FunctionKey key, KeyCode unityKeyCode)
        {
            KeyOverlie keyOverlie = GetKeyOverlie(key);
            keyOverlie.UnityKeyCode = unityKeyCode;
            SaveOnPlayerPrefs(key, unityKeyCode);

#if UNITY_EDITOR
            ResetUseKey();
#endif
        }

        #endregion


        #region Unity

        /// <summary>
        /// 在初始化时,初始化
        /// </summary>
        private void Awake()
        {
            if (IsRegister)
            {
                InitializeKey();
            }
            else
            {
                RecoveryAllKey();
            }
        }

        #endregion


        #region 按键注册;

        private static KeyOverlie GetKeyOverlie(FunctionKey key)
        {
#if UNITY_EDITOR
            if (!_KeySet.ContainsKey(key))
            {
                Debug.LogError(key + "这个按键没有注册!");
            }
#endif
            KeyOverlie keyOverlie = _KeySet[key];
            return keyOverlie;
        }

        /// <summary>
        /// 向类注册此按键;
        /// </summary>
        public static void LogIn(FunctionKey key, int hashCode)
        {
            KeyOverlie keyOverlie = GetKeyOverlie(key);
            keyOverlie.LogIn(hashCode);
        }

        /// <summary>
        /// 向类注销此按键;
        /// </summary>
        public static bool LogOut(FunctionKey key, int hashCode)
        {
            KeyOverlie keyOverlie = GetKeyOverlie(key);
            return keyOverlie.LogOut(hashCode);
        }

        /// <summary>
        /// 向类注册这些按键;
        /// </summary>
        /// <param name="keys">多个按键</param>
        /// <param name="onlyID">注册的ID值;</param>
        public static void LogInMore(FunctionKey keys, int onlyID)
        {
            foreach (FunctionKey item in Enum.GetValues(typeof(FunctionKey)))
            {
                if ((keys & item) > 0)
                {
                    LogIn(item, onlyID);
                }
            }
        }

        /// <summary>
        /// 向类注销这些按键;
        /// </summary>
        /// <param name="keys">多个按键</param>
        /// <param name="onlyID">注册的ID值;</param>
        public static void LogOutMore(FunctionKey keys, int onlyID)
        {
            foreach (FunctionKey item in Enum.GetValues(typeof(FunctionKey)))
            {
                if ((keys & item) > 0)
                {
                    LogOut(item, onlyID);
                }
            }
        }


        #endregion


        #region 按键获取;

        /// <summary>
        /// 是否启用,初始为True;若为false,则所有查询都返回false;
        /// </summary>
        public static bool Enable { get; set; }

        /// <summary>
        /// hashCode值是否有效;
        /// </summary>
        private static bool IsEffective(FunctionKey key, int hashCode, out KeyCode unityKeyCode)
        {
            KeyOverlie keyOverlie = GetKeyOverlie(key);
            unityKeyCode = keyOverlie.UnityKeyCode;
            return keyOverlie.IsEffective(hashCode);
        }

        /// <summary>
        /// 当按键按下;
        /// </summary>
        public static bool GetKeyDown(FunctionKey key, int hashCode)
        {
            KeyCode unityKeyCode;
            bool isEffective = IsEffective(key, hashCode, out unityKeyCode);

            if (isEffective && Enable)
            {
                return UnityEngine.Input.GetKeyDown(unityKeyCode);
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 当这些按键其中一个按下,则返回true,否则返回false;
        /// </summary>
        /// <param name="keys">要检查的按键;</param>
        /// <param name="hashCode">唯一ID;</param>
        /// <returns></returns>
        public static bool GetKeysDown(FunctionKey keys, int onlyID)
        {
            foreach (FunctionKey item in Enum.GetValues(typeof(FunctionKey)))
            {
                if ((keys & item) > 0 && GetKeyDown(item, onlyID))
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 当按键按下,不存在优先级;
        /// </summary>
        public static bool GetKeyDown(FunctionKey key)
        {
            if (Enable)
            {
                KeyCode unityKeyCode = GetUnityKey(key);
                return UnityEngine.Input.GetKeyDown(unityKeyCode);
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 当按键抬起;
        /// </summary>
        public static bool GetKeyUp(FunctionKey key, int hashCode)
        {
            KeyCode unityKeyCode;
            bool isEffective = IsEffective(key, hashCode, out unityKeyCode);

            if (isEffective && Enable)
            {
                return UnityEngine.Input.GetKeyUp(unityKeyCode);
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 当这些按键其中一个抬起,则返回true,否则返回false;
        /// </summary>
        /// <param name="keys">要检查的按键;</param>
        /// <param name="hashCode">唯一ID;</param>
        /// <returns></returns>
        public static bool GetKeysUp(FunctionKey keys, int onlyID)
        {
            foreach (FunctionKey item in Enum.GetValues(typeof(FunctionKey)))
            {
                if ((keys & item) > 0 && GetKeyUp(item, onlyID))
                {
                        return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 当按键按下,不存在优先级;
        /// </summary>
        public static bool GetKeyUp(FunctionKey key)
        {
            if (Enable)
            {
                KeyCode unityKeyCode = GetUnityKey(key);
                return UnityEngine.Input.GetKeyUp(unityKeyCode);
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 当按键按着;
        /// </summary>
        public static bool GetKey(FunctionKey key, int hashCode)
        {
            KeyCode unityKeyCode;
            bool isEffective = IsEffective(key, hashCode, out unityKeyCode);

            if (isEffective && Enable)
            {
                return UnityEngine.Input.GetKey(unityKeyCode);
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 当这些按键其中一个按着,则返回true,否则返回false;
        /// </summary>
        /// <param name="keys">要检查的按键;</param>
        /// <param name="hashCode">唯一ID;</param>
        /// <returns></returns>
        public static bool GetKeys(FunctionKey keys, int onlyID)
        {
            foreach (FunctionKey item in Enum.GetValues(typeof(FunctionKey)))
            {
                if ((keys & item) > 0 && GetKey(item, onlyID))
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 当按键按下,不存在优先级;
        /// </summary>
        public static bool GetKey(FunctionKey key)
        {
            if (Enable)
            {
                KeyCode unityKeyCode = GetUnityKey(key);
                return UnityEngine.Input.GetKey(unityKeyCode);
            }
            else
            {
                return false;
            }
        }

        #endregion


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


        #region 唯一值获取(静态);

        private static int m_id = int.MinValue;

        /// <summary>
        /// 获取到一个独一无二的值,用于注册之类的;
        /// </summary>
        /// <returns></returns>
        public static int GetOnlyId()
        {
            return ++m_id;
        }

        #endregion


        /// <summary>
        /// 后加入先响应;不带线程锁;
        /// </summary>
        private class KeyOverlie
        {

            public KeyOverlie()
            {
                list = new LinkedList<int>();
                UnityKeyCode = KeyCode.None;
            }

            public KeyOverlie(KeyCode unityKeyCode)
            {
                list = new LinkedList<int>();
                UnityKeyCode = unityKeyCode;
            }


            public KeyCode UnityKeyCode{ get; set; }

            private LinkedList<int> list;

            public int lastLogIn
            {
                get
                {
                    try
                    {
                        return list.Last.Value;
                    }
                    catch (NullReferenceException)
                    {
                        return default(int);
                    }
                }
            }

            /// <summary>
            /// 向其注册这个值若存在相同值,则返回false;O(n)
            /// </summary>
            public void LogIn(int hashCode)
            {
                if (list.Contains(hashCode))
                {
                    throw new ArgumentException("UnityKeyCode" + UnityKeyCode.ToString());
                }
                else
                {
                    list.AddLast(hashCode);
                }
            }

            /// <summary>
            /// 向其注销这个值;O(n)
            /// </summary>
            public bool LogOut(int hashCode)
            {
                return list.Remove(hashCode);
            }

            /// <summary>
            /// 确认这个值是否为现在的有效值;有效返回true,否则返回false;
            /// </summary>
            public bool IsEffective(int hashCode)
            {
                return hashCode == lastLogIn;
            }

        }

    }


    public class KeyAttribute : Attribute
    {

        public KeyAttribute(FunctionKey functionKey, bool isDefaultKey = false)
        {
            this.FunctionKey = functionKey;
            this.IsDefaultKey = isDefaultKey;
        }

        public bool IsDefaultKey { get; private set; }

        public FunctionKey FunctionKey { get; private set; }
    }


}
