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
        private string WelcomeScene;
        [SerializeField]
        private string MainMenuScene;
        [SerializeField]
        private string GameScene;

        public string WelcomeSceneName => WelcomeScene;
        public string MainMenuSceneName => MainMenuScene;
        public string GameSceneName => GameScene;

        private void Awake()
        {
            singleton.SetInstance(this);
        }
    }
}
