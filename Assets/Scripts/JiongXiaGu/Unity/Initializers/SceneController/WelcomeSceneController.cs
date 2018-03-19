using JiongXiaGu.Unity.RunTime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace JiongXiaGu.Unity.Initializers
{

    /// <summary>
    /// 欢迎界面
    /// </summary>
    public class WelcomeSceneController : MonoBehaviour
    {
        private void Start()
        {
            Stage.LoadMainMenuScene();
        }
    }
}
