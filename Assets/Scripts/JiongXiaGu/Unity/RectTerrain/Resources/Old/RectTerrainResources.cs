﻿//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using JiongXiaGu.Unity.Resources;

//namespace JiongXiaGu.Unity.RectTerrain
//{

//    /// <summary>
//    /// 地形资源;
//    /// </summary>
//    public class RectTerrainResources
//    {
//        public RectTerrainResources()
//        {
//            Landform = new Dictionary<int, LandformRes>();
//        }

//        /// <summary>
//        /// 不进行初始化赋值的构造函数;
//        /// </summary>
//        internal RectTerrainResources(bool none)
//        {
//        }

//        public Dictionary<int, LandformRes> Landform { get; set; }

//        public void Clear()
//        {
//            Landform.Clear();
//        }
//    }


//    public class RectTerrainResourcesSerializer
//    {
//        static RectTerrainResourcesSerializer()
//        {
//            DefaultInstance = new RectTerrainResourcesSerializer();
//        }

//        public RectTerrainResourcesSerializer()
//        {
//            LandformSerializer = new LandformResourceSerializer(new XmlSerializer<LandformRes[]>(), new ResourcesMultipleSearcher("Terrain/Landform"));
//        }

//        public static RectTerrainResourcesSerializer DefaultInstance { get; private set; }
//        public LandformResourceSerializer LandformSerializer { get; private set; }

//        /// <summary>
//        /// 序列化资源;
//        /// </summary>
//        public void Serialize(RectTerrainResources res)
//        {
//            LandformSerializer.Write(res.Landform);
//        }

//        /// <summary>
//        /// 反序列化资源;
//        /// </summary>
//        public RectTerrainResources Deserialize()
//        {
//            var res = new RectTerrainResources(true)
//            {
//                Landform = LandformSerializer.Read(),
//            };
//            return res;
//        }
//    }
//}
