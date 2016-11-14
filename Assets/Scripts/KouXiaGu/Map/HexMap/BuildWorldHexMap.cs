using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using KouXiaGu.Map.HexMap;

namespace KouXiaGu.Map
{

    [DisallowMultipleComponent]
    public class BuildWorldHexMap : MonoBehaviour
    {

        [SerializeField, Tooltip("地图六边形的外径")]
        private float hexOuterDiameter = 2;

        [SerializeField, Tooltip("预制地图分区信息")]
        private MapBlockInfo mapPagingInfo;

        [SerializeField]
        private Transform target;

        /// <summary>
        /// 当前地图所用的六边形尺寸;
        /// </summary>
        private Hexagon mapHexagon;

        //private DynamicMapDictionary<HexMapNode> prefabMap;

        /// <summary>
        /// 当前地图所用的六边形尺寸;
        /// </summary>
        public Hexagon MapHexagon
        {
            get { return mapHexagon; }
        }

        ///// <summary>
        ///// 
        ///// </summary>
        //public IReadOnlyMap<IntVector2, HexMapNode> DynamicMap
        //{
        //    get { return prefabMap; }
        //}

        //private void Awake()
        //{
        //    mapHexagon = new Hexagon() { OuterDiameter = hexOuterDiameter };
        //    //prefabMap = new DynamicMapDictionary<HexMapNode>(mapPagingInfo);
        //}

        //private void Start()
        //{
        //    UpdateMap(false);
        //}

        //private void Update()
        //{
        //    UpdateMap();
        //}

        //private void OnDestroy()
        //{
        //    prefabMap.SaveMapPagingAll();
        //}

        //public void Add(Vector2 position, HexMapNode item)
        //{
        //    IntVector2 mapPosition = GetMapPosition(position);
        //    prefabMap.Add(mapPosition, item);
        //}

        ///// <summary>
        ///// 将浮点坐标转换成地图坐标;
        ///// </summary>
        //private PointPair GetMapPosition(Vector2 position)
        //{
        //    return mapHexagon.TransfromPoint(position);
        //}

        //private void UpdateMap(bool check = true)
        //{
        //    IntVector2 mapPosition = GetMapPosition(target.position);
        //    prefabMap.UpdateMapData(mapPosition, check);
        //}


        ///// <summary>
        ///// 向地图添加
        ///// </summary>
        //public void Add(IntVector2 position, HexMapNode item)
        //{

        //}


    }

}
