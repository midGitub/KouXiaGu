using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KouXiaGu.Collections;

namespace KouXiaGu.Terrain3D
{


    public class BuildingRes : IDisposable
    {

        #region 已初始化合集(静态);

        /// <summary>
        /// 所有已经初始化完毕的道路信息;
        /// </summary>
        static readonly CustomDictionary<int, BuildingRes> initializedDictionary = new CustomDictionary<int, BuildingRes>();

        /// <summary>
        /// 所有已经初始化完毕的道路信息;
        /// </summary>
        public static IReadOnlyDictionary<int, BuildingRes> initializedInstances
        {
            get { return initializedDictionary; }
        }

        #endregion


        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }

}
