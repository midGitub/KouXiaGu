using System;
using UnityEngine;
using UnityEngine.UI;
using XGame.Running;
using XGame.Test;

namespace XGame
{


    public class Test_Time : TestUIBase
    {

        private GameTime gameTime;

        private GameTimer gameTimer;

        [SerializeField]
        private bool IsdebugTimer;

        protected override void Awake()
        {
            base.Awake();
            gameTime = ControllerHelper.GameController.GetComponentInChildren<GameTime>();
            gameTimer = ControllerHelper.GameController.GetComponentInChildren<GameTimer>();

            if (IsdebugTimer)
            {
                gameTimer.OnEvevyMonthAction += OnMonth;
                gameTimer.OnEvevyYearAction += OnYear;
            }
        }

        protected override string Log()
        {
            string str;
            if (StateController.GetInstance.GameState == StatusType.GameRunning)
            {
                str = "当前游戏时间 : " + gameTime.NowTime.ToString();
            }
            else
            {
                str = "当前游戏时间 : Null";
            }
            return str;
        }

        private void OnMonth(DateTime dateTime)
        {
            Debug.Log("现在是" + dateTime.Month + "月");
        }

        private void OnYear(DateTime dateTime)
        {
            Debug.Log("现在是" + dateTime.Year + "年");
        }

    }

}
