using JiongXiaGu.Collections;
using JiongXiaGu.Unity.Initializers;
using JiongXiaGu.Unity.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace JiongXiaGu.Unity.RectTerrain
{

    [DisallowMultipleComponent]
    public class RectTerrainResource : MonoBehaviour, IDataInitializeHandle
    {
        public LandformDescriptionDictionary LandformDescrs { get; private set; }
        public LandformResCreater LandformRes { get; private set; }

        private void Awake()
        {
            LandformDescrs = new LandformDescriptionDictionary();
            LandformRes = new LandformResCreater(LandformDescrs);
        }

        void IDataInitializeHandle.Read(LoadableContent content, ITypeDictionary info, CancellationToken token)
        {
            throw new NotImplementedException();
        }

        void IDataInitializeHandle.Prepare(IEnumerable<ITypeDictionary> infos, CancellationToken token)
        {
            throw new NotImplementedException();
        }
    }
}
