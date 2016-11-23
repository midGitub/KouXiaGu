using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace KouXiaGu.World2D
{

    [DisallowMultipleComponent]
    public class Topography : MonoBehaviour
    {

        [SerializeField]
        int id;

        [SerializeField]
        TopographyInfo info;

        /// <summary>
        /// 唯一标识;
        /// </summary>
        public int ID
        {
            get { return id; }
        }

        /// <summary>
        /// 暂时放在这;
        /// </summary>
        public TopographyInfo Info
        {
            get { return info; }
        }

    }

}
