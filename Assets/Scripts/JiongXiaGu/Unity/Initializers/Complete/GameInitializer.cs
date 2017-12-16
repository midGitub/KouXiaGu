﻿//using JiongXiaGu.Unity.Resources;
//using System;
//using UnityEngine;

//namespace JiongXiaGu.Unity.Initializers
//{

//    /// <summary>
//    /// 游戏程序初始化器;
//    /// </summary>
//    public class GameInitializer : MonoBehaviour
//    {
//        private const string InitializerName = "程序初始化";
//        public static bool IsCompleted { get; private set; }
//        public static bool IsFaulted { get; private set; }
//        public static Exception Exception { get; private set; }

//        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
//        private static void Initialize()
//        {
//            try
//            {
//                Resource.Initialize();
//                LoadableResource.Initialize();
//                OnCompleted();
//            }
//            catch (Exception ex)
//            {
//                OnFaulted(ex);
//            }
//        }

//        private void OnDestroy()
//        {
//            LoadableResource.Quit();
//        }

//        private static void OnCompleted()
//        {
//            IsCompleted = true;
//            IsFaulted = false;
//            Exception = null;

//            UnityDebugHelper.SuccessfulReport(InitializerName);
//        }

//        private static void OnFaulted(Exception ex)
//        {
//            IsCompleted = true;
//            IsFaulted = true;
//            Exception = ex;

//            UnityDebugHelper.FailureReport(InitializerName, ex);
//        }
//    }
//}
