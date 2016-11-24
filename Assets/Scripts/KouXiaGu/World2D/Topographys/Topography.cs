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
        /// 暂时放在这;
        /// </summary>
        [SerializeField]
        TopographyInfo info;


        /// <summary>
        /// 暂时放在这;
        /// </summary>
        public TopographyInfo Info
        {
            get { return info; }
        }
        /// <summary>
        /// 唯一标识;
        /// </summary>
        public int ID
        {
            get { return id; }
        }
        public ShortVector2 MapPoint { get; set; }

        ///// <summary>
        ///// 当附近节点路径发生变化时调用;
        ///// </summary>
        //public abstract void RefreshRoad(IMap<ShortVector2, WorldNode> map);

    }

}
