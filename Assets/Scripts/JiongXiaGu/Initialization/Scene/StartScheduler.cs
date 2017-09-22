using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace JiongXiaGu.Initialization
{

    [DisallowMultipleComponent]
    public class StartScheduler : MonoBehaviour
    {
        StartScheduler() { }


        [SerializeField]
        string gameScene = "Game";

        /// <summary>
        /// 开始游戏;
        /// </summary>
        public void StartGame()
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(gameScene);
        }




    }

}
