using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace KouXiaGu.World2D
{

    [Serializable]
    public class TopographyPrefab
    {
        /// <summary>
        /// 定义的真正的ID;
        /// </summary>
        public int id = 0;

        /// <summary>
        /// 代表的预制物体;
        /// </summary>
        public Transform prefab = null;

        /// <summary>
        /// 地貌;
        /// </summary>
        public Topography topography;
    }


}
