using JiongXiaGu.Collections;
using JiongXiaGu.Unity.Initializers;
using JiongXiaGu.Unity.Resources;
using JiongXiaGu.Unity.Resources.Binding;
using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

namespace JiongXiaGu.Unity.RectTerrain
{

    /// <summary>
    /// 地形资源描述;
    /// </summary>
    public class RectTerrainResourceDescription
    {
        [XmlAsset(@"Terrain\Landform", "地形资源定义")]
        public DescriptionCollection<LandformDescription> Landform { get; set; }


    }

    [DisallowMultipleComponent]
    public class RectTerrainResource : MonoBehaviour, IResourceIntegrateHandle
    {
        [SerializeField]
        private LandformResCreater landformResCreater;
        public LandformResCreater LandformResCreater => landformResCreater;
        private BindingSerializer bindingSerializer;

        private void Awake()
        {
            bindingSerializer = new BindingSerializer(typeof(RectTerrainResourceDescription));
        }

        void IResourceIntegrateHandle.Read(LoadableContent content, ITypeDictionary data, CancellationToken token)
        {
            token.ThrowIfCancellationRequested();

            RectTerrainResourceDescription description = (RectTerrainResourceDescription)bindingSerializer.Deserialize(content);
            data.Add(description);
        }

        void IResourceIntegrateHandle.Write(LoadableContent content, ITypeDictionary data, CancellationToken token)
        {
            token.ThrowIfCancellationRequested();

            RectTerrainResourceDescription description;
            if (data.TryGetValue(out description))
            {
                bindingSerializer.Serialize(content, description);
            }
        }

        void IResourceIntegrateHandle.SetNew(ITypeDictionary[] data, CancellationToken token)
        {
            throw new NotImplementedException();
        }

        void IResourceIntegrateHandle.Clear()
        {
            throw new NotImplementedException();
        }
    }
}
