using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace KouXiaGu.Initialization
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
            SceneManager.LoadScene(gameScene);
        }




    }

}
