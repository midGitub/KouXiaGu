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
    public class RectTerrainResource : MonoBehaviour, IModificationInitializeHandle
    {
        [SerializeField]
        private LandformResCreater landformResCreater;
        public LandformResCreater LandformResCreater => landformResCreater;
        private BindingSerializer<RectTerrainResourceDescription> bindingSerializer;

        private void Awake()
        {
            bindingSerializer = new BindingSerializer<RectTerrainResourceDescription>();
            landformResCreater = new LandformResCreater();
        }

        void IModificationInitializeHandle.Initialize(IReadOnlyList<Modification> mods, CancellationToken token)
        {
            token.ThrowIfCancellationRequested();

            foreach (var mod in mods)
            {
                var description = Read(mod);

                landformResCreater.Add(mod, description.Landform);
            }
        }

        private RectTerrainResourceDescription Read(Modification content)
        {
            RectTerrainResourceDescription description = (RectTerrainResourceDescription)bindingSerializer.Deserialize(content.BaseContent);
            return description;
        }

        private void Write(Modification content, RectTerrainResourceDescription description)
        {
            bindingSerializer.Serialize(content.BaseContent, description);
        }

        private void Clear()
        {
            landformResCreater.Clear();
        }
    }
}
