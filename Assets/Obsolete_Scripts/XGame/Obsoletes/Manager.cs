//using System;
//using System.Collections.Generic;
//using System.Diagnostics;
//using System.Linq;
//using System.Text;
//using UnityEngine;


//namespace XGame
//{

//    /// <summary>
//    /// 不安全的单例模式;每个场景只允许存在一个实例化类;
//    /// </summary>
//    /// <typeparam name="T"></typeparam>
//    [Obsolete]
//    public abstract class Manager<T> : MonoBehaviour
//        where T : Manager<T>
//    {

//        public const string ManagerTag = "Manager";

//        private static T instance = null;

//        protected abstract T This{ get; }

//        public static T GetInstance
//        {
//            get
//            {
//#if UNITY_EDITOR
//                if (instance == null)
//                    UnityEngine.Debug.LogError("该场景未初始化此类!\n" + new StackTrace().ToString());
//#endif
//                return instance;
//            }
//        }

//        /// <summary>
//        /// 获取到该场景的"Manager";
//        /// </summary>
//        /// <returns></returns>
//        public static GameObject GetManager
//        {
//            get { return GameObject.FindWithTag(ManagerTag); }
//        }

//        protected virtual void Awake()
//        {
//            if (instance != null)
//                UnityEngine.Debug.LogWarning("重复实例化的单例,检查此场景是否挂在了多个此脚本!" + this);

//            instance = This;
//        }

//        protected virtual void OnDestroy()
//        {
//            instance = null;
//        }

//    }

//}
