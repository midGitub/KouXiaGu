using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JiongXiaGu.Unity.ScenarioController
{

    public class Scenario
    {
        public ScenarioDescription Description { get; private set; }

        public Scenario(ScenarioDescription description)
        {
            Description = description;
        }
    }
}
