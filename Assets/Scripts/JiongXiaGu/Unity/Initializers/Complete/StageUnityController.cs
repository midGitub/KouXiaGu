using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using System.Threading.Tasks;
using UnityEngine.SceneManagement;

namespace JiongXiaGu.Unity.Initializers
{

    [DisallowMultipleComponent]
    internal sealed class StageUnityController : MonoBehaviour
    {
        private static readonly GlobalSingleton<StageUnityController> singleton = new GlobalSingleton<StageUnityController>();
        public static StageUnityController Instance => singleton.GetInstance();

        [SerializeField]
        private UnityEngine.Object InitializationScene;
        [SerializeField]
        private UnityEngine.Object MainMenuScene;
        [SerializeField]
        private UnityEngine.Object GameScene;

        private void Awake()
        {
            singleton.SetInstance(this);
        }

        private Task Start()
        {
            return Stage.OnProgramStart();
        }

        public void LoadInitializationScene()
        {
            SceneManager.LoadScene(InitializationScene.name, LoadSceneMode.Single);
        }

        public void LoadMainMenuScene()
        {
            SceneManager.LoadScene(MainMenuScene.name, LoadSceneMode.Single);
        }

        public void LoadGameScene()
        {
            SceneManager.LoadScene(GameScene.name, LoadSceneMode.Single);
        }
    }
}
