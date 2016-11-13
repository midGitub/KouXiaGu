using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using KouXiaGu.Map.HexMap;

namespace KouXiaGu.Map
{

    [DisallowMultipleComponent]
    public class GameHexMap : MonoBehaviour
    {

        [SerializeField, Tooltip("地图六边形的外径")]
        private float hexOuterDiameter = 2;

        //[SerializeField]
        //private NaviHexMap naviHexMap;

        [SerializeField]
        private PagingInfo mapPagingInfo;

        [SerializeField]
        private Transform target;

        /// <summary>
        /// 当前地图所用的六边形尺寸;
        /// </summary>
        private Hexagon mapHexagon;

        private DynamicMapDictionary<HexMapNode> map;

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
            map = new DynamicMapDictionary<HexMapNode>(mapPagingInfo);
        }

        private void Update()
        {
            IntVector2 mapPosition = GetMapPosition(target.position);
            map.UpdateMapData(mapPosition);
        }

        public void Add(Vector2 position, HexMapNode item)
        {
            IntVector2 mapPosition = GetMapPosition(position);
            map.Add(mapPosition, item);
        }

        /// <summary>
        /// 将浮点坐标转换成地图坐标;
        /// </summary>
        private PointPair GetMapPosition(Vector2 position)
        {
            return mapHexagon.TransfromPoint(position);
        }

    }

}
