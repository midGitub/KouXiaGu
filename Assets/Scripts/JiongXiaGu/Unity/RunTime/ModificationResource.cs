using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JiongXiaGu.Unity.RectTerrain;
using JiongXiaGu.Unity.Resources;
using JiongXiaGu.Unity.Resources.Binding;

namespace JiongXiaGu.Unity.RunTime
{

    /// <summary>
    /// 模组资源;
    /// </summary>
    public class ModificationResource
    {
        /// <summary>
        /// 地形资源描述;
        /// </summary>
        [XmlAsset(@"Terrain\Landform", "地形资源描述", null, true)]
        public DescriptionCollection<LandformDescription> LandformDescriptions { get; set; }
    }

    public class ModificationResourceFactroy
    {
        private readonly BindingSerializer<ModificationResource> serializer = new BindingSerializer<ModificationResource>();

        /// <summary>
        /// 读取模组资源;
        /// </summary>
        public ModificationResource Read(Modification modification)
        {
            ModificationResource resource = new ModificationResource();
            serializer.Serialize(modification.BaseContent, ref resource);
            return resource;
        }
    }
}
