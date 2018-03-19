using JiongXiaGu.Unity.RunTime;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace JiongXiaGu.Unity.Initializers
{

    public interface IGameSceneStartUpdate
    {
    }

    [DisallowMultipleComponent]
    public sealed class GameSceneController : MonoBehaviour, ISceneController
    {
        private GameSceneController()
        {
        }

        private void Start()
        {
         
        }

        Task ISceneController.QuitScene()
        {
            return Task.CompletedTask;
        }
    }
}
