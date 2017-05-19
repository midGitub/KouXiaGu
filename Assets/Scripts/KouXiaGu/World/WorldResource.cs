//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using KouXiaGu.World.Map;

//namespace KouXiaGu.World
//{


//    public class WorldResource 
//    {

//        public static WorldResource Create()
//        {
//            var item = new WorldResource()
//            {
//                Map = MapResource.Read(),
//            };
//            return item;
//        }

//        public static IAsyncOperation<WorldResource> CreateAsync()
//        {
//            return new AsyncCreate();
//        }

//        /// <summary>
//        /// 地图数据;
//        /// </summary>
//        public MapResource Map { get; private set; }


//        class AsyncCreate : ThreadOperation<WorldResource>
//        {
//            public AsyncCreate()
//            {
//                Start();
//            }

//            protected override WorldResource Operate()
//            {
//                return Create();
//            }
//        }

//    }

//}
