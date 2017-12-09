using JiongXiaGu.Unity.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace JiongXiaGu.Unity.RectTerrain
{

    [DisallowMultipleComponent]
    public class RectTerrainResource : MonoBehaviour, IContentLoadHandler
    {
        public LandformDescrDictionary LandformDescrs { get; private set; }
        public LandformResPool LandformRes { get; private set; }

        private void Awake()
        {
            LandformDescrs = new LandformDescrDictionary();
            LandformRes = new LandformResPool(LandformDescrs);
        }

        void IContentLoadHandler.Add(LoadableContent content)
        {
            LandformDescrs.Add(content);
        }

        void IContentLoadHandler.Clear()
        {
            LandformDescrs.Clear();
        }
    }
}
