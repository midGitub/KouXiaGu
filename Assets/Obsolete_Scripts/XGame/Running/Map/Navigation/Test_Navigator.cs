//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using UnityEngine;
//using KouXiaGu.Test;

//namespace KouXiaGu.Running.Map.Guidances
//{


//    public class Test_Navigator : TestUIBase, IKey
//    {

//        private Navigator m_Navigator;


//        protected override void Awake()
//        {
//            base.Awake();
//            m_Navigator = ControllerHelper.GameController.GetComponentInChildren<Navigator>();
//        }

//        protected override string Log()
//        {
//            string strLog = "导航还初始化;";
//            if (StateController.GetInstance.GameState == StatusType.GameRunning)
//            {
//                IntVector2 position = (IntVector2)Input.MouseWorldPosition;

//                strLog = "可行走的方向 :" + m_Navigator.GetOpenAspect(position, this);
//            }
//            return strLog;
//        }

//    }

//}
