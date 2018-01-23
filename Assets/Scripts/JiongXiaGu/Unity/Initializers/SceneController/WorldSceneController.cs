using JiongXiaGu.Unity;
using JiongXiaGu.Unity.Initializers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using System.Threading.Tasks;

namespace JiongXiaGu.Unity
{


    public class WorldSceneController : MonoBehaviour, ISceneController
    {
        Task ISceneController.QuitScene()
        {
            return Task.CompletedTask;
        }
    }
}
