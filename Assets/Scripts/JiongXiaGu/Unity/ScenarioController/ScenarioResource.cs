using JiongXiaGu.Unity.Initializers;
using JiongXiaGu.Unity.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace JiongXiaGu.Unity.ScenarioController
{

    [DisallowMultipleComponent]
    internal class ScenarioResource : MonoBehaviour, IModificationInitializeHandle
    {
        private ScenarioResource()
        {
        }

        void IModificationInitializeHandle.Initialize(IReadOnlyList<Modification> mods, CancellationToken token)
        {
            
        }
    }
}
