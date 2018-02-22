using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JiongXiaGu.Unity.Resources;

namespace JiongXiaGu.Unity.ScenarioController
{

    /// <summary>
    /// 游戏剧情;
    /// </summary>
    public class Scenario
    {
        public ScenarioDescription Description { get; private set; }
        public Content Resources { get; private set; }

        public Scenario(ScenarioDescription description)
        {
            Description = description;
        }
    }
}
