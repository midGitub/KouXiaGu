//using System;
//using System.Collections;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using Assets.Scripts.Game.Start;
//using UnityEngine;
//using UnityEngine.SceneManagement;

//namespace XGame
//{

//    /// <summary>
//    /// 游戏状态和变更游戏状态的控制器;负责游戏场景切换;
//    /// </summary>
//    [DisallowMultipleComponent]
//    public class SceneController : MonoBehaviour, ISceneStart
//    {

//        #region Unity;

//        private void Awake()
//        {
//            WaitScene = false;
//        }

//        #endregion

//        /// <summary>
//        /// 预先读取的游戏场景;
//        /// </summary>
//        private AsyncOperation m_gameSceneAsync;

//        /// <summary>
//        /// 预先读取的开始场景;
//        /// </summary>
//        private AsyncOperation m_startSceneAsync;

//        /// <summary>
//        /// 等待读取场景,若为true,则不允许跳转场景,为false,则允许跳转场景;
//        /// </summary>
//        public static bool WaitScene { get; set; }

//        #region 场景切换;

//        /// <summary>
//        /// 异步加载跳转到游戏场景;
//        /// </summary>
//        /// <returns>若无法跳转则返回false,准备跳转或者正在跳转返回true;</returns>
//        [ContextMenu("异步加载到游戏")]
//        public bool ToGameScene()
//        {
//            //若正在保存游戏,则返回等待;
//            if (WaitScene)
//                return false;

//            if (m_gameSceneAsync == null)
//            {
//                LoadGameSceneAsync(true);
//            }
//            else
//            {
//                m_gameSceneAsync.allowSceneActivation = true;
//            }
//            return true;
//        }

//#if UNITY_EDITOR
//        [ContextMenu("预先读取游戏场景")]
//        private void Test_LoadGameSceneAsync()
//        {
//            LoadGameSceneAsync();
//        }
//#endif

//        /// <summary>
//        /// 预先读取游戏场景;
//        /// </summary>
//        public void LoadGameSceneAsync(bool allowSceneActivation = false)
//        {
//            if (m_gameSceneAsync == null)
//            {
//                m_gameSceneAsync = SceneManager.LoadSceneAsync(1);
//                m_gameSceneAsync.allowSceneActivation = allowSceneActivation;
//            }
//        }

//        /// <summary>
//        /// 退出到开始界面;
//        /// </summary>
//        /// <returns>若无法跳转则返回false,准备跳转或者正在跳转返回true;</returns>
//        [ContextMenu("异步加载到开始界面")]
//        public bool ToStartScene()
//        {
//            //若正在保存游戏,则返回等待;
//            if (WaitScene)
//                return false;

//            if (m_startSceneAsync == null)
//            {
//                LoadStartSceneAsync(true);
//            }
//            else
//            {
//                m_startSceneAsync.allowSceneActivation = true;
//            }
//            return true;
//        }

//        /// <summary>
//        /// 预先读取开始场景;
//        /// </summary>
//        //[ContextMenu("预先读取开始场景")]
//        public void LoadStartSceneAsync(bool allowSceneActivation = false)
//        {
//            if (m_startSceneAsync == null)
//            {
//                m_startSceneAsync = SceneManager.LoadSceneAsync(0);
//                m_startSceneAsync.allowSceneActivation = allowSceneActivation;
//            }
//        }

//        #endregion


//        #region IScene

//        ///// <summary>
//        ///// 当跳转到游戏场景调用;
//        ///// </summary>
//        //void IStartGame.GameScene()
//        //{
//        //    m_gameSceneAsync = null;
//        //}

//        /// <summary>
//        /// 当跳转到开始场景调用;
//        /// </summary>
//        void ISceneStart.StartScene()
//        {
//            m_startSceneAsync = null;
//        }

//        #endregion

//    }

//}
