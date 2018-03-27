using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JiongXiaGu.Unity.Maps;
using JiongXiaGu.Unity.Resources.BindingSerialization;
using JiongXiaGu.Grids;
using JiongXiaGu.Collections;

namespace JiongXiaGu.Unity.Scenarios
{

    /// <summary>
    /// 游戏剧情资源;
    /// </summary>
    public struct ScenarioResource
    {
        /// <summary>
        /// 描述;
        /// </summary>
        [XmlAsset(ScenarioFactory.ScenarioDescriptionName)]
        public ScenarioDescription Description { get; set; }

        /// <summary>
        /// 地图;
        /// </summary>
        [ProtoAsset(@"Map", true)]
        public SerializableDictionary<RectCoord, MapNode> Map { get; set; }
    }
}
