using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace KouXiaGu.World2D
{

    [DisallowMultipleComponent]
    public class Road : MonoBehaviour
    {

        [SerializeField]
        HexDirection roadDirection;

        [ShowOnlyProperty]
        string Dic { get { return roadDirection.ToString(); } }

        /// <summary>
        /// 本身是否存在道路;
        /// </summary>
        public bool HaveRoad
        {
            get { return (roadDirection & HexDirection.Self) > 0; }
        }

        public void SetState(HexDirection roadDirection)
        {
            this.roadDirection = roadDirection;
            Debug.Log(transform.position.ToString() + roadDirection.ToString());
        }

    }

}
