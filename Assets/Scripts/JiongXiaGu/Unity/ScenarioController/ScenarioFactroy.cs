using JiongXiaGu.Unity.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JiongXiaGu.Unity.ScenarioController
{


    public class ScenarioFactory
    {
        private readonly Lazy<XmlSerializer<ScenarioDescription>> descriptionSerializer = new Lazy<XmlSerializer<ScenarioDescription>>();


    }
}
