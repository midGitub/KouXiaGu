﻿//using JiongXiaGu.Grids;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace JiongXiaGu.Unity.Maps
//{


//    public abstract class MapGenerator
//    {
//        MapDescription description;

//        public MapGenerator(MapDescription description)
//        {
//            this.description = description;
//        }

//        public MapDescription Description
//        {
//            get { return description; }
//            set { description = value; }
//        }

//        /// <summary>
//        /// 生成地图;
//        /// </summary>
//        public virtual OMap Generate()
//        {
//            OMap map = new OMap(description);
//            GenerateData(map.MapData.Data);
//            return map;
//        }

//        /// <summary>
//        /// 生成地图数据;
//        /// </summary>
//        public abstract void GenerateData(IDictionary<RectCoord, MapNode> mapData);
//    }
//}