using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Events;

namespace XGame
{


    /// <summary>
    /// 游戏状态控制;事件加入使用Unity.Awake()方法;
    /// </summary>
    [DisallowMultipleComponent]
    public class StateController : Controller<StateController>
    {
        protected StateController() { }

        [SerializeField]
        private StatusType gameState = StatusType.Ready;

        protected override StateController This
        {
            get { return this; }
        }

        /// <summary>
        /// 游戏当前状态;
        /// </summary>
        public StatusType GameState
        {
            get { return gameState; }
            set { gameState = value; }
        }

    }

}
