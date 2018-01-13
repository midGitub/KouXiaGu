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
            landformResCreater = new LandformResCreater();
        }

        void IResourceIntegrateHandle.Read(ModificationContent content, ITypeDictionary data, CancellationToken token)
        {
            token.ThrowIfCancellationRequested();

            RectTerrainResourceDescription description = (RectTerrainResourceDescription)bindingSerializer.Deserialize(content);
            data.Add(description);
        }

        /// <summary>
        /// 输出指定资源到目录;
        /// </summary>
        /// <exception cref="InvalidOperationException">未找到指定资源</exception>
        void IResourceIntegrateHandle.Write(ModificationContent content, ITypeDictionary data, CancellationToken token)
        {
            token.ThrowIfCancellationRequested();

            RectTerrainResourceDescription description;
            if (data.TryGetValue(out description))
            {
                bindingSerializer.Serialize(content, description);
            }
            else
            {
                throw new InvalidOperationException("未找到对应");
            }
        }

        void IResourceIntegrateHandle.SetNew(LoadableData[] collection, CancellationToken token)
        {
            foreach (var data in collection)
            {
                var rectTerrainResource = data.Data.Get<RectTerrainResourceDescription>();

                landformResCreater.Add(data.Content, rectTerrainResource.Landform);
            }
        }

        void IResourceIntegrateHandle.Clear()
        {
            landformResCreater.Clear();
        }
    }
}
