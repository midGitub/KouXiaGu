using JiongXiaGu.Collections;
using JiongXiaGu.Unity.Initializers;
using JiongXiaGu.Unity.Resources;
using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

namespace JiongXiaGu.Unity.RectTerrain
{

    [DisallowMultipleComponent]
    public class RectTerrainResource : MonoBehaviour
    {
        [SerializeField]
        private LandformResCreater landformResCreater;
        public LandformResCreater LandformResCreater => landformResCreater;
        private LandformDescriptionSerializer landformDescriptionSerializer;

        private void Awake()
        {
            landformDescriptionSerializer = new LandformDescriptionSerializer();
        }

        //void IResourceLoadHandle.Read(LoadableContent loadableContent, ITypeDictionary info, CancellationToken token)
        //{
        //    DescriptionCollection<LandformDescription> landform;
        //    if (landformDescriptionSerializer.TryDeserialize(loadableContent.Content, out landform))
        //    {
        //        info.AddOrUpdate(landform);
        //    }
        //}
    }
}
