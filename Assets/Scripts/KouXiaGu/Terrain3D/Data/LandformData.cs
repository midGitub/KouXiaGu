using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KouXiaGu.Grids;

namespace KouXiaGu.Terrain3D
{

    public class LandformData
    {

        LandformData()
        {
        }

        public LandformData(IDictionary<CubicHexCoord, TerrainNode> data)
        {
            this.Data = data;
        }

        /// <summary>
        /// 当前查询的数据;
        /// </summary>
        public IDictionary<CubicHexCoord, TerrainNode> Data { get; internal set; }


        public LandformNode this[CubicHexCoord coord]
        {
            get { return Data[coord].Landform; }
            set
            {
                var node = Data[coord];
                node.Landform = value;
                Data[coord] = node;
            }
        }

        /// <summary>
        /// 设置到数据;
        /// </summary>
        public void Set(CubicHexCoord coord, int id, float angle)
        {
            var node = Data[coord];
            LandformNode info = node.Landform;

            info.ID = id;
            info.Angle = angle;

            Data[coord] = node;
        }

        /// <summary>
        /// 获取到该位置的地貌信息;
        /// </summary>
        public bool TryGetValue(CubicHexCoord coord, out LandformNode info)
        {
            TerrainNode node;
            if (Data.TryGetValue(coord, out node))
            {
                info = node.Landform;
                if (info.Exist())
                    return true;
                else
                    return false;
            }
            info = default(LandformNode);
            return false;
        }

    }

}
