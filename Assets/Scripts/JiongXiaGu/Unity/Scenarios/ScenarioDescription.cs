using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JiongXiaGu.Unity.Scenarios
{

    /// <summary>
    /// 剧本描述;
    /// </summary>
    public struct ScenarioDescription
    {
        public string ID { get; set; }
        public string LocName { get; set; }
        public string Author { get; set; }
        public string Version { get; set; }
        public string Tags { get; set; }
        public string LocMessage { get; set; }
    }
}
