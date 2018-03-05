using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JiongXiaGu.Unity.Maps;
using JiongXiaGu.Unity.Resources.Binding;
using JiongXiaGu.Grids;

namespace JiongXiaGu.Unity.Scenarios
{

    /// <summary>
    /// 游戏剧情;
    /// </summary>
    public struct Scenario
    {
        [XmlAsset(ScenarioFactory.ScenarioDescriptionName)]
        public ScenarioDescription Description { get; set; }
        [ProtoAsset(@"Map", true)]
        public Map<RectCoord> Map { get; set; }
    }
}
