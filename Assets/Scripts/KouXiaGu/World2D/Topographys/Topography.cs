using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KouXiaGu.World2D.Map;
using UnityEngine;

namespace KouXiaGu.World2D
{

    [DisallowMultipleComponent]
    public class Topography : MonoBehaviour
    {

        /// <summary>
        /// 地貌的ID;
        /// </summary>
        [SerializeField]
        int id;

        /// <summary>
        /// 唯一标识;
        /// </summary>
        public int ID
        {
            get { return id; }
        }



    }

}
