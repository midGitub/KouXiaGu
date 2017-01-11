using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KouXiaGu.Terrain3D
{

    /// <summary>
    /// 在开始启动时准备的内容;
    /// </summary>
    public class TerrainPrepare : UnitySington<TerrainPrepare>
    {

        /// <summary>
        /// 是否准备完毕?
        /// </summary>
        public bool IsReady { get; private set; }


        void Start()
        {
            IsReady = true;
        }


    }

}
