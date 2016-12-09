using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace KouXiaGu.HexTerrain
{


    /// <summary>
    /// 对基本地形进行修改的
    /// </summary>
    public class RevisRenderer : UnitySingleton<RevisRenderer>
    {
        RevisRenderer() { }




        #region 烘焙队列;


        IEnumerator Baking()
        {
            yield break;
        }

        #endregion

    }

}
