using JiongXiaGu.Unity.Initializers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using System.Threading;
using JiongXiaGu.Unity.Resources;

namespace JiongXiaGu.Unity.RectTerrain
{

    /// <summary>
    /// 地形资源;
    /// </summary>
    public class RectTerrainResource : MonoBehaviour, IModDataInitializeHandle
    {
        Task IModDataInitializeHandle.Initialize(IEnumerable<ModInfo> datanfos, CancellationToken token)
        {
            throw new NotImplementedException();
        }
    }
}
