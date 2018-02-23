using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JiongXiaGu.Unity.RectMaps;
using JiongXiaGu.Unity.Resources.Binding;

namespace JiongXiaGu.Unity.ScenarioController
{

    /// <summary>
    /// 游戏剧情;
    /// </summary>
    public struct Scenario
    {
        [XmlAsset(ScenarioFactory.ScenarioDescriptionName)]
        public ScenarioDescription Description { get; set; }
        [ProtoAsset(@"Map", true)]
        public RectMap Map { get; set; }
    }
}
