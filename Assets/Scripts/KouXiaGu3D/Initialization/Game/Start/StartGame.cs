using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace KouXiaGu.Initialization
{

    public class StartGame : MonoBehaviour
    {

        [SerializeField]
        string gameSceneName = "Game";

        public void Game()
        {
            SceneManager.LoadScene(gameSceneName);
        }

    }

}
