//using System.Collections.Generic;
//using KouXiaGu.Grids;

//namespace KouXiaGu.Terrain3D
//{

//    public class LandformData
//    {

//        /// <summary>
//        /// 节点不存在道路时放置的标志;
//        /// </summary>
//        const int EMPTY_MARK = 0;
//        const float DEFAULT_ANGLE = 0;


//        LandformData()
//        {
//        }

//        public LandformData(IDictionary<CubicHexCoord, MapNode> data)
//        {
//            this.Data = data;
//        }

//        /// <summary>
//        /// 当前查询的数据;
//        /// </summary>
//        public IDictionary<CubicHexCoord, MapNode> Data { get; internal set; }

//        public LandformNode this[CubicHexCoord coord]
//        {
//            get { return Data[coord].Landform; }
//            set
//            {
//                var node = Data[coord];
//                node.Landform = value;
//                Data[coord] = node;
//            }
//        }

//        /// <summary>
//        /// 更新这个节点的内容;
//        /// </summary>
//        public bool Update(CubicHexCoord coord, int id, float angle)
//        {
//            MapNode node;
//            if (Data.TryGetValue(coord, out node))
//            {
//                if (node.Landform.ID != id)
//                {
//                    node.Landform.ID = id;
//                    node.Landform.Angle = angle;
//                    Data[coord] = node;
//                    return true;
//                }
//            }
//            return false;
//        }

//        /// <summary>
//        /// 清除这个坐标的建筑物信息;
//        /// </summary>
//        public bool Destroy(CubicHexCoord coord)
//        {
//            MapNode node;
//            if (Data.TryGetValue(coord, out node))
//            {
//                if (node.Landform.ID != EMPTY_MARK)
//                {
//                    node.Landform.ID = EMPTY_MARK;
//                    node.Landform.Angle = DEFAULT_ANGLE;
//                    Data[coord] = node;
//                    return true;
//                }
//            }
//            return false;
//        }

//        /// <summary>
//        /// 是否存在建筑物?
//        /// </summary>
//        public bool Exist(CubicHexCoord coord)
//        {
//            MapNode node;
//            if (Data.TryGetValue(coord, out node))
//            {
//                return Exist(node.Landform);
//            }
//            return false;
//        }

//        /// <summary>
//        /// 是否存在建筑物?
//        /// </summary>
//        public bool Exist(LandformNode landform)
//        {
//            return landform.ID != EMPTY_MARK;
//        }

//        /// <summary>
//        /// 尝试获取到这个位置的道路信息;
//        /// </summary>
//        public bool TryGetValue(CubicHexCoord coord, out LandformNode landform)
//        {
//            MapNode node;
//            if (Data.TryGetValue(coord, out node))
//            {
//                landform = node.Landform;
//                if (Exist(landform))
//                    return true;
//            }
//            landform = default(LandformNode);
//            return false;
//        }

//    }

//}
