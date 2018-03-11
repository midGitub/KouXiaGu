using JiongXiaGu.Unity.RectTerrain;
using JiongXiaGu.Unity.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JiongXiaGu.Unity.RunTime
{


    public class RunTimeResource
    {
        public IReadOnlyList<Modification> Modifications { get; private set; }
        public LandformResource LandformResource { get; private set; }

        /// <summary>
        /// 从模组初始化;
        /// </summary>
        public RunTimeResource(IEnumerable<Modification> modifications)
        {
            Modifications = new List<Modification>(modifications);
            //LandformResource = new LandformResource(modifications);
        }

    }
}
