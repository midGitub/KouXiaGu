using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace XGame
{


    public static class ControllerHelper
    {

        private static GameObject m_GameController;

        /// <summary>
        /// 获取到GameController物体;
        /// </summary>
        public static GameObject GameController
        {
            get{ return m_GameController == null ?m_GameController = GameObject.FindWithTag("GameController") :m_GameController;}
        }

    }

}
