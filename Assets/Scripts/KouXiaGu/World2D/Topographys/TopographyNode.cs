using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace KouXiaGu.World2D
{

    public struct TopographyNode
    {

        public TopographyNode(int topographyID, Transform topographyObject)
        {
            this.topographyID = topographyID;
            this.topographyObject = topographyObject;
        }

        /// <summary>
        /// 当前保存的地貌;
        /// </summary>
        public int topographyID;

        /// <summary>
        /// 再次实例化的物体;
        /// </summary>
        public Transform topographyObject;

    }

}
