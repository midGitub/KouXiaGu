//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using UnityEngine;

//namespace XGame
//{

//    [DisallowMultipleComponent]
//    public class GameController : Controller<GameController>
//    {

//        /// <summary>
//        /// 游戏状态;
//        /// </summary>
//        [SerializeField]
//        private GameState state;

//        [Obsolete]
//        public static readonly WaitForFixedUpdate WaitForFixedUpdate = new WaitForFixedUpdate();

//        /// <summary>
//        /// 游戏现在的状态;
//        /// </summary>
//        public static GameState State
//        {
//            get { return GetInstance != null ? GetInstance.state : GameState.Quit; }
//            set { GetInstance.state = value; }
//        }

//        protected override GameController This
//        {
//            get{ return this; }
//        }

//        protected override void Awake()
//        {
//            base.Awake();
//            DontDestroyOnLoad(gameObject);
//        }

//        #region 游戏状态;

//        /// <summary>
//        /// 设置游戏阶段为初始化;
//        /// </summary>
//        [Obsolete]
//        internal static void SetGameState_Initialize()
//        {
//            State = GameState.Initialize;
//        }

//        /// <summary>
//        /// 设置游戏阶段为游戏阶段;并设置为已经初始化完毕;
//        /// </summary>
//        [Obsolete]
//        internal static void SetGameState_Game()
//        {
//            State = GameState.Game;
//        }

//        /// <summary>
//        /// 设置游戏阶段为开始界面;
//        /// </summary>
//        [Obsolete]
//        internal static void SetGameState_Start()
//        {
//            State = GameState.Start;
//        }

//        #endregion

//    }

//}
