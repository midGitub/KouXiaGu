using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JiongXiaGu.Unity.ScenarioController
{

    /// <summary>
    /// 剧本描述;
    /// </summary>
    public struct ScenarioDescription
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public string Author { get; set; }
        public string Version { get; set; }
        public string Tags { get; set; }
        public string Message { get; set; }
    }
}
