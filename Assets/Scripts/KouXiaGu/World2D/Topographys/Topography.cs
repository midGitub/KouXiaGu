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

        /// <summary>
        /// 唯一标识;
        /// </summary>
        public int ID
        {
            get { return id; }
        }



    }

}
