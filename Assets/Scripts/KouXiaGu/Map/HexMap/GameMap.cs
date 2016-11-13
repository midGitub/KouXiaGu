using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using KouXiaGu.Map.HexMap;

namespace KouXiaGu.Map
{

    [DisallowMultipleComponent]
    public class GameMap : MonoBehaviour
    {

        [SerializeField, Tooltip("地图六边形的外径")]
        private float hexOuterDiameter = 2;

        [SerializeField]
        private NaviHexMap naviHexMap;

        /// <summary>
        /// 当前地图所用的六边形尺寸;
        /// </summary>
        private Hexagon mapHexagon;

        /// <summary>
        /// 当前地图所用的六边形尺寸;
        /// </summary>
        public Hexagon MapHexagon
        {
            get { return mapHexagon; }
        }

        private void Awake()
        {
            mapHexagon = new Hexagon() { OuterDiameter = hexOuterDiameter };
        }


    }

}
