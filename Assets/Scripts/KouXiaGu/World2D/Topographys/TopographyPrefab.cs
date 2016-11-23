﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace KouXiaGu.World2D
{

    [Serializable]
    public class TopographyPrefab
    {
        public int id
        {
            get { return topography.ID; }
        }

        /// <summary>
        /// 代表的预制物体;
        /// </summary>
        [SerializeField]
        public Topography topography = null;

        /// <summary>
        /// 地貌信息;
        /// </summary>
        public TopographyInfo Info
        {
            get { return topography.Info; }
        }
    }


}
