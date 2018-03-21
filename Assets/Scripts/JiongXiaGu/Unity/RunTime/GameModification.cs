using JiongXiaGu.Unity.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JiongXiaGu.Unity.RunTime
{


    public static class GameModification
    {
        /// <summary>
        /// 所有模组;
        /// </summary>
        public static IReadOnlyList<Modification> Modifications { get; private set; }

        /// <summary>
        /// 所有激活的模组;
        /// </summary>
        public static IReadOnlyList<Modification> ActivatedModifications { get; private set; }

        /// <summary>
        /// 寻找所有模组;
        /// </summary>
        internal static void SearcheAll()
        {
            throw new NotImplementedException();
        }
    }
}
