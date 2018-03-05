using JiongXiaGu.Unity;
using JiongXiaGu.Unity.Initializers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using System.Threading.Tasks;
using JiongXiaGu.Unity.Scenarios;
using JiongXiaGu.Unity.Archives;

namespace JiongXiaGu.Unity
{

    public interface IWorldSceneInitializeHandle
    {
        void Initialize(Scenario scenario, Archive archive);
    }

    public class WorldSceneController : MonoBehaviour, ISceneController
    {
        Task ISceneController.QuitScene()
        {
            return Task.CompletedTask;
        }
    }
}
